using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    public float speed = 2f;
    public bool rmvParentOnReset = true;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    private bool isMoving = false;

    //Sometimes we need information about the grasping state
    protected BaseGraspController graspController;

    protected void Start () {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        graspController = GetComponent<BaseGraspController>();

        EventManager.StartListening("ResetTools", OnResetTools);
    }

    protected void Update() {
        if (isMoving) {
            Move(initialPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.time * 2);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Desk" && !graspController.IsGrabbed()) {
            isMoving = true;
        }
    }

    private void Move(Vector3 targetPosition) {
        float distance = Vector3.Distance(transform.position, targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * distance * speed);
        rb.useGravity = false;
        rb.isKinematic = true;

        if (transform.position == initialPosition) {
            isMoving = false;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    protected void OnResetTools() {
        if (rmvParentOnReset) {
            transform.parent = null;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isMoving = false;
        rb.useGravity = true;
    }
}

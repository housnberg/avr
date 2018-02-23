using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    private const float MOVEMENT_THRESHOLD = 0.25f;

    public float moveSpeed = 2f;
    public bool rmvParentOnReset = true;
    public AudioSource[] tableHitSounds;

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

        EventManager.StartListening(ComplexBombEvent.RESET_TOOLS, OnReset);
    }

    protected void Update() {
        if (isMoving) {
            Move(initialPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.time * 2);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Desk" && !graspController.IsGrabbed()) {
            if (tableHitSounds.Length != 0 && !isMoving) {
                AudioSource currentSound = tableHitSounds[(int)Random.Range(0, tableHitSounds.Length - 1)];
                AudioSource.PlayClipAtPoint(currentSound.clip, transform.position, currentSound.volume);
            }
            BaseHitableObject hitable = GetComponent<BaseHitableObject>();
            if (hitable != null) {
                hitable.SetCurrentlyHitable(true);
            }
            isMoving = true;
        }
    }

    private void Move(Vector3 targetPosition) {
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < MOVEMENT_THRESHOLD) {
            distance = MOVEMENT_THRESHOLD;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * distance * moveSpeed);
        rb.useGravity = false;
        rb.isKinematic = true;

        if (transform.position == initialPosition) {
            isMoving = false;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    protected void OnReset() {
        if (rmvParentOnReset) {
            transform.parent = null;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isMoving = false;
        rb.useGravity = true;

        BaseHitableObject hitable = GetComponent<BaseHitableObject>();
        if(hitable != null) {
            hitable.SetCurrentlyHitable(true);
        }
    }
}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ToolHitableObject : BaseHitableObject {

    public Vector3 targetPosition;
    public Transform anchor;
    
    private Vector3 initialPosition;
    private Rigidbody rb;
    private bool isMoving;
    private bool isRotating;

    new void Start() {
        base.Start();

        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;

        if (anchor != null) {
            targetPosition = anchor.position;
        }
    }

    new void Update() {
        base.Update();

        if (shouldDoHitAction()) {
            isMoving = true;
        }
        if (isMoving) {
            Move(targetPosition);
        }

        if (transform.position == targetPosition) {
            isMoving = false;
            rb.isKinematic = false;
        }
    }

    private void Move(Vector3 targetPosition) {
        float distance = Vector3.Distance(transform.position, targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * distance * speed);
        rb.useGravity = false;
        rb.isKinematic = true;
    }
}

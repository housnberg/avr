using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ToolHitableObject : BaseHitableObject {

    private const float MOVEMENT_THRESHOLD = 0.25f;

    public Vector3 targetPosition;
    public Transform anchor;
    
    private Vector3 initialPosition;
    private Rigidbody rb;
    private bool isMoving;

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
            isCurrentlyHitable = false;
        }

        if (isMoving) {
            Move(targetPosition);
			EventManager.TriggerEvent ("TutorialTools");
        }

        if (targetPosition.Equals(transform.position)) {
            isCurrentlyHitable = false;
        }
        if (initialPosition == transform.position) {
            isCurrentlyHitable = true;
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

        if (transform.position.Equals(targetPosition)) {
            isMoving = false;
            rb.isKinematic = false;
            isCurrentlyHitable = true;
        }
    }
}

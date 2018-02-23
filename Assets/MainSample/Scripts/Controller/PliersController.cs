using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersController : BaseController {

    private enum State { OPEN, CLOSED };

    public float openingThreshold = 2.5f;
    public AudioSource cutSound;

    private bool leftHandleCollided = false;
    private bool rightHandleCollided = false;
    private Collision lastCollision;

    private HingeJoint handleRightHj;
    private JointSpring spring;
    private Transform adjustment;

    private State currentState = State.OPEN;
    private State lastState;

    new void Start() {
        base.Start();

        handleRightHj = this.GetComponentInChildren<HingeJoint>();
        adjustment = this.transform.GetChild(0);
    }

    new void Update() {
        base.Update();
        ChangeState();

        JointSpring spring = handleRightHj.spring;
        Quaternion currentRotation = transform.rotation;
        adjustment.rotation = currentRotation * Quaternion.Euler(0, -0.5f * spring.targetPosition, 0);

        if (lastState == State.OPEN && currentState == State.CLOSED) {
            AudioSource.PlayClipAtPoint(cutSound.clip, transform.position, cutSound.volume);
        }
    }

    void OnTriggerEnter(Collider other) {
        Cable cable = other.transform.GetComponent<Cable>();
        if (cable != null && !isPliersOpen()) {
            cable.resetStartAndEndPoints(cable.jointBreakForce);
        }
    }

    void OnTriggerExit(Collider other) {
        Cable cable = other.transform.GetComponent<Cable>();
        if (cable != null) {
            cable.resetStartAndEndPoints(Mathf.Infinity);
        }
    }

    public bool isPliersOpen() {
        return handleRightHj.spring.targetPosition > openingThreshold;
    }

    private void ChangeState() {
        lastState = currentState;
        if (handleRightHj.spring.targetPosition > openingThreshold) {
            if (currentState != State.OPEN) {
                currentState = State.OPEN;
            }
        } else {
            if (currentState != State.CLOSED) {
                currentState = State.CLOSED;
            }
        }
    }

}

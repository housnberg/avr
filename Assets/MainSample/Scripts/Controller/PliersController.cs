using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersController : MonoBehaviour {

    private bool leftHandleCollided = false;
    private bool rightHandleCollided = false;
    private Collision lastCollision;

    private HingeJoint handleRightHj;
    private JointSpring spring;
    private Transform adjustment;

    void Start() {
        handleRightHj = this.GetComponentInChildren<HingeJoint>();
        adjustment = this.transform.GetChild(0);
    }

    void Update() {
        JointSpring spring = handleRightHj.spring;
        Quaternion currentRotation = transform.rotation;
        adjustment.rotation = currentRotation * Quaternion.Euler(0, -0.5f * spring.targetPosition, 0);
    }

    void OnTriggerEnter(Collider other) {
        WireController wireController = other.transform.GetComponent<WireController>();
        if (wireController != null && !isPliersOpen()) {
            wireController.resetStartAndEndPoints(wireController.jointBreakForce);
        }
    }

    void OnTriggerExit(Collider other) {
        WireController wireController = other.transform.GetComponent<WireController>();
        if (wireController != null) {
            wireController.resetStartAndEndPoints(Mathf.Infinity);
        }
    }

    public bool isPliersOpen() {
        return handleRightHj.spring.targetPosition >= 10;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersController : BaseController {

    private bool leftHandleCollided = false;
    private bool rightHandleCollided = false;
    private Collision lastCollision;

    private HingeJoint handleRightHj;
    private JointSpring spring;
    private Transform adjustment;

    new void Start() {
        base.Start();

        handleRightHj = this.GetComponentInChildren<HingeJoint>();
        adjustment = this.transform.GetChild(0);
    }

    new void Update() {
        base.Update();

        JointSpring spring = handleRightHj.spring;
        Quaternion currentRotation = transform.rotation;
        adjustment.rotation = currentRotation * Quaternion.Euler(0, -0.5f * spring.targetPosition, 0);
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
        return handleRightHj.spring.targetPosition >= 10;
    }

}

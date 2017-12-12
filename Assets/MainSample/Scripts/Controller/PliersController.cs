using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PliersController : MonoBehaviour {

    private bool leftHandleCollided = false;
    private bool rightHandleCollided = false;
    private Collision lastCollision;

    void OnTriggerEnter(Collider other) {
        WireController wireController = other.transform.GetComponent<WireController>();
        if (wireController != null) {
            wireController.resetStartAndEndPoints(wireController.jointBreakForce);
        }
    }

    void OnTriggerExit(Collider other) {
        WireController wireController = other.transform.GetComponent<WireController>();
        if (wireController != null) {
            wireController.resetStartAndEndPoints(Mathf.Infinity);
        }
    }



}

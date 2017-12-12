using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour {

    public float jointBreakForce = 100;

    private bool hasHandleLeftCollided;
    private bool hasHandleRightCollided;

    private Transform startPoint;
    private Transform endPoint;

    // Disable physics for all other elements except for Pliers
    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag != TagConstants.HANDLE_LEFT_COLLIDER && collision.transform.tag != TagConstants.HANDLE_RIGHT_COLLIDER) {
            Physics.IgnoreCollision(collision.collider, this.transform.GetComponent<Collider>());
        }
    }

    public void setStartPoint(Transform startPoint) {
        this.startPoint = startPoint;
    }

    public void setEndPoint(Transform endPoint) {
        this.endPoint = endPoint;
    }
    
    public void resetStartAndEndPoints(float value) {
        FixedJoint[] startJoints = startPoint.GetComponents<FixedJoint>();
        FixedJoint[] endJoints = endPoint.GetComponents<FixedJoint>();

        foreach (FixedJoint joint in startJoints) {
            joint.breakForce = value;
        }
        foreach (FixedJoint joint in endJoints) {
            joint.breakForce = value;
        }
    }
}

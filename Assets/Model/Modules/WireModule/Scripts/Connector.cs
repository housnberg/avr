using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

    private FixedJoint[] joints;
    
	void Start () {
        joints = GetComponents<FixedJoint>();
	}

    void OnJointBreak(float breakForce) {
        FixedJoint[] currentJoints = GetComponents<FixedJoint>();

        foreach (FixedJoint joint in joints) {
            if (joint != null) {
                joint.connectedBody.useGravity = true;
            }
        }
        Destroy(gameObject);
    }
}

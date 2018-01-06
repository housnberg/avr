using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

    private FixedJoint[] joints;

	// Use this for initialization
	void Start () {
        joints = GetComponents<FixedJoint>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnJointBreak(float breakForce) {
        FixedJoint[] currentJoints = GetComponents<FixedJoint>();

        foreach (FixedJoint joint in joints) {
            joint.connectedBody.useGravity = true;
        }
        Destroy(gameObject);
            
    }
}

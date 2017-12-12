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

        /*
        if (currentJoints[0] == null) {
            joints[0].connectedBody.useGravity = true;
        } else {
            joints[1].connectedBody.useGravity = true;
        }
        joints[1].connectedBody.useGravity = true;
        joints[0].connectedBody.useGravity = true;

    */
        Destroy(gameObject);
        //gameObject.SetActive(false);
            
    }
}

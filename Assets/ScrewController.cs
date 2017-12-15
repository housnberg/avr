using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewController : MonoBehaviour {

	HingeJoint hJoint;

	BoxCollider recess;

	// Use this for initialization
	void Start () {
		hJoint = GetComponent<HingeJoint> ();
		recess = GetComponent<BoxCollider> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "ScrewdriverTip") {
			print ("Getting close");
			if (recess.bounds.Contains (other.bounds.center)) {
				print ("Tip insertion successful");
				hJoint.connectedBody = other.transform.parent.parent.GetComponent<Rigidbody> ();

			}
		}
	}

}

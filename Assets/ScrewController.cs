using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewController : MonoBehaviour {

	BoxCollider recess;

	// Use this for initialization
	void Start () {
		recess = GetComponent<BoxCollider> ();
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "ScrewdriverTip") {
			ScrewdriverGraspController screwdriverController = other.transform.parent.parent.GetComponent<ScrewdriverGraspController> ();
			screwdriverController.connectScrew (transform.parent.gameObject, recess.bounds.center);
		}
	}

}

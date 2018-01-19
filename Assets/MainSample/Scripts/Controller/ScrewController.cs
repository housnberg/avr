using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewController : MonoBehaviour {

	GameObject metalPlate;
	BoxCollider recess;
	public bool unscrewed;

	// Use this for initialization
	void Start () {
		metalPlate = transform.parent.parent.gameObject;
		recess = GetComponent<BoxCollider> ();
		unscrewed = false;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "ScrewdriverTip") {
			ScrewdriverGraspController screwdriverController = other.transform.parent.parent.GetComponent<ScrewdriverGraspController> ();
			screwdriverController.connectScrew (transform.parent.gameObject, recess.bounds.center);
		}
	}

	public void Disconnect () {
		unscrewed = true;
		metalPlate.GetComponent<PlateController> ().CheckScrews ();
		enabled = false;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class CaseButtonController : MonoBehaviour {

	bool isPressed;
	Outline outlineScript;

	// Use this for initialization
	void Start () {
		isPressed = false;
		this.outlineScript = GetComponent<Outline> ();
		this.outlineScript.enabled = false;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "FingerTip") {
			isPressed = true;
			this.outlineScript.enabled = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "FingerTip") {
			isPressed = false;
			this.outlineScript.enabled = false;
		}
	}

	public bool IsPressed() {
		return isPressed;
	}

}

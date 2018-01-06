using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseButtonController : MonoBehaviour {

	bool isPressed;

	// Use this for initialization
	void Start () {
		isPressed = false;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "FingerTip") {
			isPressed = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "FingerTip") {
			isPressed = false;
		}
	}

	public bool IsPressed() {
		return isPressed;
	}

}

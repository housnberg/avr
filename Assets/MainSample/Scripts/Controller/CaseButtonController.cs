using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class CaseButtonController : MonoBehaviour {

    public AudioSource clickEnterSound;
    public AudioSource clickExitSound;

	bool isPressed;
	Outline outlineScript;
    
	void Start () {
		isPressed = false;
		this.outlineScript = GetComponent<Outline> ();

        EnableOutline(false);
    }

	void OnTriggerEnter (Collider other) {
		if (other.tag == "FingerTip" && !isPressed) {
			isPressed = true;
            EnableOutline(true);

            if (clickEnterSound != null) {
                AudioSource.PlayClipAtPoint(clickEnterSound.clip, transform.position, clickEnterSound.volume);
            }
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "FingerTip" && isPressed) {
			isPressed = false;

            EnableOutline(false);

            if (clickExitSound != null) {
                AudioSource.PlayClipAtPoint(clickExitSound.clip, transform.position, clickExitSound.volume);
            }
        }
	}

	public bool IsPressed() {
		return isPressed;
	}

    private void EnableOutline(bool enabled) {
        if (outlineScript != null) {
            this.outlineScript.enabled = enabled;
        }
    } 
}

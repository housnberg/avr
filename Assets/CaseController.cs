using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseController : MonoBehaviour {


	GameObject lid;
	bool isOpen;

	CaseButtonController leftButton;
	CaseButtonController rightButton;
	
	GameObject[] fingerTips;

	// Use this for initialization
	void Start () {
		lid = GameObject.FindGameObjectWithTag ("CaseLid");
		fingerTips = GameObject.FindGameObjectsWithTag ("FingerTip");
		leftButton = GameObject.FindGameObjectWithTag("LeftCaseButton").GetComponent<CaseButtonController>();
		rightButton = GameObject.FindGameObjectWithTag("RightCaseButton").GetComponent<CaseButtonController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isOpen && CheckCollider()) {
			StartCoroutine ("Open", 1.5f);
		}
	}

	IEnumerator Open (float time) {
		isOpen = true;

		float elapsedTime = 0.0f;

		Quaternion startingRotation = lid.transform.rotation;
		Quaternion targetRotation =  startingRotation * Quaternion.Euler (new Vector3(-90.0f, 0.0f, 0.0f));

		while (elapsedTime < time) {
			elapsedTime += Time.deltaTime;
			lid.transform.rotation = Quaternion.Lerp(startingRotation, targetRotation,  (elapsedTime / time)  );
			yield return new WaitForEndOfFrame ();
		}
	}

	bool CheckCollider() {
		return (leftButton.IsPressed() && rightButton.IsPressed());

//		bool leftPressed = false;
//		bool rightPressed = false;
//		foreach (GameObject fingerTip in fingerTips) {
//			if (leftButton.bounds.Intersects(fingerTip.GetComponent<CapsuleCollider>().bounds)) {
//				print ("left hit");
//				leftPressed = true;
//			}
//			if (rightButton.bounds.Intersects(fingerTip.GetComponent<CapsuleCollider>().bounds)) {
//				print ("right hit");
//				rightPressed = true;
//			}
//		}
//		return (leftPressed && rightPressed);
	}
}

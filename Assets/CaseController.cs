using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseController : MonoBehaviour {

	private HingeJoint hinge;
	private bool isOpen;

	private CaseButtonController leftButton;
	private CaseButtonController rightButton;
	
	private GameObject[] fingerTips;
    

	// Use this for initialization
	void Start () {
		hinge = GameObject.FindGameObjectWithTag(TagConstants.CASE_LID).GetComponent<HingeJoint>();
		fingerTips = GameObject.FindGameObjectsWithTag (TagConstants.FINGER_TIP);
		leftButton = GameObject.FindGameObjectWithTag(TagConstants.LEFT_CASE_BUTTON).GetComponent<CaseButtonController>();
        rightButton = GameObject.FindGameObjectWithTag(TagConstants.RIGHT_CASE_BUTTON).GetComponent<CaseButtonController>();   
	}
	
	// Update is called once per frame
	void Update () {
		if (!isOpen && CheckCollider()) {
            JointSpring spring = hinge.spring;
            spring.targetPosition = 120;
            hinge.spring = spring;
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

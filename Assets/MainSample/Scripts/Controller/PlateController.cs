using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
	List<GameObject> screws = new List<GameObject> ();
	// Use this for initialization
	void Start () {
		print ("PlateController started");
		foreach (Transform child in transform) {
			if (child.name.StartsWith("Screw")) {
				print (child.name + "was added");
				screws.Add (child.gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void CheckScrews ()
	{	
		print ("Checking Screws");
		bool allUnscrewed = true;
		foreach (GameObject screw in screws) {
			print (screw.name);
			if (screw.GetComponentInChildren<ScrewController> ().unscrewed == false) {
				allUnscrewed = false;
			}
		}
		if (allUnscrewed) {
			print ("all unscrewed, detaching plate");
			DetachPlate ();
		}
	}

	void DetachPlate(){
		// ignore collisions with Leap hand
		GameObject leapHand = GameObject.FindWithTag ("Hand");
		foreach (Collider cLeap in leapHand.GetComponentsInChildren<Collider> ()) {
			foreach (Collider cPlate in GetComponentsInChildren<Collider> ()) {
				Physics.IgnoreCollision(cPlate, cLeap);
			}
		}

		Rigidbody rig = this.GetComponent<Rigidbody> ();
		rig.isKinematic = false;
		rig.AddForce (transform.up * 100);
		EventManager.TriggerEvent ("MetalplateRemoved");
	}

}
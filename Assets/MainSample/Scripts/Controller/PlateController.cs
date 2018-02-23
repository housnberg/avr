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
//		// ignore collisions with Leap hand
//		Collider[] cls = GameObject.FindWithTag ("Hand").GetComponentsInChildren<Collider>();
//		Collider[] pCls = this.GetComponentsInChildren<Collider> ();
//		foreach (Collider cl in cls) {
//			foreach (Collider pCl in pCls) {
//				if (cl != null && pCl != null) {
//					Physics.IgnoreCollision(pCl, cl, true);
//				}
//			}
//		}

		Rigidbody rig = this.GetComponent<Rigidbody> ();
		rig.isKinematic = false;
		EventManager.TriggerEvent (ComplexBombEvent.METALPLATE_REMOVED);
		rig.AddForce ((transform.up-transform.forward) * 200);
	}

}
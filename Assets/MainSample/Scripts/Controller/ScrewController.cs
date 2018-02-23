using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewController : MonoBehaviour {

	GameObject metalPlate;
	BoxCollider recess;
	public bool unscrewed;

    private bool connected;
    private ScrewdriverGraspController screwdriverController;


    void Start () {
		metalPlate = transform.parent.parent.gameObject;
		recess = GetComponent<BoxCollider> ();
		unscrewed = false;
	}

    void Update() {
        if (connected) {
            BaseHitableObject hitable = screwdriverController.GetHitable();
            if (hitable != null) {
                hitable.SetCurrentlyHitable(false);
            }
        }
    }

    void OnTriggerEnter (Collider other) {
		if (other.tag == "ScrewdriverTip") {
			screwdriverController = other.transform.parent.parent.GetComponent<ScrewdriverGraspController> ();
			screwdriverController.connectScrew (transform.parent.gameObject, recess.bounds.center);

            connected = true;
            BaseHitableObject hitable = screwdriverController.GetHitable();
		}
	}

    void OnTriggerExit(Collider other) {
        if (other.tag == "ScrewdriverTip") {
            connected = false;
        }
    }

    public void Disconnect () {
		unscrewed = true;
		metalPlate.GetComponent<PlateController> ().CheckScrews ();
		enabled = false;
        connected = false;
	}

}

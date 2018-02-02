using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class ScrewdriverGraspController : BaseGraspController {

    public float looseningThreshold = 0.2f;

	GraspController[] hands;

	Rigidbody rb;
	GameObject connectedScrew = null;
	Transform tip;

	Quaternion startRotation;
	float rotationDelta = 0f;
	float overallRotationDelta = -180f;
	float rotationThreshold = 500f;
	Vector3 newPosition = Vector3.zero;
    Vector3 startHandPosition;

	float screwLength;
	Vector3 firstContactPosition;

    public override void DoGraspAction ()
	{
        if (connectedScrew != null) {
            if ((hand.PalmPosition.ToUnityScaled() - startHandPosition).magnitude > looseningThreshold) {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                connectedScrew.transform.parent = null;
                connectedScrew = null;
            }
			if (rotationDelta >= 0) {
				rb.constraints &= ~RigidbodyConstraints.FreezeRotationY;
			} else if (rotationDelta < 0) {
				rb.constraints |= RigidbodyConstraints.FreezeRotationY;
			}

			if (overallRotationDelta > rotationThreshold) {
				disconnectScrew ();
			} else {
				rotationDelta = (transform.rotation * Quaternion.Inverse (startRotation)).y;
				print (overallRotationDelta);
			}
			// move screwdriver upwards while unscrewing
			newPosition = firstContactPosition + new Vector3(.0f, (1/rotationThreshold*overallRotationDelta)*screwLength, .0f);
			transform.position = newPosition;
		}

	}

	public override void CancelGraspAction ()
	{
		if (connectedScrew != null) {
			if (rb.constraints != RigidbodyConstraints.FreezeAll)
				rb.constraints |= RigidbodyConstraints.FreezeRotationY;

			overallRotationDelta += Quaternion.Angle (startRotation, transform.rotation);
			startRotation = transform.rotation;
		}
	}

	public override void Init ()
	{
		rb = GetComponent<Rigidbody> ();
		tip = transform.Find ("TipRef");
		screwLength = -1;
	}

	public void connectScrew (GameObject screw, Vector3 recessCenter) 
	{
		print ("connecting");
		float tipDistance = (tip.position - transform.position).magnitude;
		transform.position = recessCenter + new Vector3(.0f, tipDistance, .0f);
		transform.rotation = Quaternion.identity;

		rb.constraints = RigidbodyConstraints.FreezeAll;
		connectedScrew = screw;
		connectedScrew.transform.parent = transform;

		if (screwLength == -1) {
			screwLength = (tip.position - connectedScrew.transform.position).magnitude;
			firstContactPosition = transform.position;
		}
		
		hands = GameObject.FindObjectsOfType<GraspController>();
        startHandPosition = hand.PalmPosition.ToUnityScaled();
        foreach (GraspController hand in hands) {
			hand.requestRelease();
		}

	}

	public void disconnectScrew ()
	{
		print ("disconnecting");
		connectedScrew.GetComponentInChildren<ScrewController> ().Disconnect ();
		connectedScrew.GetComponentInChildren<BoxCollider> ().enabled = false;
		this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

		connectedScrew.GetComponent<CapsuleCollider> ().enabled = true;
		connectedScrew.GetComponent<Rigidbody> ().isKinematic = false;
		connectedScrew.transform.parent = null;
		connectedScrew = null;

	}
}

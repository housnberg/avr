using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSphereController : MonoBehaviour {

	Vector3 initScale;

	public bool scaleToDistance;

	// Use this for initialization
	void Start () {
		initScale = transform.localScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		RaycastHit hit2;
		Camera cam = Camera.main;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit2)) {
			print (transform.parent.name);
			transform.position = hit2.point;

			if (scaleToDistance) {
				Plane plane = new Plane(cam.transform.forward, cam.transform.position); 
				float distance = plane.GetDistanceToPoint(hit2.point);
				transform.localScale =  initScale * distance; 
			}
		}
	}
}

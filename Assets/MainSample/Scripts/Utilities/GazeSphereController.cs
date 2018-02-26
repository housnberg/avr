using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeSphereController : MonoBehaviour {

	Vector3 initScale;

	public bool scaleToDistance;
    
	void Start () {
		initScale = transform.localScale;
	}
	
	void FixedUpdate () {
		RaycastHit hit;
		Camera cam = Camera.main;
		if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit)) {
			transform.position = hit.point;

			if (scaleToDistance) {
				Plane plane = new Plane(cam.transform.forward, cam.transform.position); 
				float distance = plane.GetDistanceToPoint(hit.point);
				transform.localScale =  initScale * distance; 
			}
		}
	}
}

using UnityEngine;
using System.Collections;

public class ResetBottle : MonoBehaviour {

  private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
    this.initialPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
    if (Input.GetKeyUp(KeyCode.Space))
    {
      this.GetComponent<Rigidbody>().useGravity = false;
      this.GetComponent<Rigidbody>().isKinematic = true;
      this.transform.position = new Vector3(0, -100, 0);
      this.transform.Rotate(new Vector3(0f, 0f, 0f));

      GraspableObject graspable = this.GetComponent<GraspableObject>();
      graspable.ResetVariables();
      graspable.ResetPositionAndOrientation(0.0f, new Vector3(0.0f, .19f, .2f));

      this.transform.position = this.initialPosition;
      this.GetComponent<Rigidbody>().isKinematic = false;
    }

	}
}

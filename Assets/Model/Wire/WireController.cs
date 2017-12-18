using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour {

    public float jointBreakForce = 100;
    public GameObject particleSystemPrefab;
    private GameObject particleSystemInstance;

    private bool hasHandleLeftCollided;
    private bool hasHandleRightCollided;

    private Transform startPoint;
    private Transform endPoint;
    private bool isCutted;

    // Disable physics for all other elements except for Pliers
    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag != TagConstants.HANDLE_LEFT_COLLIDER && collision.transform.tag != TagConstants.HANDLE_RIGHT_COLLIDER) {
            Physics.IgnoreCollision(collision.collider, this.transform.GetComponent<Collider>());
        }
    }

    public void setStartPoint(Transform startPoint) {
        this.startPoint = startPoint;
    }

    public void setEndPoint(Transform endPoint) {
        this.endPoint = endPoint;
    }
    
    public void resetStartAndEndPoints(float value) {
        FixedJoint[] startJoints = startPoint.GetComponents<FixedJoint>();
        FixedJoint[] endJoints = endPoint.GetComponents<FixedJoint>();

        foreach (FixedJoint joint in startJoints) {
            joint.breakForce = value;
        }
        foreach (FixedJoint joint in endJoints) {
            joint.breakForce = value;
        }
    }

    void Start() {
        if (particleSystemPrefab != null) {
            particleSystemInstance = Instantiate(particleSystemPrefab);
            particleSystemInstance.transform.position = endPoint.position;
            particleSystemInstance.transform.parent = this.transform;
            particleSystemInstance.gameObject.SetActive(false);
        }
    }

    void Update() {
        if (endPoint == null && startPoint == null) {
            Destroy(gameObject);
        } else if (endPoint == null && !isCutted) {
            if (particleSystemInstance != null) {
                particleSystemInstance.SetActive(true);
            }
            startPoint.Rotate(new Vector3(0, 0, 1), Random.Range(25f, 40f));
            isCutted = true;
        } else if (startPoint == null && !isCutted) {
            endPoint.Rotate(new Vector3(0, 0, 1), Random.Range(25f, 40f));
            isCutted = true;
        }
    }
}

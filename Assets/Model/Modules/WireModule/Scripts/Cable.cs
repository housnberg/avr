using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour {

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

    void Update() {
        if (particleSystemInstance != null) {
            Debug.Log(particleSystemInstance.transform.localEulerAngles);
        }
        if (endPoint == null && startPoint == null) {
            Destroy(gameObject);
        } else if (endPoint == null && !isCutted) {
            Cut(startPoint);
            if (particleSystemInstance == null && particleSystemPrefab != null) {
                particleSystemInstance = Instantiate(particleSystemPrefab);
                particleSystemInstance.transform.position = transform.position;
                particleSystemInstance.transform.parent = this.transform;
            }
        } else if (startPoint == null && !isCutted) {
            Cut(endPoint);
        }
    }

    private void Cut(Transform connector) {
        connector.Rotate(new Vector3(0, 0, 1), Random.Range(25f, 40f));
        isCutted = true;
    }

    public bool IsCutted() {
        return isCutted;
    }
}

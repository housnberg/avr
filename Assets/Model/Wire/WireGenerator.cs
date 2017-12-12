using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: This code is crap. Refactor it boy!
[RequireComponent(typeof(Renderer))]
public class WireGenerator : MonoBehaviour {

    //TODO: ONLY WORKAROUND !!!
    private const int SCALE = 1;

    private float scaleFactor;

    public Transform startPoint;
    public Transform endPoint;

    public int numberOfConnectors = 2;
    public float maxRange = 0.25f;
    public bool connect = true;

    public GameObject cablePrefab;
    public GameObject connectorPrefab;

    private ArrayList pointsBetween = new ArrayList();
    private Renderer material;

    // Use this for initialization
    void Start() {
        scaleFactor = 1 / transform.root.transform.localScale.y;
        material = gameObject.GetComponent(typeof(Renderer)) as Renderer;

        generatePoints();

        for (int i = 0; i < pointsBetween.Count - 1; i++) {
            Transform currentPoint = pointsBetween[i] as Transform;
            Transform nextPoint = pointsBetween[i + 1] as Transform;

            generateWire(currentPoint, nextPoint);
        }
    }

    private void generatePoints() {
        startPoint.GetComponent<Renderer>().material = material.material;
        pointsBetween.Add(startPoint);
        float step = 1f / (numberOfConnectors + 1);
        for (float perc = step; perc < 1f; perc += step) {
            Vector3 v = Vector3.Lerp(startPoint.position, endPoint.position, perc);
            v.x += Random.Range(-maxRange * (1 / scaleFactor), maxRange * (1 / scaleFactor));
            v.y += Random.Range(-maxRange * (1 / scaleFactor), maxRange * (1 / scaleFactor));
            v.z += Random.Range(-maxRange * (1 / scaleFactor), maxRange * (1 / scaleFactor));
            GameObject connectorInstance = Instantiate(connectorPrefab);
            connectorInstance.transform.position = v;
            connectorInstance.transform.parent = transform;
            connectorInstance.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
            connectorInstance.transform.GetComponent<Renderer>().material = material.material;

            pointsBetween.Add(connectorInstance.transform);
        }
        endPoint.GetComponent<Renderer>().material = material.material;
        pointsBetween.Add(endPoint);
    }

    private void generateWire(Transform start, Transform end) {
        float dist = Vector3.Distance(start.position, end.position);

        GameObject cableInstance = Instantiate(cablePrefab);
        WireController wireController = cableInstance.GetComponent<WireController>();
        wireController.setStartPoint(start);
        wireController.setEndPoint(end);

        cableInstance.transform.parent = transform;
        cableInstance.transform.GetComponent<Renderer>().material = material.material;

        cableInstance.transform.localScale = new Vector3(SCALE, (dist / 2) * scaleFactor, SCALE);
        cableInstance.transform.position = start.position;

        cableInstance.transform.LookAt(end, Vector3.down);
        cableInstance.transform.Rotate(Vector3.right, 90);

        Vector3 dir = end.position - cableInstance.transform.position;
        cableInstance.transform.position += dir * 0.5f;

        if (connect) {
            FixedJoint[] jointsStart = start.GetComponents<FixedJoint>();
            FixedJoint[] jointsEnd = end.GetComponents<FixedJoint>();

            jointsStart[1].connectedBody = cableInstance.transform.GetComponent<Rigidbody>();
            jointsEnd[0].connectedBody = cableInstance.transform.GetComponent<Rigidbody>();
        }
        else {
            cableInstance.transform.GetComponent<Rigidbody>().isKinematic = true;
            start.GetComponent<Rigidbody>().isKinematic = true;
            end.GetComponent<Rigidbody>().isKinematic = true;
        }

    }
}

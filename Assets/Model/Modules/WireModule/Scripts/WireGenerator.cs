using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WireGenerator : MonoBehaviour {

    //TODO: ONLY WORKAROUND !!!
    private const int SCALE = 1;

    private float scaleFactor;

    public string wireName;
    public bool shouldBeCutted = false;
    public bool cutable = true;

    public Transform startPoint;
    public Transform endPoint;

    public int numberOfConnectors = 2;
    public float maxRange = 0.25f;
    public bool connect = true;

    public GameObject cablePrefab;
    public GameObject connectorPrefab;

    private ArrayList connectors = new ArrayList();
    private Renderer material;

    private ArrayList cables = new ArrayList();
    private bool isCutted = false;

    // Use this for initialization
    void Start() {
        if (wireName == "") {
            wireName = this.name;
        }
        if (shouldBeCutted) {
            cutable = true;
        }
        if (!cutable) {
            shouldBeCutted = false;
        }

        scaleFactor = 1 / transform.root.transform.localScale.y;
        material = gameObject.GetComponent(typeof(Renderer)) as Renderer;

        generateConnectors();

        for (int i = 0; i < connectors.Count - 1; i++) {
            Transform currentPoint = connectors[i] as Transform;
            Transform nextPoint = connectors[i + 1] as Transform;

            generateCable(currentPoint, nextPoint);
        }
    }

    private void generateConnectors() {
        startPoint.GetComponent<Renderer>().material = material.material;
        connectors.Add(startPoint);
        float step = 1f / (numberOfConnectors + 1);
        for (float perc = step; perc < 1f; perc += step) {
            Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, perc);
            position.x += Random.Range(-maxRange * (1 / scaleFactor), maxRange * (1 / scaleFactor));
            position.y += Random.Range(-maxRange * (1 / scaleFactor), maxRange * (1 / scaleFactor));
            position.z += Random.Range(-maxRange * (1 / scaleFactor), maxRange * (1 / scaleFactor));
            GameObject connectorInstance = Instantiate(connectorPrefab);
            connectorInstance.transform.position = position;
            connectorInstance.transform.parent = transform.GetComponentsInChildren<Transform>()[2];
            connectorInstance.transform.localScale = new Vector3(SCALE, SCALE, SCALE);
            connectorInstance.transform.GetComponent<Renderer>().material = material.material;

            connectors.Add(connectorInstance.transform);
        }
        endPoint.GetComponent<Renderer>().material = material.material;
        connectors.Add(endPoint);
    }

    private void generateCable(Transform start, Transform end) {
        float dist = Vector3.Distance(start.localPosition, end.localPosition);

        GameObject cableInstance = Instantiate(cablePrefab);
        Cable cable = cableInstance.GetComponent<Cable>();
        cable.setStartPoint(start);
        cable.setEndPoint(end);

        cableInstance.transform.parent = transform.GetComponentsInChildren<Transform>()[2];
        cableInstance.transform.GetComponent<Renderer>().material = material.material;
        
        cableInstance.transform.localScale = new Vector3(SCALE, (dist / 2), SCALE);
        cableInstance.transform.localPosition = start.localPosition;

        cableInstance.transform.LookAt(end, Vector3.down);
        cableInstance.transform.Rotate(Vector3.right, 90);

        Vector3 dir = end.position - cableInstance.transform.position;
        cableInstance.transform.position += dir * 0.5f;
        cables.Add(cable);

        if (connect) {
            FixedJoint[] jointsStart = start.GetComponents<FixedJoint>();
            FixedJoint[] jointsEnd = end.GetComponents<FixedJoint>();

            jointsStart[1].connectedBody = cableInstance.transform.GetComponent<Rigidbody>();
            jointsEnd[0].connectedBody = cableInstance.transform.GetComponent<Rigidbody>();
        } else {
            cableInstance.transform.GetComponent<Rigidbody>().isKinematic = true;
            start.GetComponent<Rigidbody>().isKinematic = true;
            end.GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    void Update() {
        if (!isCutted) {
            foreach (Cable cable in cables) {
                if (cable.IsCutted() && shouldBeCutted) {
                    EventManager.TriggerEvent("Cutted");
                    isCutted = true;

                    break;
                }
                if (cable.IsCutted() && !shouldBeCutted) {
                    EventManager.TriggerEvent("CuttedFalse");
                    isCutted = true;

                    break;
                }
            }
        }
    }
}

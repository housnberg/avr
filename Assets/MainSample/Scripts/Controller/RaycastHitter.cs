using UnityEngine;

public class RaycastHitter : MonoBehaviour {

    public bool debugRaycast = true;

    private BaseHitableObject lastHittedObject;
    private float hitTime;
    
    void Start() {
        hitTime = 0;
    }
    
	void Update () {
		RaycastHit hit;
        BaseHitableObject currentHittedObject;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        if (debugRaycast) {
            Debug.DrawRay(transform.position, forward, Color.green);
        }
        
        if (Physics.Raycast(transform.position, forward, out hit)) {
            currentHittedObject = hit.collider.GetComponent<BaseHitableObject>();
            Debug.Log("currentHittedObject: " + currentHittedObject);
            if (currentHittedObject != null) {
                if (currentHittedObject.Equals(lastHittedObject)) {
                    hitTime += Time.deltaTime;
                    currentHittedObject.SetHitted(true);
                    currentHittedObject.SetHitTime(hitTime);
                }
                lastHittedObject = currentHittedObject;
            } else {
                hitTime = 0;
                if (lastHittedObject != null) {
                    lastHittedObject.SetHitted(false);
                    lastHittedObject.SetHitTime(hitTime);
                }
            }
        }
	}

}

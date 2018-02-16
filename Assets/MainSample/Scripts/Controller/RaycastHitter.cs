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
//            Debug.DrawRay(transform.position, forward, Color.green);
        }
		if (Physics.SphereCast (new Ray (transform.position, forward), 0.01f, out hit)) {
				currentHittedObject = hit.collider.GetComponent<BaseHitableObject>();
				if (currentHittedObject != null && currentHittedObject.GetCurrentlyHitable()) {
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

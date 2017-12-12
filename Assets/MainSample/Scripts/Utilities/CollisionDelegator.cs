using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use this to delegate a collision from a child object to a parent object.
/// The parent object should always implement the CollisionDelegate interface.
/// </summary>
public class CollisionDelegator : MonoBehaviour {

    public MonoBehaviour collisionDelegate;
    public CollisionDelegate cDelegate;

    void Start() {
        if (collisionDelegate != null) {
            cDelegate = collisionDelegate.GetComponent<CollisionDelegate>();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collisionDelegate != null) {
            cDelegate.onCollisionEnterChild(collision, this);
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collisionDelegate != null) {
            cDelegate.onCollisionExitChild(collision, this);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton.
/// Use this if you want to store per Application grasp data.
/// </summary>
public class GraspManager : MonoBehaviour {

    private GraspableObject lastGrabbedObject;

    private static GraspManager instance;

    public GraspableObject getLastGrabbedOject() {
        return lastGrabbedObject;
    }

    public static GraspManager Instance {
        get { return instance ?? (instance = new GameObject("GraspManager").AddComponent<GraspManager>()); }
    }

    public void setLastGrabbedOject(GraspableObject lastGrabbedObject) {
        this.lastGrabbedObject = lastGrabbedObject;
    }
}

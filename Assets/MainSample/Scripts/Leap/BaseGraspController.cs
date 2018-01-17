using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[RequireComponent(typeof(GraspableObject))]
public abstract class BaseGraspController : MonoBehaviour {

    private GraspableObject graspableObject;
    private GameObject handWrapper;
    private HandModel handModel;
    protected Hand hand;

    private bool hasBeenAdjusted = false;
    private Transform wrapper;
    private Vector3 initialPosition;

    void Start () {
        this.graspableObject = this.GetComponent<GraspableObject>();
        this.Init();
    }
	
	void Update () {
        handWrapper = GameObject.FindWithTag(TagConstants.HAND);
        if (handWrapper != null) {
            handModel = handWrapper.GetComponent<HandModel>();
            hand = handModel.GetLeapHand();

            if (graspableObject.IsGrabbed()) {
                DoGraspAction();

            } else {
                CancelGraspAction();
            }
        } 
	}

    public bool IsGrabbed() {
        return graspableObject.IsGrabbed();
    }
    
    /// <summary>
    /// The action which should be performed when the graspable object is grasped
    /// </summary>
    public abstract void DoGraspAction();

    /// <summary>
    /// The action which should be performed when the graspable object is released
    /// </summary>
    public abstract void CancelGraspAction();

    /// <summary>
    /// Initialize 
    /// </summary>
    public abstract void Init();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[RequireComponent(typeof(GraspableObject))]
public abstract class BaseGraspController : MonoBehaviour {
    
    private GraspableObject graspableObject;
    protected HandModel hand;
    protected Hand leapHand;

    private bool hasBeenAdjusted = false;
    private Transform wrapper;
    private Vector3 initialPosition;
    private BaseHitableObject baseHitable;

    private bool isResetted;

    void Start () {
        this.graspableObject = this.GetComponent<GraspableObject>();
        this.baseHitable = this.GetComponent<BaseHitableObject>();
        this.Init();
    }
	
	void Update () {
        GameObject[] handWrappers = GameObject.FindGameObjectsWithTag(TagConstants.HAND);
        if (handWrappers.Length != 0) {
            foreach (GameObject handWrapper in handWrappers) {
                hand = handWrapper.GetComponent<HandModel>();
                leapHand = hand.GetLeapHand();
                if (leapHand.IsRight) {
                    break;
                }
            }
            if (graspableObject.IsGrabbed()) {
                if (baseHitable != null) {
                    baseHitable.SetCurrentlyHitable(false);
                    isResetted = false;
                }
                DoGraspAction();
            } else {
                if (baseHitable != null) {
                    ResetState();
                }
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

    private void ResetState() {
        if (!isResetted) {
            baseHitable.SetCurrentlyHitable(true);
            isResetted = true;
        }
    }
}
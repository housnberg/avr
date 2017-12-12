using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

[RequireComponent(typeof(GraspableObject))]
public abstract class BaseGraspController : MonoBehaviour {

    protected GraspableObject graspableObject;
    protected GameObject handWrapper;
    protected HandModel handModel;
    protected Hand hand;
    
    void Start () {
        this.graspableObject = this.GetComponent<GraspableObject>();
        this.init();
    }
	
	void Update () {
        //TODO: Schoeneres Konstrukt überlegen
        handWrapper = GameObject.FindWithTag(TagConstants.HAND);
        if (handWrapper != null) {
            handModel = handWrapper.GetComponent<HandModel>();
            hand = handModel.GetLeapHand();

            if (graspableObject.IsGrabbed()) {
                doGraspAction();
            } else {
                cancelGraspAction();
            }
        } 
	}
    
    /// <summary>
    /// The action which should be performed when the graspable object is grasped
    /// </summary>
    public abstract void doGraspAction();

    /// <summary>
    /// The action which should be performed when the graspable object is released
    /// </summary>
    public abstract void cancelGraspAction();

    /// <summary>
    /// Initialize 
    /// </summary>
    public abstract void init();
}

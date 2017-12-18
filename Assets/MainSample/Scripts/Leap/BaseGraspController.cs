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

    public bool adjustGrasping = false;
    public Vector3 adjustment;

    private bool hasBeenAdjusted = false;
    private Transform wrapper;
    private Vector3 initialPosition;

    void Start () {
        this.graspableObject = this.GetComponent<GraspableObject>();
        this.init();
    }
	
	void Update () {
        handWrapper = GameObject.FindWithTag(TagConstants.HAND);
        if (handWrapper != null) {
            handModel = handWrapper.GetComponent<HandModel>();
            hand = handModel.GetLeapHand();

            if (graspableObject.IsGrabbed()) {
                doGraspAction();
                /*
                if (adjustGrasping && !hasBeenAdjusted) {
                    wrapper = this.transform.GetChild(0);
                    initialPosition = wrapper.localPosition;
                    wrapper.localPosition = adjustment;

                    hasBeenAdjusted = true;
                }
                */
            } else {
                cancelGraspAction();
                /*
                if (adjustGrasping && hasBeenAdjusted) {
                    wrapper.localPosition = initialPosition;

                    hasBeenAdjusted = false;
                }
                */
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
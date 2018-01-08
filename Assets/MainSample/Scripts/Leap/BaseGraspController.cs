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
    
    public Vector3 adjustment;

    private bool hasBeenAdjusted = false;
    private Transform wrapper;
    private Vector3 initialPosition;

    public Rigidbody anchor;
    public float breakForce = Mathf.Infinity;

    private Joint anchorJoint;

    void Start () {
        this.graspableObject = this.GetComponent<GraspableObject>();
        this.init();
        if (anchor != null) {
            anchorJoint = anchor.GetComponent<Joint>();
            anchorJoint.connectedBody = this.GetComponent<Rigidbody>();
        }
    }
	
	void Update () {
        handWrapper = GameObject.FindWithTag(TagConstants.HAND);
        if (handWrapper != null) {
            handModel = handWrapper.GetComponent<HandModel>();
            hand = handModel.GetLeapHand();

            if (graspableObject.IsGrabbed()) {
                if (anchor != null) {
                    anchorJoint.breakForce = breakForce;
                }
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
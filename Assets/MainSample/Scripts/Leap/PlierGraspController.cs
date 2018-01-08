using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlierGraspController : BaseGraspController {

    private HingeJoint handleRightHj;
    private JointSpring spring;

    private bool rotated = false;

    private float maxTargetPosition;

    public override void doGraspAction() {
        spring.targetPosition = maxTargetPosition - (maxTargetPosition * hand.GrabStrength);
        handleRightHj.spring = spring;
        
        if (!rotated) {
            Vector3 palmDirection = hand.PalmNormal.ToUnityScaled(true);
            //transform.rotation = Quaternion.LookRotation(palmDirection);
            rotated = true;
        }
    }
    
    public override void cancelGraspAction() {
        spring.targetPosition = maxTargetPosition;
        handleRightHj.spring = spring;
    }
    
    public override void init() {
        handleRightHj = this.GetComponentInChildren<HingeJoint>();
        spring = handleRightHj.spring;
        maxTargetPosition = spring.targetPosition;
    }
}
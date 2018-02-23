using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlierGraspController : BaseGraspController {

    private HingeJoint handleRightHj;
    private JointSpring spring;

    private bool rotated = false;

    private float maxTargetPosition;

    public override void DoGraspAction() {
        float newTarget = maxTargetPosition - (maxTargetPosition * leapHand.GrabStrength);
        spring.targetPosition = newTarget < 0 ? 0 : newTarget; 
        handleRightHj.spring = spring;
        
        if (!rotated) {
            Vector3 palmDirection = leapHand.PalmNormal.ToUnityScaled(true);
            rotated = true;
        }
    }
    
    public override void CancelGraspAction() {
        spring.targetPosition = maxTargetPosition;
        handleRightHj.spring = spring;
    }
    
    public override void Init() {
        handleRightHj = this.GetComponentInChildren<HingeJoint>();
        spring = handleRightHj.spring;
        maxTargetPosition = spring.targetPosition;
    }
}
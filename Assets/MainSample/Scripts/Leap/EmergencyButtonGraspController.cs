using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyButtonGraspController : BaseGraspController {

    public float maxOpeningDegree = 90;


    private HingeJoint hinge;
    private JointSpring spring;

    public override void cancelGraspAction() {
        //Evtl zuruecksetzen
    }


    public override void doGraspAction() {
        spring.targetPosition = 15; //Irgend eine Berechnung, wenn das Ding gegriffen wird
        //Nicht über 90 grad

        hinge.spring = spring;
    }

    public override void init() {
        hinge = this.GetComponentInChildren<HingeJoint>();
        spring = hinge.spring;
    }
}

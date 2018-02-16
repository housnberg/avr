using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionHitableObject : BaseHitableObject {

    public Transform anchor;

	new void Update () {
//        base.Update();
//
		if (shouldDoHitAction()) {
			EventManager.TriggerEvent ("TutorialInstructions");
			
//            transform.localPosition = anchor.position;
//            transform.rotation = anchor.rotation;
//            transform.parent = anchor;
        }
    }
}

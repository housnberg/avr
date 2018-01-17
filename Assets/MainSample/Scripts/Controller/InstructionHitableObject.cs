using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionHitableObject : BaseHitableObject {

    public Transform anchor;

	new void Update () {
        base.Update();

		if (shouldDoHitAction()) {
            transform.localPosition = anchor.position;
            transform.rotation = anchor.rotation;
            transform.parent = anchor;
        }
    }
}

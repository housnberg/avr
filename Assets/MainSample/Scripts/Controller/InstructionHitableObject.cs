using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionHitableObject : BaseHitableObject {

    public Transform anchor;

    private Quaternion initialRotation;
    private Vector3 initialPosition;

    new void Start() {
        base.Start();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (anchor == null) {
            interval = 0.1f;
            DisableProgressBar(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == TagConstants.FINGER_TIP) {
            if (anchor != null) {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
            }
        }
    }

    public override void DoHitAction() {
        if (ShouldDoHitAction()) {
            EventManager.TriggerEvent(ComplexBombEvent.TUTORIAL_INSTRUCTIONS);

            if (anchor != null) {
                transform.position = anchor.position;
                transform.rotation = anchor.rotation;
            }
        }
    }
}

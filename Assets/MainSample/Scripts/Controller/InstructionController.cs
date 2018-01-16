using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionController : BaseController {

    void OnTriggerEnter(Collider other) {
       if (other.tag == "FingerTip") {
            OnResetTools();
       }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionText : MonoBehaviour {

    public string moduleName;
    public int priority = 0;

    [TextArea]
    public string instructionText;

}

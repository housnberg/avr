using System;
using UnityEngine;
using UnityEngine.UI;

public class InstructionController : BaseController {

    public InstructionText[] instructionTexts;
    public bool useTextsInScene = true;

    private Text text;

    new void Start() {
        base.Start();

        if (useTextsInScene) {
            instructionTexts = FindObjectsOfType<InstructionText>();
        }

        text = transform.Find("Quad/Canvas/Instruction").GetComponent<Text>();

        Array.Sort(instructionTexts, delegate (InstructionText text1, InstructionText text2) {
            return text1.priority.CompareTo(text2.priority);
        });

        foreach (InstructionText instructionText in instructionTexts) {
            text.text = text.text + instructionText.instructionText + "\n\n";
        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "FingerTip") {
            OnResetTools();
        }
    }


}


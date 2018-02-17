using UnityEngine;

public class Module : MonoBehaviour {

    [TextArea]
    public string instruction;
    public int priority;

    private bool isPassed;

    public void ModulePassed(bool passed) {
        isPassed = passed;
        string moduleEvent = "ModulePassed";
        if (!passed) {
            moduleEvent = "ModuleFailed";
        }
        EventManager.TriggerEvent(moduleEvent);
    }

    public bool GetPassed() {
        return isPassed;
    }

}

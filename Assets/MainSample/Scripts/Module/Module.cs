using UnityEngine;

public abstract class Module : MonoBehaviour {

    [TextArea]
    public string instruction;
    public int priority;

    private bool isPlayed;

    protected void Passed(bool passed) {
        if (!isPlayed) {
            isPlayed = true;
            ComplexBombEvent moduleEvent = ComplexBombEvent.MODULE_PASSED;
            if (!passed) {
                moduleEvent = ComplexBombEvent.MODULE_FAILED;
            }
            EventManager.TriggerEvent(moduleEvent);
        }
    }

    public bool GetPlayed() {
        return isPlayed;
    }

}

using UnityEngine;

public class Module : MonoBehaviour {

    [TextArea]
    public string instruction;
    public int priority;

    private bool isPlayed;

    public void ModulePassed(bool passed) {
        if (!isPlayed) {
            isPlayed = true;
            string moduleEvent = "ModulePassed";
            if (!passed) {
                moduleEvent = "ModuleFailed";
            }
            EventManager.TriggerEvent(moduleEvent);
        }
    }

    public bool GetPlayed() {
        return isPlayed;
    }

}

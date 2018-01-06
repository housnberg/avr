using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {

    public void ModulePassed(bool passed) {
        string moduleEvent = "ModulePassed";
        if (!passed) {
            moduleEvent = "ModuleFailed";
        }
        EventManager.TriggerEvent(moduleEvent);
    }

}

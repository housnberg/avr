using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WireModule : Module {

    private int amountCutableWires = 0;
    private int amountCuttedWires = 0;

    private UnityAction someListener;

    void Awake() {
        WireGenerator[] wires = GetComponentsInChildren<WireGenerator>();
        foreach(WireGenerator wire in wires) {
            if (wire.shouldBeCutted) {
                amountCutableWires++;
            }
        }

        EventManager.StartListening("Cutted", Cutted);
        EventManager.StartListening("CuttedFalse", CuttedFalse);
    }

    void Cutted() {
        amountCuttedWires++;
        if (amountCuttedWires == amountCutableWires) {
            ModulePassed(true);
        }
    }

    void CuttedFalse() {
        ModulePassed(false);
    }
}

public class WireModule : Module {

    private int amountCutableWires = 0;
    private int amountCuttedWires = 0;

    void Awake() {
        WireGenerator[] wires = GetComponentsInChildren<WireGenerator>();
        foreach(WireGenerator wire in wires) {
            if (wire.shouldBeCutted) {
                amountCutableWires++;
            }
        }

        EventManager.StartListening(ComplexBombEvent.RIGHT_CABLE_CUTTED, OnRightCableCutted);
        EventManager.StartListening(ComplexBombEvent.WRONG_CABLE_CUTTED, OnWrongCableCutted);
    }

    private void OnRightCableCutted() {
        amountCuttedWires++;
        if (amountCuttedWires == amountCutableWires) {
            Passed(true);
        }
    }

    private void OnWrongCableCutted() {
        Passed(false);
    }
}

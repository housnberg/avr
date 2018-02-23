using Leap;
using UnityEngine;

[RequireComponent(typeof(SkeletalHand))]
public class Menu : MonoBehaviour {

    private SkeletalHand hand;
    private MenuButton[] buttons;

	void Start () {
        hand = GetComponent<SkeletalHand>();
        buttons = FindObjectsOfType<MenuButton>();
	}
	
	void Update () {
        Vector3 forward = hand.GetPalmNormal();
        Hand leapHand = hand.GetLeapHand();

        Vector3 centroid = leapHand.Fingers[1].TipPosition.ToUnityScaled() * .25f + leapHand.Fingers[2].TipPosition.ToUnityScaled() * .25f + leapHand.Fingers[3].TipPosition.ToUnityScaled() * .25f + leapHand.Fingers[4].TipPosition.ToUnityScaled() * .25f;
        float distance = Vector3.Distance(leapHand.PalmPosition.ToUnityScaled(), centroid);

        if (forward.z < 0 && forward.y > 0 && distance > 0.075f) {
            foreach (MenuButton button in buttons) {
                button.gameObject.SetActive(true);
            }
			EventManager.TriggerEvent (ComplexBombEvent.TUTORIAL_MENU);
        } else {
            foreach (MenuButton button in buttons) {
                button.gameObject.SetActive(false);
            }
        }
        //Debug.Log("Palm Direction: " + forward + " " + showButtons);
        //Debug.DrawRay(transform.position, forward, Color.green);

    }
}

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TutorialManager : MonoBehaviour {

	public static TutorialManager instance = null;

	private int tutorialCounter = 0;

	public bool tutorialCompleted = false;

	private GameObject tutorialScreen;
	public GameObject[] hints;

	void Start() {
	}

	void Awake() {    
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);

		tutorialScreen = GameObject.Find("TutorialScreen");
		tutorialScreen.SetActive (true);

		EventManager.StartListening(ComplexBombEvent.TUTORIAL_TOOLS, OnTutorialTools);
        EventManager.StartListening(ComplexBombEvent.TUTORIAL_COMPLETED, OnTutorialCompleted);
    }

	private void OnTutorialTools() {
		Debug.Log ("Tutorial tools completed");
		hints[0].SetActive(false);
		hints[1].SetActive (true);
		
		EventManager.StopListening (ComplexBombEvent.TUTORIAL_TOOLS, OnTutorialTools);
		EventManager.StartListening (ComplexBombEvent.TUTORIAL_MENU, OnTutorialMenu);
	}
	
	private void OnTutorialMenu() {
		Debug.Log ("Tutorial menu completed");
		hints[1].SetActive (false);
		hints[2].SetActive (true);
		
		EventManager.StopListening (ComplexBombEvent.TUTORIAL_MENU, OnTutorialMenu);
		EventManager.StartListening (ComplexBombEvent.TUTORIAL_RESET, OnTutorialReset);
	}
	
	private void OnTutorialReset() {
		Debug.Log ("Tutorial reset completed");
		hints[2].SetActive (false);
		hints[3].SetActive (true);
		
		EventManager.StopListening (ComplexBombEvent.TUTORIAL_RESET, OnTutorialReset);
		EventManager.StartListening (ComplexBombEvent.TUTORIAL_INSTRUCTIONS, OnTutorialInstructions);
	}

	private void OnTutorialInstructions() {
		Debug.Log ("Tutorial instructions completed");
		hints[3].SetActive(false);

		tutorialCompleted = true;
		EventManager.StopListening(ComplexBombEvent.TUTORIAL_INSTRUCTIONS, OnTutorialInstructions);
        EventManager.TriggerEvent(ComplexBombEvent.TUTORIAL_COMPLETED);
	}

    private void OnTutorialCompleted() {
        tutorialCompleted = true;
        foreach (GameObject hint in hints) {
            hint.SetActive(false);
        }
        EventManager.StopListening(ComplexBombEvent.TUTORIAL_MENU, OnTutorialMenu);
        EventManager.StopListening(ComplexBombEvent.TUTORIAL_RESET, OnTutorialReset);
        EventManager.StopListening(ComplexBombEvent.TUTORIAL_INSTRUCTIONS, OnTutorialInstructions);
        EventManager.StopListening(ComplexBombEvent.TUTORIAL_TOOLS, OnTutorialTools);
    }
}

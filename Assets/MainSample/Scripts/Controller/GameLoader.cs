using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {

    public GameObject eventManager;
    public GameObject tutorialManager;
    public GameObject gameManager;              

    void Awake() {
        if (EventManager.instance == null) {
            Instantiate(eventManager);
        }
        if (TutorialManager.instance == null) {
            Instantiate(tutorialManager);
        }
        if (GameManager.instance == null) {
            Instantiate(gameManager);
        }  
    }
}

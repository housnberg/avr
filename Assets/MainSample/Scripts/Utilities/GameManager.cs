using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Singleton.
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private int amountModules;
    private int amountSucceededModules;

    private CountdownModule countdownModule;
    private Bomb bomb;

    public AudioSource gameOverSound;
    public AudioSource winSound;

    private bool gameLost = false;

    void Awake() {    
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        amountModules = GameObject.FindObjectsOfType<Module>().Length;
        countdownModule = GameObject.FindObjectOfType<CountdownModule>();
        bomb = GameObject.FindObjectOfType<Bomb>();

        EventManager.StartListening("ModulePassed", ModulePassed);
        EventManager.StartListening("ModuleFailed", ModuleFailed);
    }

    void ModulePassed() {
        Debug.Log("YEAH MODULE PASSED");
        amountSucceededModules++;
        if (amountSucceededModules == amountModules) {
            if (winSound != null) {
                winSound.Play();
            }
            if (countdownModule != null) {
                countdownModule.Stop();
            }
            Debug.Log("YOU DEFUSED THE BOMB!!!");
        }
    }

    void ModuleFailed() {
        gameLost = true;
        if (countdownModule != null) {
            countdownModule.FastCountdown();
        }
        if (gameOverSound != null) {
            gameOverSound.Play();
        }
        bomb.Explode();
        Debug.Log("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM");
    }
}
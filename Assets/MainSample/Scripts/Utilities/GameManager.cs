using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton.
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private int amountModules;
    private int amountSucceededModules;

    private CountdownModule timer;
    private Bomb bomb;

    private GameObject gameOverScreen;
    private GameObject gameWonScreen;

    public AudioSource gameOverSound;
    public AudioSource winSound;

    private bool gameLost = false;
    private bool gameWon = false;

    void Awake() {    
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        gameOverScreen = GameObject.Find("GameOverScreen");
        gameWonScreen = GameObject.Find("GameWonScreen");

        gameOverScreen.SetActive (false);
        gameWonScreen.SetActive (false);

        amountModules = GameObject.FindObjectsOfType<Module>().Length;
        timer = GameObject.FindObjectOfType<CountdownModule>();
        bomb = GameObject.FindObjectOfType<Bomb>();

        EventManager.StartListening("ModulePassed", OnModulePassed);
        EventManager.StartListening("ModuleFailed", OnModuleFailed);
        EventManager.StartListening("ReloadGame", OnReloadGame);
    }

    void OnModulePassed() {
        Debug.Log("YEAH MODULE PASSED");
        if (!gameLost && !gameWon) {
            amountSucceededModules++;
            if (amountSucceededModules == amountModules) {
                gameWon = true;
                if (winSound != null) {
                    winSound.Play();
                    StartCoroutine(ShowScreenAfterSeconds(winSound.clip.length, gameWonScreen));
                }
                if (timer != null) {
                    timer.Stop();
                }
                Debug.Log("YOU DEFUSED THE BOMB!!!");
            }
        }
    }

    void OnModuleFailed() {
        if (!gameLost && !gameWon) {
            gameLost = true;
            if (timer != null) {
                timer.FastCountdown();
            }
            StartCoroutine(ExplodeAfterDelay(timer.beepLong.clip.length));
        }
    }

    private IEnumerator ExplodeAfterDelay(float seconds) {
        yield return new WaitForSeconds(seconds);

        if (gameOverSound != null) {
            gameOverSound.Play();
            StartCoroutine(ShowScreenAfterSeconds(gameOverSound.clip.length, gameOverScreen));
        }
        if (bomb != null) {
            bomb.Explode();
        }
        Debug.Log("BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM");
    }

    private IEnumerator ShowScreenAfterSeconds(float seconds, GameObject screen) {
        yield return new WaitForSeconds(seconds);

        Debug.Log("Set screen active");
        screen.SetActive(true);
    }

    private void OnReloadGame() {
        Debug.Log("RESTART SCENE");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
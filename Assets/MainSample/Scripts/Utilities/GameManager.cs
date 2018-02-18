using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

/// <summary>
/// Singleton.
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    private Module[] modules;

    private int amountSucceededModules;

    private CountdownModule timer;
    private Bomb bomb;

    private GameObject gameOverScreen;
    private GameObject gameWonScreen;
    private Text instructionText;

    public AudioSource gameOverSound;
    public AudioSource winSound;
    public AudioSource modulePassedSound;
    public AudioSource backgroundMusic;
    public Transform backgroundMusicSourceAnchor;
    public bool playBackgroundMusic = true;

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

        modules = GameObject.FindObjectsOfType<Module>();
        timer = GameObject.FindObjectOfType<CountdownModule>();
        bomb = GameObject.FindObjectOfType<Bomb>();

        instructionText = transform.Find("Instruction/Quad/Canvas/InstructionText").GetComponent<Text>();

        EventManager.StartListening("ModulePassed", OnModulePassed);
        EventManager.StartListening("ModuleFailed", OnModuleFailed);
        EventManager.StartListening("ReloadGame", OnReloadGame);

        EventManager.StartListening("TutorialCompleted", OnTutorialCompleted);
        EventManager.StartListening("CaseOpened", OnCaseOpened);
        EventManager.StartListening("MetalplateRemoved", OnMetalplateRemoved);

        Array.Sort(modules, delegate (Module current, Module other) {
            return current.priority.CompareTo(other.priority);
        });

        PlayBackgroundMusic();
    }

    void OnModulePassed() {
        if (!gameLost && !gameWon) {
            amountSucceededModules++;
            nextSolvableModuleIntruction();
            if (amountSucceededModules == modules.Length) {
                gameWon = true;
                EventManager.TriggerEvent("TutorialCompleted");
                if (winSound != null) {
                    PrintText(gameWonScreen);
                    winSound.Play();
                    StartCoroutine(ShowScreenAfterSeconds(winSound.clip.length, gameWonScreen));
                }
                if (timer != null) {
                    timer.Stop();
                }
            } else {
                if (modulePassedSound != null) {
                    modulePassedSound.Play();
                }
            }
        }
    }

    void OnModuleFailed() {
        EventManager.TriggerEvent("TutorialCompleted");
        if (!gameLost && !gameWon) {
            gameLost = true;
            PrintText(gameOverScreen);
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
    }

    private IEnumerator ShowScreenAfterSeconds(float seconds, GameObject screen) {
        yield return new WaitForSeconds(seconds);
        
        screen.SetActive(true);
    }

    private void OnReloadGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTutorialCompleted() {
        setInstructionText("OPEN THE SUITCASE BY TOUCHING THE FRONT BUTTONS SIMULTANEOUSLY\n\nBEWARE! THE DETONATION COUNTDOWN WILL START AS SOON AS YOU OPEN THE CASE!");
    }

    private void OnCaseOpened() {
        setInstructionText("TAKE THE SCREWDRIVER AND REMOVE THE METAL PLATE TO GET TO THE CORE MODULE");
    }

    private void OnMetalplateRemoved() {
        nextSolvableModuleIntruction();
    }

    private void nextSolvableModuleIntruction() {
        string instruction = "No Modules left!";
        foreach (Module module in modules) {
            if (!module.GetPlayed()) {
                instruction = module.instruction;
                break;
            }
        }
        setInstructionText(instruction);
    }

    private void setInstructionText(string instructionText) {
        if (instructionText != null) {
            this.instructionText.text = instructionText;
        }
    }

    private void PlayBackgroundMusic() {
        if (backgroundMusic != null) {
            if (playBackgroundMusic) {
                float playtime = UnityEngine.Random.Range(0, backgroundMusic.clip.length / 3);
                backgroundMusic.time = playtime;

                if (backgroundMusicSourceAnchor != null) {
                    AudioSource.PlayClipAtPoint(backgroundMusic.clip, backgroundMusicSourceAnchor.position, backgroundMusic.volume);
                } else {
                    backgroundMusic.Play();
                }
            } else {
                backgroundMusic.Stop();
                backgroundMusic.playOnAwake = false;
            }
        }
    }

    private void PrintText(GameObject screen) {
        Text remainingTime = screen.transform.Find("RemainingTime").GetComponent<Text>();
        Text defusedModules = screen.transform.Find("DefusedModules").GetComponent<Text>();

        remainingTime.text = "remaining time: " + timer.GetFormattedTime() + " / " + timer.GetFormattedInitialTime();
        defusedModules.text = "number of defused modules: " + amountSucceededModules + " / " + modules.Length;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownModule : MonoBehaviour {

    private Text counter;
    private bool started = false;
    public bool playOnAwake = true;

    private bool stop = false;
    private bool fastCountdown = false;

    public AudioSource beepShort;
    public AudioSource beepLong;

    public float minutes = 5;
    public float seconds = 0;
    public float miliseconds = 0;

    void Awake() {
        EventManager.StartListening("StartTimer", StartTimer);

        if (playOnAwake) {
            started = true;
        }
    }

    void Start() {
        counter = GetComponentInChildren<Text>();
    }

    void Update() {
        if (started) {
            if (!stop) {
                if (miliseconds <= 0) {
                    if (seconds <= 0) {
                        minutes--;
                        seconds = 59;
                        if (beepShort != null) {
                            beepShort.Play();
                        }
                    }
                    else if (seconds >= 0) {
                        seconds--;
                        if (beepShort != null) {
                            beepShort.Play();
                        }
                    }
                    miliseconds = 100;
                }

                miliseconds -= Time.deltaTime * 100;

                if ((minutes <= 0 && seconds <= 0 && miliseconds <= 0) || fastCountdown) {
                    stop = true;
                    minutes = 0;
                    seconds = 0;
                    miliseconds = 0;
                    beepLong.Play();
                    EventManager.TriggerEvent("ModuleFailed");
                }

                counter.text = string.Format("{0:00}-{1:00}-{2:00}", minutes, seconds, (int) miliseconds < 0 ? 0 : (int) miliseconds);
            }
        }
    }

    private void StartTimer() {
        started = true;
    }

    public void FastCountdown() {
        fastCountdown = true;
    }

    public void Stop() {
        stop = true;
    }
}

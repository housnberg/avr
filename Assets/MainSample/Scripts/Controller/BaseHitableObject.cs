using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BaseHitableObject : MonoBehaviour {

    public ProgressBar progressBar;
    public float interval = 2f;
    public float moveSpeed = 2;

    private bool isHitted = false;
    private float hitTime = 0;
    protected bool isCurrentlyHitable = true;
    protected bool isDoingHitAction;

    private ProgressBar progressBarInstance;
    private bool progressbarDisabled = false;

    protected void Start() {
        if (progressBar != null) {
            progressBarInstance = Instantiate(progressBar);
            progressBarInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            progressBarInstance.gameObject.SetActive(false);
            progressBarInstance.transform.parent = transform;
        }
        EventManager.StartListening(ComplexBombEvent.RESET_TOOLS, OnResetTools);
    }

    protected void Update() {
        if ((isHitted && isCurrentlyHitable) || isDoingHitAction) {
            if (progressBarInstance != null && !progressbarDisabled && !isDoingHitAction) {
                progressBarInstance.gameObject.SetActive(true);
                progressBarInstance.UpdateProgressBar(Normalize(hitTime));
            }
            if (ShouldDoHitAction() || isDoingHitAction) {
                isDoingHitAction = true;
                DoHitAction();
            }
        } else {
            if (progressBarInstance != null && !progressbarDisabled) {
                progressBarInstance.gameObject.SetActive(false);
            }
        }
        if (isDoingHitAction) {
            if (progressBarInstance != null && !progressbarDisabled) {
                progressBarInstance.gameObject.SetActive(false);
            }
        }
    }

    public void SetHitted(bool hitted) {
        isHitted = hitted;
    }

    public void SetHitTime(float hitTime) {
        this.hitTime = hitTime;
    }

    public bool GetCurrentlyHitable() {
        return isCurrentlyHitable;
    }

    public void SetCurrentlyHitable(bool currentlyHitable) {
        this.isCurrentlyHitable = currentlyHitable;
    }

    protected bool ShouldDoHitAction() {
        return isHitted && (hitTime >= interval);
    }

    public void DisableProgressBar(bool disabled) {
        progressbarDisabled = disabled;

    }

    private float Normalize(float value) {
        return value / interval;
    }

    private void OnResetTools() {
        isDoingHitAction = false;
    }

    public abstract void DoHitAction();
}

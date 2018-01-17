using UnityEngine;

public abstract class BaseHitableObject : MonoBehaviour {

    public ProgressBar progressBar;
    public float interval = 2f;
    public float speed = 2;

    protected bool isHitted = false;
    protected float hitTime = 0;

    private ProgressBar progressBarInstance;

    protected void Start() {
        if (progressBar != null) {
            progressBarInstance = Instantiate(progressBar);
            progressBarInstance.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            progressBarInstance.gameObject.SetActive(false);
            progressBarInstance.transform.parent = transform;
        }
    }

    protected void Update() {
        if (progressBarInstance != null) {
            if (isHitted) {
                progressBarInstance.gameObject.SetActive(true);
                progressBarInstance.UpdateProgressBar(Normalize(hitTime));
            }
            else {
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

    protected bool shouldDoHitAction() {
        return isHitted && (hitTime >= interval);
    }

    private float Normalize(float value) {
        return value / interval;
    }
}

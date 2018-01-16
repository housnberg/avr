using UnityEngine;

public abstract class BaseHitableObject : MonoBehaviour {

    protected bool isHitted = false;
    protected float hitTime = 0;

    public float interval = 2f;
    public float speed = 2;

    public void SetHitted(bool hitted) {
        isHitted = hitted;
    }

    public void SetHitTime(float hitTime) {
        this.hitTime = hitTime;
    }

    protected bool shouldDoHitAction() {
        return isHitted && (hitTime >= interval);
    }
}

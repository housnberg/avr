using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public bool lookToCamera;

    private Slider progressBar;

	// Use this for initialization
	void Awake () {
        progressBar = transform.GetComponentInChildren<Slider>();
        ResetProgressBar();
	}

    void Update() {
        if (lookToCamera) {
            Vector3 v = Camera.main.transform.position - transform.position;
            v.x = v.z = 0.0f;
            transform.LookAt(Camera.main.transform.position - v);
            transform.rotation = (Camera.main.transform.rotation); // Take care about camera rotation
        }   
    }

    public void UpdateProgressBar(float value) {
        progressBar.value = value;
    }

    public void ResetProgressBar() {
        progressBar.value = 0;
    }
}

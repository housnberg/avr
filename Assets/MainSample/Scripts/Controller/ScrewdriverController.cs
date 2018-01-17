using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewdriverController : BaseController {

    /*
    public float speed = 2f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    private bool isMoving = false;
    private BaseGraspController graspController;

	void Start () {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        graspController = GetComponent<BaseGraspController>();
        //EventManager.StartListening("ResetTool", OnResetTool);
    }

    private void OnResetTool() {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Desk" && !graspController.IsGrbbed()) {
            isMoving = true;
            //GameObject copy = Instantiate(gameObject);
            //copy.transform.position = initialPosition;
            //copy.transform.rotation = initialRotation;
            //Destroy(gameObject);
        }
    }

    void Update() {
        if (isMoving) {
            Move(initialPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.time * 2);
        }
    }

    private void Move(Vector3 targetPosition) {
        Debug.Log("Started moving towards: " + targetPosition);
        float distance = Vector3.Distance(transform.position, targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * distance * speed);
        rb.useGravity = false;
        rb.isKinematic = true;

        if (transform.position == initialPosition) {
            isMoving = false;
            rb.isKinematic = false;
        }
    }
    */

}


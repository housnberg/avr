using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseController : MonoBehaviour {

	private HingeJoint hinge;
	private bool isOpen;

    public float openingAngle = 100;
    public float speed = 1f;
    public float delay;
    public float zPosition = 0;
    public float rotationAngle = 25;

    public AudioSource caseOpenSound;

    private CaseButtonController leftButton;
	private CaseButtonController rightButton;
	
	private GameObject[] fingerTips;
    private Vector3 currentCaseControllerPosition;
    private Vector3 targetPosition;
    
    private bool isMoving = false;
    private bool isRotating = false;
    
	void Start () {
		hinge = GameObject.FindGameObjectWithTag(TagConstants.CASE_LID).GetComponent<HingeJoint>();
		fingerTips = GameObject.FindGameObjectsWithTag (TagConstants.FINGER_TIP);
		leftButton = GameObject.FindGameObjectWithTag(TagConstants.LEFT_CASE_BUTTON).GetComponent<CaseButtonController>();
        rightButton = GameObject.FindGameObjectWithTag(TagConstants.RIGHT_CASE_BUTTON).GetComponent<CaseButtonController>();

		EventManager.StartListening (ComplexBombEvent.METALPLATE_REMOVED, RotateCase);
	}
	
	void Update () {
		if (!isOpen && CheckCollider()) {
            JointSpring spring = hinge.spring;
            spring.targetPosition = openingAngle;
            hinge.spring = spring;

            EventManager.TriggerEvent(ComplexBombEvent.START_COUNTDOWN);
            isOpen = true;
            EventManager.TriggerEvent(ComplexBombEvent.CASE_OPENED);

            if (caseOpenSound != null) {
                AudioSource.PlayClipAtPoint(caseOpenSound.clip, transform.position, caseOpenSound.volume);
            }

            StartCoroutine(Move(delay));
		}

        float step = speed * Time.deltaTime;
        if (isMoving) {
			targetPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }

        if (isRotating) {
			float test = Mathf.Sin(rotationAngle * Mathf.Deg2Rad) - 0.215f;
			targetPosition = new Vector3(transform.position.x, test, zPosition);
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            transform.Rotate(new Vector3(2, 0, 0));
            if (Mathf.Round(transform.eulerAngles.x) >= rotationAngle) {
                isRotating = false;
            }
        }
	}

	bool CheckCollider() {
		return (leftButton.IsPressed() && rightButton.IsPressed());
	}

    private IEnumerator Move(float seconds) {
        yield return new WaitForSeconds(seconds);

        isMoving = true;
    }

	private void RotateCase() {
		isRotating = true;
	}
}

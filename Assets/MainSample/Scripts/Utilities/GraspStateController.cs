using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// This script handles the grasp and carry interaction; it runs the checks for the initial hand position,
/// displays the bottle and monitors the interaction
/// </summary>
public class GraspStateController : MonoBehaviour {
    // set to true to obtain some diagnostic command line output
    private static bool Verbose = true;
    // the controller is some kind of state-machine that uses these
    // values to control the interaction accordingly
    private enum InteractionStage {
        STARTUP,
        TARGET_PRESENTATION,
        GRASPING_INTERACTION,
        WAITING
    }

    private class InteractionStateObject {
        // this class can be used as data container to store time-stamps and
        // stuff like this, here, only the state is relevant
        public GraspStateController.InteractionStage InteractionStage;
    }
    // how long the feedback will be shown
    public float FeedbackIntervalInSeconds = 1.0f;
    // text element to present feedback
    public UnityEngine.UI.Text FeedbackDisplay;
    // the to-be-grasped object
    public GraspableObject[] Targets;
    // jsut some generic response text (this interaction was used in one experiment)
    private string[] positiveFeedbackTemplates = new string[] {
        "Sehr gut",
        "Super",
        "Gut gemacht"
    };

    //actual hand controller that handles the Leap data;
    public SimplifiedHandController HandControllerReference;
    // current state in the sequence
    private InteractionStateObject CurrentInteractionState;
    // you can change this to 'upsidedown'
    private string bottleOrientationMode = "upright";
    // the range monitor, used to detect whether the object left the interaction space
    private EffectorRangeCheck RangeCheck;

    // becomes true in the first update, allows a 'late' start so to say
    public bool Started = false;
    public static bool IsActive = false;

    // position where the target will always appear
    public Vector3 InitialTargetPosition;
    // id of the interacting hand, in case of online data this can be used to check whether the
    // sensor lost the hand in between
    private int CurrentHandID;
    
    // just to control if and when the interaction state is checked (see the Update method)
    private bool CheckInteractionState;
    // the meta-controller, it is notified when the interaction is finished
    public Interactioncontroller master;
    // current state within the sequence
    public Interactioncontroller.InteractionStates InteractionState;
    // delegate to invoke the InteractionStateUpdate method
    private Action InteractionStateUpdateDelegate;

    void Awake() {
        GameObject rangeCheckContainer = GameObject.FindGameObjectWithTag(TagConstants.RANGE_CHECK);
        this.RangeCheck = rangeCheckContainer.GetComponent<EffectorRangeCheck>();
        this.RangeCheck.ObjectLeftBounds += this.rangeCheckHandler;

        // register container controller
        GameObject container = GameObject.FindGameObjectWithTag(TagConstants.CONTAINER);
        if (container != null) {
            ContainerController containerController = container.GetComponent<ContainerController>();
            if (containerController != null) {
                containerController.ObjectWasReleased += this.checkTargetPosition;
                containerController.ObjectWasDestroyed += this.checkTargetDestruction;
            }
        }

        foreach(GraspableObject target in Targets) {
            target.gameObject.SetActive(true);
            target.transform.position = new Vector3(0, -100, 100);
            target.GetComponent<Rigidbody>().isKinematic = true;
        }

        this.InteractionStateUpdateDelegate = delegate () {
            this.InteractionStateUpdate();
        };

        //setup interaction control
        this.CurrentInteractionState = new InteractionStateObject();
        this.CurrentInteractionState.InteractionStage = InteractionStage.STARTUP;

        if (GraspStateController.Verbose) UnityEngine.Debug.Log("done with awaking...");
    }

    void OnDestroy() {
        // unregister container controller
        GameObject container = GameObject.FindGameObjectWithTag("Container");
        if (container != null) {
            ContainerController containerController = container.GetComponent<ContainerController>();
            if (containerController != null) {
                containerController.ObjectWasReleased -= this.checkTargetPosition;
                containerController.ObjectWasDestroyed -= this.checkTargetDestruction;
            }
        }
        this.RangeCheck.ObjectLeftBounds -= this.rangeCheckHandler;
    }


    // Update is called once per frame
    void Update() {
        if (!this.Started && GraspStateController.IsActive) {
            this.Started = true;
            this.CurrentInteractionState.InteractionStage = InteractionStage.STARTUP;
            this.InteractionStateUpdate();
        }
        // check pose
        if (this.Started && GraspStateController.IsActive && this.CurrentInteractionState.InteractionStage == InteractionStage.STARTUP) {
            //this.checkInitialPose();
            //For now, we will set the 
            this.CurrentHandID = this.HandControllerReference.currentRightHandID;
            this.CurrentInteractionState.InteractionStage = InteractionStage.TARGET_PRESENTATION;
            this.InteractionStateUpdate();
        }

        // target presentation check
        if (this.Started && GraspStateController.IsActive && this.CurrentInteractionState.InteractionStage == InteractionStage.TARGET_PRESENTATION) {
            /*
            if (GraspStateController.Verbose) UnityEngine.Debug.Log("target presentation check...");
            if (!this.HandPoseController.checkInitialPose())
            {
                this.cancelAndResetInteractionSequence();
            }
            */
        }

        //check hand during trajectory
        if (this.Started && GraspStateController.IsActive && this.CurrentInteractionState.InteractionStage != InteractionStage.STARTUP) {
            // cancel interaction
            if (this.CurrentHandID != this.HandControllerReference.currentRightHandID && this.CurrentInteractionState.InteractionStage == InteractionStage.GRASPING_INTERACTION) {
                this.cancelAndResetInteractionSequence();
            }
        }

        //checkInteractionState = true after fixation
        if (this.Started && GraspStateController.IsActive && this.CheckInteractionState) {
            if (this.InteractionState != Interactioncontroller.InteractionStates.NONE) {
                this.InteractionStateUpdate();
                // just paranoia
                this.CheckInteractionState = false;
            }
        }
    }

    public void cancelAndResetInteractionSequence() {
        // try to stop coroutine
        StopCoroutine("CoroutineTimer.Start");
        // if target has been assigned, reset it
        this.cancelInteractionSequence();
        // reset interaction state...
        if (GraspStateController.Verbose) UnityEngine.Debug.Log("interaction canceled");
        this.CurrentInteractionState.InteractionStage = InteractionStage.WAITING;
        StartCoroutine(this.FeedbackInterval());
    }

    private IEnumerator FeedbackInterval() {
        yield return new WaitForSeconds(this.FeedbackIntervalInSeconds);
        this.FeedbackDisplay.text = "";
        this.master.GraspInteractionDone();

        yield return null;
    }

    public void ExternalInteractionStart() {
        this.CurrentInteractionState.InteractionStage = InteractionStage.STARTUP;
        this.InteractionStateUpdate();
    }
    /// <summary>
    /// this is the state machine that controls the interaction
    /// </summary>
    public void InteractionStateUpdate() {

        if (!GraspStateController.IsActive) {
            this.Started = false;
            return;
        }

        if (GraspStateController.Verbose) UnityEngine.Debug.Log("timer called, in state = " + CurrentInteractionState.InteractionStage.ToString() + "...");
        switch (this.CurrentInteractionState.InteractionStage) {
            case InteractionStage.STARTUP:
                if (GraspStateController.Verbose) UnityEngine.Debug.Log("startup...");
                this.FeedbackDisplay.text = "";

                if (GraspStateController.Verbose) UnityEngine.Debug.Log("startup done...");
                break;
            case InteractionStage.TARGET_PRESENTATION:
                // just paranoia, it is highly unlikely that something like this happens
                /*
                if (this.Target.GetComponent<GraspableObject>().IsGrabbed()) {
                    GraspController[] hands = GameObject.FindObjectsOfType<GraspController>();

                    foreach (GraspController hand in hands) {
                        hand.requestRelease();
                    }
                }
                */
                foreach (GraspableObject target in Targets) {
                    if (!target.IsInitialized()) {
                        if (this.bottleOrientationMode == "upright") {
                            target.ResetPositionAndOrientation(0.0f, this.InitialTargetPosition);
                            if (GraspStateController.Verbose) Debug.Log("upright target...");
                        }
                        else if (this.bottleOrientationMode == "upsidedown") {
                            target.ResetPositionAndOrientation(180.0f, this.InitialTargetPosition);
                            if (GraspStateController.Verbose) Debug.Log("rotated target...");
                        }

                        target.GetComponent<Rigidbody>().isKinematic = false;
                        target.GetComponent<Rigidbody>().useGravity = true;
                        target.SetInitialized(true);
                    }
                }

                this.CurrentInteractionState.InteractionStage = InteractionStage.GRASPING_INTERACTION;
                // enable range check
                this.InteractionState = Interactioncontroller.InteractionStates.NONE;
                this.CheckInteractionState = true;
                this.RangeCheck.clearMonitor();
                // TODO: add the target to the monitor, have a look at the EffectorRangeCheck script
                foreach(GraspableObject target in Targets) {
                    this.RangeCheck.monitorObject(target.gameObject);
                }

                if (Verbose) Debug.Log("current interaction state: " + CurrentInteractionState.InteractionStage);
                break;

            case InteractionStage.GRASPING_INTERACTION:
                if (GraspStateController.Verbose) Debug.Log("response state...");
                this.CheckInteractionState = false;

                if (this.InteractionState != Interactioncontroller.InteractionStates.IN_BOX) {
                    this.cancelInteractionSequence();
                } else {
                    this.FeedbackDisplay.text = this.positiveFeedbackTemplates[UnityEngine.Random.Range(0, this.positiveFeedbackTemplates.Length - 1)];

                    //this.deactivateTarget();
                    this.InteractionState = Interactioncontroller.InteractionStates.NONE;
                    // clear monitor
                    this.RangeCheck.clearMonitor();
                }
                this.CurrentInteractionState.InteractionStage = InteractionStage.WAITING;
                StartCoroutine(this.FeedbackInterval());

                break;
        }
    }

    private void cancelInteractionSequence() {
        this.RangeCheck.clearMonitor();

        GraspableObject lastGrabbedObject = GraspManager.Instance.getLastGrabbedOject();

        switch (this.InteractionState) {
            case Interactioncontroller.InteractionStates.NONE:
                this.FeedbackDisplay.text = "Die Hand muss im Sensorbereich bleiben.";

                foreach (GraspableObject target in Targets) {
                    this.deactivateTarget(target);
                }
                foreach (GraspableObject target in Targets) {
                    target.transform.position = new Vector3(0, -100, 100);
                    target.transform.rotation = Quaternion.identity;
                    target.SetInitialized(false);
                }

                break;
            case Interactioncontroller.InteractionStates.TARGET_DESTROYED:
                this.FeedbackDisplay.text = "Bitte mach die Flasche nicht kaputt.";

                this.deactivateTarget(lastGrabbedObject);
                lastGrabbedObject.transform.position = new Vector3(0, -100, 100);
                lastGrabbedObject.transform.rotation = Quaternion.identity;
                lastGrabbedObject.SetInitialized(false);

                break;
            case Interactioncontroller.InteractionStates.WRONG_ORIENTATION:
                this.FeedbackDisplay.text = "Bitte stell die Flasche richtig herum ab.";

                this.deactivateTarget(lastGrabbedObject);
                lastGrabbedObject.transform.position = new Vector3(0, -100, 100);
                lastGrabbedObject.transform.rotation = Quaternion.identity;
                lastGrabbedObject.SetInitialized(false);

                break;
            case Interactioncontroller.InteractionStates.OUT_OF_BOUNDS:
                this.FeedbackDisplay.text = "Objekt außer Reichweite.";

                this.deactivateTarget(lastGrabbedObject);
                lastGrabbedObject.transform.position = new Vector3(0, -100, 100);
                lastGrabbedObject.transform.rotation = Quaternion.identity;
                lastGrabbedObject.SetInitialized(false);

                break;
        }
    }

    private void rangeCheckHandler(GameObject checkedObject) {
        if (GraspStateController.Verbose) UnityEngine.Debug.Log(checkedObject.name + " left bounds...");
        this.InteractionState = Interactioncontroller.InteractionStates.OUT_OF_BOUNDS;
        this.cancelAndResetInteractionSequence();
    }

    private void deactivateTarget(GraspableObject target) {
        target.GetComponent<Rigidbody>().useGravity = false;
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.position = new Vector3(0, -100, 100);
        target.transform.rotation = Quaternion.identity;
        if (target.GetComponent<GraspableObject>().BreakableJoint != null) {
            Joint joint = target.GetComponent<GraspableObject>().BreakableJoint.GetComponent<Joint>();
            if (joint != null) {
                joint.connectedBody = null;
            }
        }
    }

    private void checkTargetPosition(GameObject gameObject, bool validOrientation) {
        if (!GraspStateController.IsActive) {
            return;
        }

        if (this.InteractionState == Interactioncontroller.InteractionStates.NONE && this.CurrentInteractionState.InteractionStage == InteractionStage.GRASPING_INTERACTION) {
            if (GraspStateController.Verbose) UnityEngine.Debug.Log("target in position...");
            this.InteractionState = validOrientation ? Interactioncontroller.InteractionStates.IN_BOX : Interactioncontroller.InteractionStates.WRONG_ORIENTATION;
        }
    }

    private void checkTargetDestruction(GameObject gameObject) {
        if (!GraspStateController.IsActive) {
            return;
        }

        if (this.InteractionState == Interactioncontroller.InteractionStates.NONE && this.CurrentInteractionState.InteractionStage == InteractionStage.GRASPING_INTERACTION) {
            if (GraspStateController.Verbose) UnityEngine.Debug.Log("target destroyed...");
            this.InteractionState = Interactioncontroller.InteractionStates.TARGET_DESTROYED;
        }
    }
}
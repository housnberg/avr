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
    
    private static long InitialPoseDuration = 500L;
    private long PoseCheckTimeStamp;
    // just to control if and when the interaction state is checked (see the Update method)
    private bool CheckInteractionState;
    // the meta-controller, it is notified when the interaction is finished
    public Interactioncontroller master;
    // current state within the sequence
    public Interactioncontroller.InteractionStates InteractionState;
    // delegate to invoke the InteractionStateUpdate method
    private Action InteractionStateUpdateDelegate;

    void Awake() {
        GameObject rangeCheckContainer = GameObject.FindGameObjectWithTag("RangeCheck");
        this.RangeCheck = rangeCheckContainer.GetComponent<EffectorRangeCheck>();
        this.RangeCheck.ObjectLeftBounds += this.rangeCheckHandler;

        // register container controller
        GameObject container = GameObject.FindGameObjectWithTag("Container");
        if (container != null) {
            ContainerController containerController = container.GetComponent<ContainerController>();
            if (containerController != null) {
                containerController.ObjectWasDestroyed += this.checkTargetDestruction;
            }
        }

        this.InteractionStateUpdateDelegate = delegate () { this.InteractionStateUpdate(); };

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
                containerController.ObjectWasDestroyed -= this.checkTargetDestruction;
            }
        }

        this.RangeCheck.ObjectLeftBounds -= this.rangeCheckHandler;
    }

    // Update is called once per frame
    void Update() {
        if (!this.Started && GraspStateController.IsActive) {
            this.Started = true;
            this.PoseCheckTimeStamp = -1L;
            this.CurrentInteractionState.InteractionStage = InteractionStage.STARTUP;
            this.InteractionStateUpdate();
        }

        // target presentation check
        if (this.Started && GraspStateController.IsActive && this.CurrentInteractionState.InteractionStage == InteractionStage.TARGET_PRESENTATION) {
            //Do nothing 
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
                this.CurrentInteractionState.InteractionStage = InteractionStage.GRASPING_INTERACTION;
                // enable range check
                this.InteractionState = Interactioncontroller.InteractionStates.NONE;
                this.CheckInteractionState = true;
                this.RangeCheck.clearMonitor();

                if (Verbose) Debug.Log("current interaction state: " + CurrentInteractionState.InteractionStage);
                break;

            case InteractionStage.GRASPING_INTERACTION:
                if (GraspStateController.Verbose) Debug.Log("response state...");
                this.CheckInteractionState = false;

                if (this.InteractionState != Interactioncontroller.InteractionStates.IN_BOX) {
                    this.cancelInteractionSequence();
                }  else {
                    this.InteractionState = Interactioncontroller.InteractionStates.NONE;
                    // clear monitor
                    this.RangeCheck.clearMonitor();

                    this.PoseCheckTimeStamp = -1L;
                }
                this.CurrentInteractionState.InteractionStage = InteractionStage.WAITING;
                StartCoroutine(this.FeedbackInterval());

                break;
        }
    }

    private void cancelInteractionSequence() {
        this.RangeCheck.clearMonitor();

        switch (this.InteractionState) {
            case Interactioncontroller.InteractionStates.NONE:
                this.FeedbackDisplay.text = "Die Hand muss im Sensorbereich bleiben.";
                break;
            case Interactioncontroller.InteractionStates.TARGET_DESTROYED:
                this.FeedbackDisplay.text = "Bitte mach die Flasche nicht kaputt.";
                break;
            case Interactioncontroller.InteractionStates.OUT_OF_BOUNDS:
                this.FeedbackDisplay.text = "Objekt außer Reichweite.";
                break;
        }
    }

    private void rangeCheckHandler(GameObject checkedObject) {
        if (GraspStateController.Verbose) UnityEngine.Debug.Log(checkedObject.name + " left bounds...");
        this.InteractionState = Interactioncontroller.InteractionStates.OUT_OF_BOUNDS;
        this.cancelAndResetInteractionSequence();
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
using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class SchoolslagDetection : MonoBehaviour
{

    public event Action<MoveState> ChangedMoveStateEvent;
    public enum MoveState { Idle, Push, Pull, Glide, Recovery }
    [Space]
    [Header("Pushing Parameters")]

    [Range(-1, 1)]
    [SerializeField] private float pushRangeMin;

    [Range(-1, 1)]
    [SerializeField] private float pushRangeMax;

    [Range(0, 180)]
    [SerializeField] private float minAngleAcceptanceDifferenceBetweenMoveAndForwardDirection;

    [Range(0, 180)]
    [SerializeField] private float maxAngleAcceptanceDifferenceBetweenMoveAndForwardDirection;

    [SerializeField] private float minPushSpeed;
    [SerializeField] private float pushMaxDistanceBetweenHands;

    [Space]
    [Header("Gliding Parameters")]

    [Range(-1, 1)]
    [SerializeField] private float glideRangeMin;

    [Range(-1, 1)]
    [SerializeField] private float glideRangeMax;
    [SerializeField] private float glideMaxDistanceBetweenHands;


    [Space]
    [Header("Pull Parameters")]

    [Range(-1, 1)]
    [SerializeField] private float pullRangeMin;

    [Range(-1, 1)]
    [SerializeField] private float pullRangeMax;

    [Range(0, 180)]
    [SerializeField] private float minAngleIntoRecovery;

    [Range(0, 180)]
    [SerializeField] private float maxAngleIntoRecovery;

    [Space]
    [Header("Recovery Parameters")]

    [Range(0, 180)]
    [SerializeField] private float minAngleRecoveryCheckActivation;

    [Range(0, 180)]
    [SerializeField] private float maxAngleRecoveryCheckActivation;

    [SerializeField] private float minRecoveryCheckSpeedActivation;
    [SerializeField] private float minIntoRecoverySpeed;



    [Space]
    [Header("refrences")]

    [SerializeField] private Transform leftHandTrans;
    [SerializeField] private Transform rightHandTrans;

    [SerializeField] private InputActionReference leftHandPushAction;
    [SerializeField] private InputActionReference rightHandPushAction;

    [SerializeField] private HandPhysics leftHandPhysics;
    [SerializeField] private HandPhysics rightHandPhysics;
    [SerializeField] private BodyPhysics bodyPhysics;

    [SerializeField] private Transform leftHandGyroReference;
    [SerializeField] private Transform rightHandGyroReference;

    [SerializeField] private MoveState moveState;

    private bool hasMovedLeftHandBack = false;
    private bool hasMovedRightHandBack = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //leftHandPushAction.action.Enable();
        //rightHandPushAction.action.Enable();
        moveState = MoveState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();

    }
    private void OnDisable()
    {
        //leftHandPushAction.action.Disable();
        //rightHandPushAction.action.Disable();
    }

    private void StateMachine()
    {
        switch (moveState)
        {
            case MoveState.Idle: InIdlePhase(); break;
            case MoveState.Push: InPushPhase(); break;
            case MoveState.Pull: InPullPhase(); break;
            case MoveState.Glide: GlidePhase(); break;
            case MoveState.Recovery: InRecoveryPhase(); break;
        }
    }

    private void ChangeState(MoveState newState)
    {
        Debug.Log("Go To " + newState.ToString());
        ChangedMoveStateEvent?.Invoke(newState);
        moveState = newState;
    }


    private void InIdlePhase()
    {
        // if both hands have the thumbs up((0 angle?)) && phase the same direction && and both have a forward direction  
        // go to pushPhase

        if (DetectIfPushing())
        {
            ChangeState(MoveState.Push);
        }


    }

    private void InPushPhase()
    {
        // if thumbs phase inward(90 angle?) && hands are close together // /// // // if you do a kick before the glide you get extra speed and a longer glide
        // go to glide state 

        // if thumbs phase inwards with an angle greater then 110 && hands move apart && hands go to upposite direction of body velocity
        // go to pullphase\

        if (DetectIfGliding())
        {
            ChangeState(MoveState.Glide);
        }

        if (DetectIfPulling())
        {
            hasMovedLeftHandBack = false;
            hasMovedRightHandBack = false;
            ChangeState(MoveState.Pull);
        }

        //if (DetectIfIdle())
        //{
        //    Debug.Log("Go To Idle");
        //    moveState = MoveState.Idle;
        //    return;
        //}

    }
    private void GlidePhase() // lowest drag and best result 
    {
        // zolang je handen dichtbij genoeg zijn werk de glide, maar je gaat wel uiteindelijk vertragen.
        if (TargetsDistanceInRange(leftHandPhysics.Transform.position, rightHandPhysics.Transform.position, glideMaxDistanceBetweenHands)) return;

        if (DetectIfPulling())
        {
            hasMovedLeftHandBack = false;
            hasMovedRightHandBack = false;

            ChangeState(MoveState.Pull);
        }

        // if thumbs phase inwards with an angle greater then 110 && hands move apart && hands go to upposite direction of body velocity
        // go to pullphase
    }

    private void InPullPhase()
    {
        // if hands velocity is zero < 0 (hands are moving forward)
        // go to idle

        // if hands velocity is > zero (hands are moving backward) continue


        // if both hands have the thumbs up((0 angle?)) && phase the same direction && and both have a forward direction  
        // go to pushPhase

        // the bigger the difference between, your new Velocity and the bodyMoveDirection the bigger the drag and mabey a reduction in speed;
        if (DetectIfRecovering())
        {
            ChangeState(MoveState.Recovery);
        }
    }

    private void InRecoveryPhase()
    {

        if (DetectIfPushing())
        {
            ChangeState(MoveState.Push);
        }

        //if (DetectIfIdle())
        //{
        //    Debug.Log("Go To Idle");
        //    moveState = MoveState.Idle;
        //    return;
        //}

    }


    private bool DetectIfPushing()
    {
        bool lefthandIsPushing = false;


        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, pushRangeMin, pushRangeMax, Hand.Left);
        bool lefthandMovementIsGood = IsAngleDifferenceAcceptable(leftHandPhysics.transform.forward, leftHandPhysics.normalizedDirection, 0, maxAngleAcceptanceDifferenceBetweenMoveAndForwardDirection);

        if (leftHandRotationInRange && lefthandMovementIsGood)
        {
            lefthandIsPushing = true;
        }

        bool righthandIsPushing = false;

        bool rightHandRotationInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up, pushRangeMin, pushRangeMax, Hand.Right);
        bool righthandMovementIsGood = IsAngleDifferenceAcceptable(rightHandPhysics.transform.forward, rightHandPhysics.normalizedDirection, minAngleAcceptanceDifferenceBetweenMoveAndForwardDirection, maxAngleAcceptanceDifferenceBetweenMoveAndForwardDirection);

        if (rightHandRotationInRange && righthandMovementIsGood)
        {
            righthandIsPushing = true;
        }

        //Debug.Log(leftHandRotationInRange + " | " + rightHandRotationInRange);
        if (lefthandIsPushing && righthandIsPushing && TargetsDistanceInRange(leftHandPhysics.Transform.position, rightHandPhysics.Transform.position, pushMaxDistanceBetweenHands)) return true;
        return false;
    }

    private bool DetectIfPulling()
    {


        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, pullRangeMin, pullRangeMax, Hand.Left);
        bool rightHandRotationInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up, pullRangeMin, pullRangeMax, Hand.Right);

        if (leftHandRotationInRange && rightHandRotationInRange) return true;
        return false;


    }

    private bool DetectIfRecovering()
    {

        // calculate the angle between the movement velocity of the body and a hand to determine if it's pushing or brining it;s hands forward "Recovery"
        if (bodyPhysics.Speed <= 0) return false;
        if (leftHandPhysics.Speed <= 0 && rightHandPhysics.Speed <= 0) return false;

        if (hasMovedLeftHandBack == false || hasMovedRightHandBack == false)
        {

            //Debug.Log("----");
            bool leftHandVelocityDirIsGoingBack = IsAngleDifferenceAcceptable(bodyPhysics.transform.forward, leftHandPhysics.Direction, minAngleRecoveryCheckActivation, maxAngleRecoveryCheckActivation);
            if (leftHandVelocityDirIsGoingBack && leftHandPhysics.Speed > minRecoveryCheckSpeedActivation)
            {
                hasMovedLeftHandBack = true;
            }

            bool rightHandVelocityDirIsGoingBack = IsAngleDifferenceAcceptable(bodyPhysics.transform.forward, rightHandPhysics.Direction, minAngleRecoveryCheckActivation, maxAngleRecoveryCheckActivation);
            if (rightHandVelocityDirIsGoingBack && rightHandPhysics.Speed > minRecoveryCheckSpeedActivation)
            {
                hasMovedRightHandBack = true;
            }

            //Debug.Log("LSpeed : " + leftHandPhysics.Speed + " | RSpeed : " + rightHandPhysics.Speed);

            if (hasMovedLeftHandBack == false || hasMovedRightHandBack == false) return false;
        }

        bool leftHandIsMovingTowardsBodymoveDir = IsAngleDifferenceAcceptable(bodyPhysics.transform.forward, leftHandPhysics.Direction, minAngleIntoRecovery, maxAngleIntoRecovery);
        bool rightHandIsMovingTowardsBodymoveDir = IsAngleDifferenceAcceptable(bodyPhysics.transform.forward, rightHandPhysics.Direction, minAngleIntoRecovery, maxAngleIntoRecovery);

        if (leftHandIsMovingTowardsBodymoveDir && leftHandPhysics.Speed >= minIntoRecoverySpeed && rightHandIsMovingTowardsBodymoveDir && rightHandPhysics.Speed >= minIntoRecoverySpeed) return true;
        return false;
    }
    private bool DetectIfGliding()
    {
        // if hands push and front direction is the direction of body Movement Velocity
        // hands need to be close to eachother
        // ?Distance check for how far you have yoiur arms? Mabey

        if (bodyPhysics.Speed <= 0.0) return false;

        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, glideRangeMin, glideRangeMax, Hand.Left);
        bool rightHandRotationInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up, glideRangeMin, glideRangeMax, Hand.Right);


        if (leftHandRotationInRange && rightHandRotationInRange) return true;
        return false;

        //if (leftHandRotationInRange) return true;
        //return false;
    }
    private bool DetectIfIdle()
    {
        bool leftHandIsMoving = true;
        bool rightHandIsMoving = true;

        //Debug.Log(leftHandPhysics.Speed);

        if (leftHandPhysics.Speed >= 0.0 && leftHandPhysics.Speed < minPushSpeed)
        {
            leftHandIsMoving = false;

        }

        //if (rightHandPhysics.Speed > 0.0 && rightHandPhysics.Speed < minPushSpeed)
        //{
        //    rightHandIsMoving = true;
        //}


        if (leftHandIsMoving) return false;
        return true;
    }

    private bool CheckIfRotationIsInRange(Vector3 input, Vector3 refrenceVector, float minRange, float maxRange, Hand hand)
    {
        float dotProduct = Vector3.Dot(input, refrenceVector);



        if (dotProduct >= minRange && dotProduct <= maxRange) return true;
        return false;

    }

    private void CheckIfHandsAreFacingTheSameWay(Vector3 leftForwardDir, Vector3 rightForwardDir)
    {
        //float handAngleDifference = Vector3.SignedAngle(leftForwardDir, rightForwardDir);

    }

    private bool IsAngleDifferenceAcceptable(Vector3 vectorA, Vector3 vectorB, float minAngle, float maxAngle)
    {
        float angle = Vector3.Angle(vectorA, vectorB);
        Debug.Log("angle : " + angle);

        if (angle >= minAngle && angle <= maxAngle) return true;
        return false;
    }

    private bool TargetsDistanceInRange(Vector3 posA, Vector3 posB, float maxDistance)
    {
        float distance = Vector3.Distance(posA, posB);
        if (distance < maxDistance) return true;
        return false;

    }

}

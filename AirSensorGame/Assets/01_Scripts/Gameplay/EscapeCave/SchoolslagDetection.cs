using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SchoolslagDetection : MonoBehaviour
{
    public enum MoveState { Idle, Push, Pull, Glide, }
    [Space]
    [Header("Pushing Parameters")]

    [Range(-1, 1)]
    [SerializeField] private float pushRangeMin;

    [Range(-1, 1)]
    [SerializeField] private float pushRangeMax;

    [SerializeField] private float maxAngleAcceptanceDifferenceBetweenMoveAndForwardDirection;
    [SerializeField] private float minPushSpeed;
    [SerializeField] private float maxDistance;
    [SerializeField] private float leftDot;
    [SerializeField] private float rightDot;

    [Space]
    [Header("Gliding Parameters")]

    [Range(-1, 1)]
    [SerializeField] private float glideRangeMin;

    [Range(-1, 1)]
    [SerializeField] private float glideRangeMax;

    [Space]
    [Header("Pull Parameters")]

    [Range(-1, 1)]
    [SerializeField] private float pullRangeMin;

    [Range(-1, 1)]
    [SerializeField] private float pullRangeMax;

    [Space]
    [Header("refrences")]

    [SerializeField] private Transform leftHandTrans;
    [SerializeField] private Transform rightHandTrans;

    [SerializeField] private InputActionReference leftHandPushAction;
    [SerializeField] private InputActionReference rightHandPushAction;

    [SerializeField] private HandPhysics leftHandPhysics;
    [SerializeField] private HandPhysics rightHandPhysics;

    [SerializeField] private Transform leftHandGyroReference;
    [SerializeField] private Transform rightHandGyroReference;

    private MoveState moveState;


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

    private void StateMachine()
    {
        switch (moveState)
        {
            case MoveState.Idle: InIdlePhase(); break;
            case MoveState.Push: InPushPhase(); break;
            case MoveState.Pull: InPullPhase(); break;
            case MoveState.Glide: GlidePhase(); break;
        }
    }

    private void OnDisable()
    {
        //leftHandPushAction.action.Disable();
        //rightHandPushAction.action.Disable();
    }

    private void InIdlePhase()
    {
        // if both hands have the thumbs up((0 angle?)) && phase the same direction && and both have a forward direction  
        // go to pushPhase

        if (DetectIfPushing())
        {
            Debug.Log("Go To Pushing");
            moveState = MoveState.Push;
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
            Debug.Log("Go To Gliding");
            moveState = MoveState.Glide;
        }


    }
    private void GlidePhase() // lowest drag and best result 
    {
        if (DetectIfPulling())
        {
            Debug.Log("Go To Pulling");
            moveState = MoveState.Pull;
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

        if (DetectIfPushing())
        {
            Debug.Log("Go To Pushing");
            moveState = MoveState.Push;
        }
    }


    private bool DetectIfPushing()
    {
        //bool lefthandIsPushing = false;

        //bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, pushRangeMin, pushRangeMax);
        //bool lefthandMovementIsGood = IsHandMovingSameDirectionAsForwardDir(leftHandPhysics.transform.forward, leftHandPhysics.normalizedDirection);

        //if (leftHandRotationInRange && lefthandMovementIsGood)
        //{
        //    lefthandIsPushing = true;
        //}

        //bool righthandIsPushing = false;

        //bool rightHandRotationInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up * -1, pushRangeMin, pushRangeMax);
        //bool righthandMovementIsGood = IsHandMovingSameDirectionAsForwardDir(rightHandPhysics.transform.forward, rightHandPhysics.normalizedDirection);

        //if (rightHandRotationInRange && righthandMovementIsGood)
        //{
        //    righthandIsPushing = true;
        //}

        //if (lefthandIsPushing && righthandIsPushing && CloseEnough()) return true;
        //return false;

        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, pushRangeMin, pushRangeMax);
        //bool rightHandRotationInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up * -1, pushRangeMin, pushRangeMax);

        if (leftHandRotationInRange) return true;
        return false;

    }

    private bool DetectIfPulling()
    {
        // lefthand
        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, pullRangeMin, pullRangeMax);
        //bool rightHandInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up * -1, pullRangeMin, pullRangeMax);

        //if (leftHandInRange && rightHandInRange) return true;
        //return false;

        if (leftHandRotationInRange) return true;
        return false;
    }
    private bool DetectIfGliding()
    {
        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, glideRangeMin, glideRangeMax);
        //bool rightHandInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up * -1, glideRangeMin, glideRangeMax);

        //if (leftHandInRange && rightHandInRange) return true;
        //return false;

        if (leftHandRotationInRange) return true;
        return false;
    }
    private bool DetectIfIdle()
    {
        bool leftHandRotationInRange = CheckIfRotationIsInRange(leftHandPhysics.Transform.up, leftHandGyroReference.up, pushRangeMin, pullRangeMax);
        bool rightHandInRange = CheckIfRotationIsInRange(rightHandPhysics.Transform.up, rightHandGyroReference.up * -1, pushRangeMin, pullRangeMax);

        //if (leftHandInRange && rightHandInRange) return true;
        //return false;

        if (leftHandRotationInRange) return true;
        return false;
    }

    private bool CheckIfRotationIsInRange(Vector3 input, Vector3 refrenceVector, float minRange, float maxRange)
    {
        float dotProduct = Vector3.Dot(input, refrenceVector);
        leftDot = dotProduct;
        //Debug.Log("Dot : " + dotProduct);
        if (dotProduct >= minRange && dotProduct <= maxRange) return true;
        return false;

    }

    private void CheckIfHandsAreFacingTheSameWay(Vector3 leftForwardDir, Vector3 rightForwardDir)
    {
        //float handAngleDifference = Vector3.SignedAngle(leftForwardDir, rightForwardDir);

    }

    private bool IsHandMovingSameDirectionAsForwardDir(Vector3 forwardDir, Vector3 VelocityDirection)
    {

        float handAngleDifference = Vector3.AngleBetween(forwardDir, VelocityDirection);

        if (handAngleDifference < maxAngleAcceptanceDifferenceBetweenMoveAndForwardDirection) return true;
        return false;
    }

    private bool CloseEnough()
    {
        float distance = Vector3.Distance(leftHandPhysics.transform.position, rightHandPhysics.transform.position);
        if (distance < maxDistance) return true;
        return false;

    }
}

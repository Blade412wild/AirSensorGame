using UnityEngine;

public class SwimmingPhysicsTest : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform headTransform;
    [SerializeField] private SwimControls swimControls;
    [SerializeField] private Animator animator;

    [Space]
    [Header("SwimmingPhases Parameters")]
    [SerializeField] private Vector2 pushPhase;
    [SerializeField] private Vector2 glidePhase;
    [SerializeField] private Vector2 pullPhase;
    [SerializeField] private Vector2 recoveryPhase;

    [Space]
    [Header("Propulsion Force Parameters")]
    [SerializeField] private float Force;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private AnimationCurve ForceCurve;
    [SerializeField] private float animationDuration;
    [SerializeField] private float maxSpeed;

    [Space]
    [SerializeField] private float kickForce;


    [Space]
    [Header("DragForce Parameters")]
    [SerializeField] private float baseDrag;

    [Space]
    [SerializeField] float pushMultiplier;

    [Space]
    [SerializeField] float glideMultiplier;

    [Space]
    [SerializeField] float pullMultiplier;

    [Space]
    [SerializeField] float recoveryMultiplier;

    [Space]
    [SerializeField] private AnimationCurve dragCurve;

    [Space]
    [Header("Final")]
    [SerializeField] private bool useCurve;
    [SerializeField] private bool startForce;
    [SerializeField] private bool legKick;

    [Space]
    [Header("Debug")]
    [SerializeField] private Vector3 OwnCalculatedVelocity;
    [SerializeField] private Vector3 RigidbodyCalculatedVelocity;

    public Rigidbody PlayerRidigBody => rigidbody;

    private bool applyPropulsionForce;
    private Vector3 currentMoveVelocity;
    private Vector3 previousPos;
    private float Animationprogess = 0;
    private bool inAnimation = false;
    private bool lastPhysicsUpdate;
    private MoveState currentMoveState;
    private Vector3 previousMoveDir;
    private Quaternion previousRotation;
    private Vector3 currentMoveDir;
    private Quaternion currentRotation;

    private bool firstMovementAfterIdle = false;

    private bool mayUpdate = true;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rigidbody is null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!mayUpdate) return;
        CheckInput();

        if (startForce)
        {
            startForce = false;

            if (inAnimation)
            {
                ResetAnimation();
            }

            applyPropulsionForce = true;

            if (useCurve)
            {
                inAnimation = true;
            }
            else
            {
                lastPhysicsUpdate = true;
            }

        }

        if (legKick)
        {
            legKick = false;
            ApplyKick();
        }

        if (inAnimation)
        {
            Animationprogess += Time.deltaTime;

            if (Animationprogess >= animationDuration)
            {
                Animationprogess = animationDuration;

                //Debug.Log("Animation is done");
                //lastPhysicsUpdate = true;
                //ResetAnimation();
                //ApplyKick();
            }
        }
        SetMoveDir();

    }

    private void FixedUpdate()
    {
        SetCurrentMoveRotation();
        if (!mayUpdate) return;
        currentMoveVelocity = (transform.position - previousPos) / Time.deltaTime;
        OwnCalculatedVelocity = currentMoveVelocity;
        RigidbodyCalculatedVelocity = rigidbody.GetPointVelocity(transform.position);

        if (rigidbody.GetRelativePointVelocity(transform.position).sqrMagnitude > 0.0f)
        {
            float ForceMultiplier = dragCurve.Evaluate(Animationprogess);

            ApplyDrag();
        }

        if (applyPropulsionForce)
        {
            if (lastPhysicsUpdate)
            {
                lastPhysicsUpdate = false;
                applyPropulsionForce = false;
            }

            if (inAnimation)
            {
                float ForceMultiplier = ForceCurve.Evaluate(Animationprogess);

                ApplyForce(ForceMultiplier * Force);
                SpeedControl();

            }
            else
            {
                ApplyForce(1 * Force);
                SpeedControl();
            }


        }

        previousPos = transform.position;



    }

    public void StopMovement()
    {
        mayUpdate = false;
        animator.SetBool("Move", false);


    }

    public void ContinueMovement()
    {
        mayUpdate = true;
        
    }

    private void ApplyDrag()
    {
        Vector3 dragDir = (rigidbody.GetPointVelocity(transform.position).normalized * -1);
        float drag = CalculateDrag();
        rigidbody.AddForce(dragDir * drag, ForceMode.Force);
    }


    private void ApplyForce(float force)
    {
        rigidbody.AddForce(currentMoveDir * force, forceMode);
    }

    private void ApplyKick()
    {
        rigidbody.AddForce(currentMoveDir * kickForce, ForceMode.Impulse);
    }

    private void ResetAnimation()
    {
        //Debug.Log(" reset");
        Animationprogess = 0;
        inAnimation = false;
        //startForce = true;

    }

    private void StartAnimation()
    {
        inAnimation = true;

    }

    private float CalculateDrag()
    {
        //MoveState currentMoveState = GetCurrentBreathingState();
        float multiplier = GetDragMultiplier(currentMoveState);
        return baseDrag * multiplier;

    }

    //private float CalculatePropulsion()
    //{
    //    MoveState currentMoveState = GetCurrentBreathingState();

    //}

    private MoveState GetCurrentBreathingState()
    {
        if (IsInRage(Animationprogess, pushPhase.x, pushPhase.y)) return MoveState.Push;
        if (IsInRage(Animationprogess, pullPhase.x, pullPhase.y)) return MoveState.Pull;
        if (IsInRage(Animationprogess, recoveryPhase.x, recoveryPhase.y)) return MoveState.Recovery;
        if (IsInRage(Animationprogess, glidePhase.x, glidePhase.y)) return MoveState.Glide;
        return MoveState.Idle;
    }

    private float GetDragMultiplier(MoveState state)
    {
        float dragMultiplier = 1;
        switch (state)
        {
            case MoveState.Push: dragMultiplier = pushMultiplier; break;
            case MoveState.Pull: dragMultiplier = pullMultiplier; break;
            case MoveState.Recovery: dragMultiplier = recoveryMultiplier; break;
        }
        return dragMultiplier;
    }

    private bool IsInRage(float currentValue, float min, float max)
    {
        if (currentValue >= min && currentValue <= max) return true;
        return false;
    }

    private void SpeedControl()
    {
        Vector3 currentVelocity = rigidbody.GetPointVelocity(transform.position);

        if (currentVelocity.magnitude >= maxSpeed)
        {
            //Debug.Log("magnitude : " + currentVelocity.magnitude);
            rigidbody.linearVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
        }
    }

    public void SwitchMovementPhase(MoveState state)
    {
        currentMoveState = state;
        //Debug.Log("currentState : " + currentMoveState);
        switch (state)
        {
            case MoveState.Push: SwitchedToPushPhase(); break;
            case MoveState.Pull: SwitchedToPullPhase(); break;
            case MoveState.Recovery: SwitchedToRecoveryPhase(); break;

        }
    }

    public void SwitchedToPushPhase()
    {
        currentMoveState = MoveState.Push;
        //applyForce = true;
        ApplyKick();
    }
    public void SwitchedToPullPhase()
    {
        currentMoveState = MoveState.Pull;
        SetMoveDir();
        SetCurrentMoveRotation();
        applyPropulsionForce = true;
        StartAnimation();
    }
    public void SwitchedToRecoveryPhase()
    {
        currentMoveState = MoveState.Recovery;
        applyPropulsionForce = false;
        ResetAnimation();

    }
    public void SwitchedToGlidePhase()
    {
        currentMoveState = MoveState.Glide;
    }



    private void SwitchedToIdleState()
    {
        currentMoveState = MoveState.Idle;
        animator.SetBool("Move", false);
    }

    private void CheckInput()
    {
        if (swimControls.leftInputInt.y == 1)
        {
            animator.SetBool("Move", true);
        }
        else if (swimControls.leftInputInt.y == 0)
        {
            SwitchedToIdleState();
        }
    }



    private void SetMoveDir()
    {
        if (currentMoveDir != Vector3.zero)
        {
            previousMoveDir = currentMoveDir;
        }
        currentMoveDir = headTransform.forward;
    }

    private void SetCurrentMoveRotation()
    {
        //if (currentRotation != )
        //{
        //    previousRotation = currentRotation;
        //}

        currentRotation = GetCurrentXYRotation();
        transform.rotation = currentRotation;

    }

    private Quaternion GetCurrentXYRotation()
    {
        // Get controller orientation
        Quaternion q = headTransform.rotation;

        // Extract forward and up vectors from the controller

        //Vector3 fwd = q * headTransform.forward;
        //Vector3 up = q * headTransform.up;

        // Project both onto planes so Z-rotation is removed
        //fwd.z = 0;  // Keep yaw
        //up.z = 0;   // Keep pitch

        //fwd.Normalize();
        //up.Normalize();

        // Build a corrected rotation with no roll (Z)
        //Quaternion noZ = Quaternion.LookRotation(fwd, up);
        return q;

        // Apply it to the reference
        //transform.rotation = noZ;
    }


}

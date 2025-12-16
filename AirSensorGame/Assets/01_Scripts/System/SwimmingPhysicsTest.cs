using UnityEngine;

public class SwimmingPhysicsTest : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform headTransform;

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

    private bool applyForce;
    private Vector3 currentMoveVelocity;
    private Vector3 previousPos;
    private float Animationprogess = 0;
    private bool inAnimation = false;
    private bool lastPhysicsUpdate;



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
        if (startForce)
        {
            startForce = false;

            if (inAnimation)
            {
                ResetAnimation();
            }

            applyForce = true;

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

                Debug.Log("Animation is done");
                lastPhysicsUpdate = true;
                ResetAnimation();
                ApplyKick();
            }
        }


    }

    private void FixedUpdate()
    {
        currentMoveVelocity = (transform.position - previousPos) / Time.deltaTime;
        OwnCalculatedVelocity = currentMoveVelocity;
        RigidbodyCalculatedVelocity = rigidbody.GetPointVelocity(transform.position);

        if (rigidbody.GetRelativePointVelocity(transform.position).sqrMagnitude > 0.0f)
        {
            float ForceMultiplier = dragCurve.Evaluate(Animationprogess);

            ApplyDrag();
        }

        if (applyForce)
        {
            if (lastPhysicsUpdate)
            {
                lastPhysicsUpdate = false;
                applyForce = false;
            }

            if (forceMode == ForceMode.Force && useCurve == true)
            {
                float ForceMultiplier = ForceCurve.Evaluate(Animationprogess);

                ApplyForce(1 * Force);
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

    private void ApplyDrag()
    {
        Vector3 dragDir = (rigidbody.GetPointVelocity(transform.position).normalized * -1);
        float drag = CalculateDrag();
        rigidbody.AddForce(dragDir * drag, ForceMode.Force);
    }


    private void ApplyForce(float force)
    {
        rigidbody.AddForce(headTransform.forward * force, forceMode);
    }

    private void ApplyKick()
    {
        rigidbody.AddForce(headTransform.forward * kickForce, ForceMode.Impulse);
    }

    private void ResetAnimation()
    {
        Debug.Log(" reset");
        Animationprogess = 0;
        inAnimation = false;
        startForce = true;

    }

    private void StartAnimation()
    {
        inAnimation = true;
    }

    private float CalculateDrag()
    {
        MoveState currentMoveState = GetCurrentBreathingState();
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
            Debug.Log("magnitude : " + currentVelocity.magnitude);
        }
    }
}

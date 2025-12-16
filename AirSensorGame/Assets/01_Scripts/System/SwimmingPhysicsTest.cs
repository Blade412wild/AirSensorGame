using UnityEngine;

public class SwimmingPhysicsTest : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform headTransform;

    [Space]
    [Header("Propulsion Force Parameters")]
    [SerializeField] private float Force;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private AnimationCurve ForceCurve;
    [SerializeField] private float animationDuration;

    [Space]
    [SerializeField] private float kickForce;


    [Space]
    [Header("DragForce Parameters")]
    [SerializeField] private float drag;
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

            ApplyDrag(ForceMultiplier * drag);
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

            }
            else
            {
                ApplyForce(1 * Force);

            }


        }

        previousPos = transform.position;



    }

    private void ApplyDrag(float force)
    {
        Vector3 dragDir = (rigidbody.GetPointVelocity(transform.position).normalized * -1);
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

    private void CalculateDrag()
    {
         
    }

}

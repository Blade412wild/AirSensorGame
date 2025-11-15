using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class UpDownMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference spaceBarValue;
    [SerializeField] private InputActionReference wasdMovementAction;
    [SerializeField] private float speedScalar = 7;
    [SerializeField] private Transform MaxTrans;
    [SerializeField] private Transform MinTrans;

    [SerializeField] private BreathingDeviceData data;

    private Transform transform;

    public bool isblowing;
    public bool useSensor;
    public bool reset;

    [Space]
    [Range(0, 15)] public float airvelocity;

    private Vector3 max => MaxTrans.position;
    private Vector3 min => MinTrans.position;

    private Vector3 startPos;

    bool isFalling;
    bool reachedMin;
    bool reachedMax;
    float acceleration = -9.81f;
    Vector3 gravity = new Vector3(0, -9.81f, 0);
    Vector3 UpForce = new Vector3(0, 12, 0);

    Vector3 xAxisVelocity = new Vector3(0, 0, 0);
    Vector3 yAxisVelocity = new Vector3(0, 0, 0);
    Vector3 velocity;

    Vector2 keyInput;

    private void Start()
    {
        transform = GetComponent<Transform>();
        startPos = transform.position;
        spaceBarValue.action.Enable();
        wasdMovementAction.action.Enable();


    }

    private void Update()
    {
        SetUpForce();
        if (transform.position.y >= max.y)
        {
            reachedMax = true;
            velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, max.y, transform.position.z);
        }
        else if (transform.position.y <= min.y)
        {
            //Debug.Log("miny : " + min.y + " player : " + transform.position.y);
            reachedMin = true;
            velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, min.y, transform.position.z);
        }
        else
        {
            reachedMin = false;
            reachedMax = false;
        }

        keyInput = wasdMovementAction.action.ReadValue<Vector2>();
        

        //if (spaceBarValue.action.IsPressed())
        //{
        //    isblowing = true;
        //}
        //else
        //{
        //    isblowing = false;
        //}

        if (data.AirVelocity > 0 || isblowing)
        {

        }
        else // falling 
        {
            if (transform.position.y <= min.y)
            {
                //Debug.Log("miny : " + min.y + " player : " + transform.position.y);
                reachedMin = true;
                velocity = Vector3.zero;
                transform.position = new Vector3(transform.position.x, min.y, transform.position.z);
            }
            else
            {
                reachedMin = false;
            }

        }
    }

    private void FixedUpdate()
    {
        if (reset)
        {
            reset = false;
            velocity = Vector3.zero;
            transform.position = startPos;
        }

        SetXaxisVelocity();

        if (!reachedMin)
        {
            CalculateNewPosWithGravity();
        }

        if (isblowing && !reachedMax)
        {
            CalculateNewPosWithBoost();
        }

        velocity = yAxisVelocity + xAxisVelocity;
        transform.position += velocity;
    }

    private void CalculateNewPosWithGravity()
    {
        yAxisVelocity += gravity * Mathf.Pow(Time.deltaTime, 2);
        //yAxisVelocity += velocity;
    }

    private void CalculateNewPosWithBoost()
    {
        yAxisVelocity += UpForce * Mathf.Pow(Time.deltaTime, 2);
        //yAxisVelocity += velocity;
    }

    private void SetXaxisVelocity()
    {
        xAxisVelocity = (keyInput * speedScalar) * Time.deltaTime;
    }

    private void SetUpForce()
    {
        //data.AirVelocitys
        if (useSensor)
        {
            airvelocity = data.AirVelocity;
            UpForce.y = airvelocity;
        }
        else
        {
            UpForce.y = airvelocity;
        }

        if (airvelocity > 0)
        {
            isblowing = true;
        }
        else
        {
            isblowing = false;
        }
    }


}

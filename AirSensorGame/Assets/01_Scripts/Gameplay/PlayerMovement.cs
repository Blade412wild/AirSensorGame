using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PrototypeGameManager gameManager;
    [SerializeField] private InputActionReference spaceBarValue;
    [SerializeField] private InputActionReference wasdMovementAction;
    [SerializeField] private float speedScalar = 7;
    [SerializeField] private Transform MaxTrans;
    [SerializeField] private Transform MinTrans;

    [SerializeField] private BreathingDeviceData data;


    [SerializeField] private bool useSensor;

    [Space]
    [SerializeField]
    [Range(0, 15)]
    private float airvelocity;

    private Vector3 max => MaxTrans.position;
    private Vector3 min => MinTrans.position;

    private Transform transform;
    private Vector3 startPos;

    private Vector3 gravity = new Vector3(0, -9.81f, 0);
    private Vector3 UpForce = new Vector3(0, 12, 0);

    private Vector3 xAxisVelocity = new Vector3(0, 0, 0);
    private Vector3 yAxisVelocity = new Vector3(0, 0, 0);
    private Vector3 velocity;

    private Vector2 keyInput;

    private bool isblowing;
    private bool reachedMin;
    private bool reachedMax;
    private bool mayMove;

    private void Start()
    {
        transform = GetComponent<Transform>();
        startPos = transform.position;
        spaceBarValue.action.Enable();
        wasdMovementAction.action.Enable();
        gameManager.PlayerStartedGame += HandlePlayerStartedGameEvent;
    }


    private void Update()
    {
        if (!mayMove) return;

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
        if (!mayMove) return;


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
    private void HandlePlayerStartedGameEvent()
    {
        mayMove = true;
    }


}

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
    [SerializeField] private LayerMask groundLayer;


    [SerializeField] private bool useSensor;

    [Space]
    [SerializeField]
    [Range(0, 15)]
    private float airvelocity;

    [SerializeField] private float groundDistanceCheckRange;
    private bool reachedLowerBorder;

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
    private bool reachedUpperBorder;
    private bool reachedLeftBorder;
    private bool reachedRightBorder;

    private bool FirstYBorderTouch = true;
    private bool FirstXBorderTouch = true;

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
        HandleBorders();

        keyInput = wasdMovementAction.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!mayMove) return;

        CheckBorders();

        SetXaxisVelocity();

        if (!reachedLowerBorder)
        {
            CalculateNewPosWithGravity();
        }


        if (isblowing && !reachedUpperBorder)
        {
            CalculateNewPosWithBoost();
        }

        HandleBorders();

        velocity = yAxisVelocity + xAxisVelocity;
        transform.position += velocity;
        Debug.Log(velocity);
    }

    private void HandleBorders()
    {
        if (reachedUpperBorder && FirstYBorderTouch || reachedLowerBorder && FirstYBorderTouch) // resetting velocity
        {
            velocity.y = 0;
            yAxisVelocity.y = 0;
            FirstYBorderTouch = false;
            //Debug.Log("--" + velocity);

        }

        if (reachedLeftBorder && FirstXBorderTouch || reachedRightBorder && FirstXBorderTouch) // resetting velocity
        {
            velocity.x = 0;
            xAxisVelocity.x = 0;
            FirstXBorderTouch = false;
            //Debug.Log("++" + velocity);
        }

        if (!reachedUpperBorder && !reachedLowerBorder)
        {
            FirstYBorderTouch = true;
        }

        if (!reachedLeftBorder && !reachedRightBorder)
        {
            FirstXBorderTouch = true;
        }


    }

    private void CheckBorders()
    {
        reachedLowerBorder = ShootRaycast(Vector3.down);

        if (!reachedLowerBorder)
        {
            reachedUpperBorder = ShootRaycast(Vector3.up);
        }

        reachedRightBorder = ShootRaycast(Vector3.right);

        if (!reachedRightBorder)
        {
            reachedLeftBorder = ShootRaycast(Vector3.left);
        }

    }

    private bool ShootRaycast(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, groundDistanceCheckRange, groundLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(dir) * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(dir) * 1000, Color.white);
            return false;
        }
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
        if (reachedLeftBorder && keyInput.x <0)
        {
            xAxisVelocity.x = 0;
        }
        if (reachedRightBorder && keyInput.x > 0)
        {
            xAxisVelocity.x = 0;
        }
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

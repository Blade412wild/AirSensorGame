using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class HandPhysics : MonoBehaviour
{
    public enum Hand { Left, Right }

    public bool UpdateFollowObject;

    public Vector3 CurrentVelocity { get; private set; }
    public Vector3 Direction { get; private set; }
    public Vector3 normalizedDirection => Direction.normalized;

    public float Speed => Direction.magnitude / Time.deltaTime;
    public Transform Transform { get; private set; }

    [SerializeField] private Hand hand;
    [SerializeField] private InputActionReference buttonRefrence;
    [SerializeField] private Transform followingObject;

    private Vector3 newDirection;
    private Vector3 previousVelocity;

    private Vector3 currentPos;
    private Vector3 previousPos;

    private bool previousTrackingState;


    private void Start()
    {
        Transform = GetComponent<Transform>();
    }

    private void Update()
    {
        UpdateHandPhysics();
        UpdateFollowingObject();
    }

    private void UpdateHandPhysics()
    {
        currentPos = Transform.position;

        if (previousPos == Vector3.zero)
        {
            previousPos = currentPos;
        }
        Direction = currentPos - previousPos;
        CurrentVelocity = Direction / Time.deltaTime;

        previousPos = currentPos;
    }

    private void UpdateFollowingObject()
    {
        //if (buttonRefrence.action.ReadValue<bool>() == true || UpdateFollowObject == true)

        if (UpdateFollowObject == true)
        {
            if (previousTrackingState == false)
            {
                followingObject.transform.position = Transform.position;
                previousTrackingState = true;
            }
            else
            {

                followingObject.transform.position += CurrentVelocity * Time.deltaTime;
            }

        }
    }




}

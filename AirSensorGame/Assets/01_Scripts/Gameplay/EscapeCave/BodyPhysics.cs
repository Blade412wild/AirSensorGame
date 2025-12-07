using UnityEngine;
using UnityEngine.InputSystem;

public class BodyPhysics : MonoBehaviour
{

    public bool UpdateFollowObject;

    public Vector3 CurrentVelocity { get; private set; }
    public Vector3 Direction { get; private set; }
    public Vector3 normalizedDirection => Direction.normalized;

    public float Speed => Direction.magnitude / Time.deltaTime;
    public Transform Transform { get; private set; }

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
        UpdatePhysics();
    }

    private void UpdatePhysics()
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
}

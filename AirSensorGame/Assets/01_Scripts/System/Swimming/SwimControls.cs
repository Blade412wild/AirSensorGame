using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimControls : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private InputActionReference leftControllerStickRef;
    [SerializeField] private InputActionReference rightControllerStickRef;

    [Space]
    [SerializeField] private InputActionReference WASDInput;
    [SerializeField] private InputActionReference SpaceInput;


    [Space]
    [Header("Parameters")]
    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private bool useKeboard = false;

    private Vector3 horizontalDir;
    private Vector3 verticalDir;
    private Vector3 currentDir;

    private Vector2 leftControllerStickInput;
    private Vector2 rightControllerStickInput;

    private float currentYaw;
    private float currentPitch;

    public Vector2 rightInput { get; private set; }
    public Vector2 leftInput { get; private set; }
    public Vector2Int rightInputInt => ConvertToVector2Int(rightInput);
    public Vector2Int leftInputInt => ConvertToVector2Int(leftInput);



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftControllerStickRef.action.Enable();
        rightControllerStickRef.action.Enable();

        WASDInput.action.Enable();
        SpaceInput.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (useKeboard)
        {
            rightInput = GetStickInput(rightControllerStickRef);
            leftInput = GetStickInput(leftControllerStickRef);
        }
        else
        {
            rightInput = GetStickInput(WASDInput);

            if (SpaceInput.action.IsPressed())
            {
                leftInput = Vector2.up;
            }
            else
            {
                leftInput = Vector2.zero;
            }
        }



        Vector2 horizontalInput = rightInput;
        horizontalInput.y = 0;

        Vector2 verticalInput = rightInput;
        verticalInput.x = 0;

        float yawRotationTurnAmount = GetTurnAmount(horizontalInput, horizontalInput.x, horizontalRotationSpeed);
        float pitchRotationTurnAmount = GetTurnAmount(verticalInput, verticalInput.y, verticalRotationSpeed);

        //Debug.Log("input : " + input + " | yaw amount : " + yawRotationTurnAmount + " | pitch amount : " + pitchRotationTurnAmount);

        TurnRigHorizontal(yawRotationTurnAmount);
        TurnRigVertical(pitchRotationTurnAmount);
        ApplyRotation();
    }

    private Vector2 GetStickInput(InputActionReference reference)
    {
        Vector2 input = reference.action.ReadValue<Vector2>();
        return input;
    }
    private float GetTurnAmount(Vector2 input, float axis, float speed)
    {
        float turnAmount = input.magnitude * (Mathf.Sign(axis) * speed * Time.deltaTime);
        return turnAmount;
    }

    private void TurnRigHorizontal(float turnAmount)
    {
        if (Mathf.Approximately(turnAmount, 0f))
            return;

        currentYaw += turnAmount;
        ApplyRotation();
    }

    private void TurnRigVertical(float turnAmount)
    {
        if (Mathf.Approximately(turnAmount, 0f))
            return;

        currentPitch -= turnAmount;
        currentPitch = Mathf.Clamp(currentPitch, -89f, 89f);

        ApplyRotation();
    }

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
    }


    public Vector2Int ConvertToVector2Int(Vector2 vector) {

        Vector2Int vector2Int = Vector2Int.zero;
        vector2Int.x = Mathf.RoundToInt(vector.x);
        vector2Int.y = Mathf.RoundToInt(vector.y);
        return vector2Int;

    }

}

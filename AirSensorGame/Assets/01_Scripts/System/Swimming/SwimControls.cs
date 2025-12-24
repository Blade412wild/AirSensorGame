using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwimControls : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private InputActionReference leftControllerStickRef;
    [SerializeField] private InputActionReference rightControllerStickRef;

    [Space]
    [Header("Parameters")]
    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float verticalRotationSpeed;

    private Vector3 horizontalDir;
    private Vector3 verticalDir;
    private Vector3 currentDir;

    private Vector2 leftControllerStickInput;
    private Vector2 rightControllerStickInput;

    private float currentYaw;
    private float currentPitch;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leftControllerStickRef.action.Enable();
        rightControllerStickRef.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = GetStickInput(rightControllerStickRef);

        Vector2 horizontalInput = input;
        horizontalInput.y = 0;

        Vector2 verticalInput = input;
        verticalInput.x = 0;

        float yawRotationTurnAmount = GetTurnAmount(horizontalInput, horizontalInput.x, horizontalRotationSpeed);
        float pitchRotationTurnAmount = GetTurnAmount(verticalInput, verticalInput.y, verticalRotationSpeed);

        Debug.Log("input : " + input + " | yaw amount : " + yawRotationTurnAmount + " | pitch amount : " + pitchRotationTurnAmount);

        TurnRigHorizontal(yawRotationTurnAmount);
        TurnRigVertical(pitchRotationTurnAmount);
        ApplyRotation();
    }

    private Vector2 GetStickInput(InputActionReference reference)
    {
        Vector2 input = reference.action.ReadValue<Vector2>();
        return input;
    }
    private float GetTurnAmount(Vector2 input, float axis , float speed)
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
}

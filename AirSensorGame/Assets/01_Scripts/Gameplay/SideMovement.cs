using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SideMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference wasdMovementAction;
    [SerializeField] private float speedScalar = 7; 
    private Transform transform;

    private void Start()
    {
        transform = GetComponent<Transform>();  
        wasdMovementAction.action.Enable();
    }

    private void Update()
    {
        Vector2 input = wasdMovementAction.action.ReadValue<Vector2>();
        transform.position = (input * speedScalar) * Time.deltaTime;
    }

}

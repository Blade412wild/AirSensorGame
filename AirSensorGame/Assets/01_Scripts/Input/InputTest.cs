using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    [SerializeField] private InputActionProperty TestActionValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TestActionValue.action.performed += OnSelectValuePerformed;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSelectValuePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("select : " + context) ;
    } 
}

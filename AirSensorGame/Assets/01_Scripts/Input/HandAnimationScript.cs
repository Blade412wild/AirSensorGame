using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    [Space]
    [SerializeField] private InputActionProperty triggerValue;
    [SerializeField] private InputActionProperty gripValue;
    [SerializeField] private bool OnUpdate;

    private void Start()
    {
        triggerValue.action.performed += OnHandTriggerPerformed;
        triggerValue.action.canceled += OnHandTriggerCanceled;

        gripValue.action.performed += OnGripTriggerperformed;
        gripValue.action.canceled += OnGripTriggerCanceled;

    }

    //private void Update()
    //{
    //    if (!OnUpdate) return;
    //    animator.SetFloat("Trigger", triggerValue.action.ReadValue<float>());
    //    animator.SetFloat("Grip", gripValue.action.ReadValue<float>());

    //}

    private void OnHandTriggerPerformed(InputAction.CallbackContext context)
    {
        if (OnUpdate) return;
        animator.SetFloat("Trigger", context.ReadValue<float>());
    }

    private void OnGripTriggerperformed(InputAction.CallbackContext context)
    {
        if (OnUpdate) return;
        animator.SetFloat("Grip", context.ReadValue<float>());
    }
    private void OnHandTriggerCanceled(InputAction.CallbackContext context)
    {
        if (OnUpdate) return;
        animator.SetFloat("Trigger", 0);
    }

    private void OnGripTriggerCanceled(InputAction.CallbackContext context)
    {
        if (OnUpdate) return;
        animator.SetFloat("Grip", 0);
    }



}

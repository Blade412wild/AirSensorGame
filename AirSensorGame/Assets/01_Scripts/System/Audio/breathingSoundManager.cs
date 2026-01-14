using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class breathingSoundManager : MonoBehaviour
{
    [SerializeField] private MicrocontrollerManager microcontrollerManager;
    [SerializeField] private HandAudioManager breathInManager;
    [SerializeField] private HandAudioManager breathOutManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private InputActionReference refrence;

    private bool soundIsBeingPlayed;
    private bool firstpress;
    private bool IsCalibrating;

    private void Start()
    {
        refrence.action.started += HandleButtonPressedEvent;
        refrence.action.canceled += HandleButtonReleasedEvent;
        microcontrollerManager.StartCallibrationEvent += () => IsCalibrating = true;
        microcontrollerManager.FinishedCallibrationEvent += () => IsCalibrating = false;


    }

    private void OnDestroy()
    {
        microcontrollerManager.StartCallibrationEvent -= () => IsCalibrating = true;
        microcontrollerManager.FinishedCallibrationEvent -= () => IsCalibrating = false;

    }

    private void HandleButtonPressedEvent(InputAction.CallbackContext context)
    {

        if (audioSource == null || IsCalibrating) return;
        AudioClip clip = breathInManager.GetAudioClip();
        audioSource.clip = clip;
        audioSource.Play();
        
        
    }

    private void HandleButtonReleasedEvent(InputAction.CallbackContext context)
    {
        if (audioSource == null || IsCalibrating) return;

        AudioClip clip = breathOutManager.GetAudioClip();
        audioSource.clip = clip;
        audioSource.Play();
    }



}

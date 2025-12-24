using System.Resources;
using UnityEditor;
using UnityEngine;

public class BreathCalibration : MonoBehaviour
{

    [SerializeField] private MicrocontrollerManager microcontrollerManager;
    [SerializeField] private BreathingDeviceData data;
    [SerializeField] private Transform leftLungTransform;
    [SerializeField] private Transform rightLungTransform;

    [Range(0, 100)]
    [SerializeField] private float progress;
    [SerializeField] private Vector3 min;
    [SerializeField] private Vector3 max;

    [SerializeField] private CalibrationState state = CalibrationState.Idle;
    [SerializeField] private bool isCalibrating;
    [SerializeField] private bool nexState;

    private float minFloatRange = 0;
    private float maxFloatRange = 100;
    private int counter = 0;

    private void Start()
    {
        leftLungTransform.localScale = min;
        rightLungTransform.localScale = min;
    }

    private void Update()
    {

        if (nexState)
        {
            nexState = false;
            GoToNextState();
        }
        if (!isCalibrating) return;
        SettingStates();
    }

    private void SettingStates()
    {
        switch (state)
        {
            case CalibrationState.Idle: InIdleState(); break;
            case CalibrationState.SettingMin: InSettingMinState(); break;
            case CalibrationState.SettingMax: InSettingMaxState(); break;
            case CalibrationState.Reviewing: InReviewingState(); break;
        }
    }

    


    private void InIdleState()
    {

    }

    private void InSettingMinState()
    {

    }

    private void InSettingMaxState()
    {

    }

    private void InReviewingState()
    {
        Vector3 newScale = ScaleUtils.ScaleFromFloatRange(progress, minFloatRange, maxFloatRange, min, max);
        leftLungTransform.localScale = newScale;
        rightLungTransform.localScale = newScale;
    }

    private void StartCalibration()
    {
        state = CalibrationState.SettingMin;
        isCalibrating = true;
    }

    private void GoToMinState()
    {
        state = CalibrationState.SettingMin;
    }

    private void GoToMaxState()
    {
        state = CalibrationState.SettingMax;
    }

    private void GoToReviewState()
    {
        state = CalibrationState.Reviewing;
    }

    private void GoToIdle()
    {
        state = CalibrationState.Idle;
    }
     
    private void RestartCalibration()
    {
        counter = 0;    
        StartCalibration();
        microcontrollerManager.SendMessage("4");

    }

    private void GoToNextState()
    {
        microcontrollerManager.SendMessage("2");
        counter++;

        switch (counter)
        {
            case 0: GoToIdle(); break;  
            case 1: GoToMinState(); break;
            case 2: GoToMaxState(); break;
            case 3: GoToReviewState(); break;
            case 4: FinishedCalibration(); break;
        }
    }

    private void FinishedCalibration()
    {
        counter = -1; // is because nextstate does ++ before switch
        isCalibrating = false;
        GoToNextState();
    }

}

public enum CalibrationState {Idle, SettingMin, SettingMax, Reviewing }

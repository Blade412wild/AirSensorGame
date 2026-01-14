using UnityEngine;

public class CalibrationEnviromentSwitch : MonoBehaviour 
{

    [SerializeField] private MicrocontrollerManager microcontrollerManager;

    [Space]
    [SerializeField] private GameObject swimmingEnviroment;
    [SerializeField] private GameObject callibrationEnviroment;

    [Space]
    [SerializeField] private GameObject PlayerMesh;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    private void Awake()
    {
        microcontrollerManager.StartCallibrationEvent += HandleStartCallibrationEvent;
        microcontrollerManager.FinishedCallibrationEvent += HandleFinishedCallibrationEvent;
    }

    private void OnDisable()
    {
        microcontrollerManager.StartCallibrationEvent -= HandleStartCallibrationEvent;
        microcontrollerManager.FinishedCallibrationEvent -= HandleFinishedCallibrationEvent;
    }

    private void HandleStartCallibrationEvent()
    {
        swimmingEnviroment.SetActive(false);
        callibrationEnviroment.SetActive(true);
        PlayerMesh.SetActive(false);
        leftHand.SetActive(false);
        rightHand.SetActive(false);
    }

    private void HandleFinishedCallibrationEvent()
    {
        swimmingEnviroment.SetActive(true);
        callibrationEnviroment.SetActive(false);
        PlayerMesh.SetActive(true);
        leftHand.SetActive(true);
        rightHand.SetActive(true);
    }




}

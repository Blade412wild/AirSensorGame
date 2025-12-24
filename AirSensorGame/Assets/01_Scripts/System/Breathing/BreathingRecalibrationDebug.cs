using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BreathingRecalibrationDebug : MonoBehaviour
{
    [SerializeField] private BreathingDeviceData data;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private InputActionReference leftRefrence;
    [SerializeField] private InputActionReference rightRefrence;

    private void Start()
    {

        leftRefrence.action.Enable();   
        rightRefrence.action.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        progressText.text = data.chestPostion.ToString();

        //Debug.Log("left : " + leftRefrence.action.ReadValue<float>());
        //Debug.Log("right : " + rightRefrence.action.ReadValue<float>());

    }
}

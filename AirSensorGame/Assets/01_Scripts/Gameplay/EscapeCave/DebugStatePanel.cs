using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering;

public class DebugStatePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text StateText;
    [SerializeField] private SchoolslagDetection schoolslagDetection;
    [SerializeField] private float lenght;
    [SerializeField] private float height;

    private void Start()
    {
        schoolslagDetection.ChangedMoveStateEvent += HandleChangeMovementStateEvent;
        //transform.position = Camera.main.transform.position;
    }

    private void Update()
    {
        UpdatePosAndRotation();
    }

    private void UpdatePosAndRotation()
    {
        Vector3 newPos = (Camera.main.transform.up * height) + (Camera.main.transform.forward * lenght);
        transform.position = Camera.main.transform.position + newPos;

        Vector3 lookDir = (transform.position - Camera.main.transform.position);
        

        //transform.LookAt(Camera.main.transform.position);
        transform.rotation = Quaternion.LookRotation(lookDir);
        //Quaternion.LookRotation()

    }

    private void HandleChangeMovementStateEvent(SchoolslagDetection.MoveState state)
    {
        StateText.text = state.ToString();
    }
}

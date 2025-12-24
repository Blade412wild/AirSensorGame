using Unity.XR.CoreUtils;
using UnityEngine;

public class settingYOffset : MonoBehaviour
{

   [SerializeField] private XROrigin origin;
    [SerializeField] private Transform cameraTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(origin == null || cameraTransform == null) return;
        origin.CameraYOffset = cameraTransform.localPosition.y * -1;
    }
}

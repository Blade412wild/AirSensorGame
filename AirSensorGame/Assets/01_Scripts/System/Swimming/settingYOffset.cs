using Unity.XR.CoreUtils;
using UnityEngine;

public class settingYOffset : MonoBehaviour
{

   [SerializeField] private XROrigin origin;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform cameraOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(origin == null || cameraTransform == null) return;
        Vector3 newOffset = cameraTransform.localPosition * -1;
        //newOffset.x = 0;
        //newOffset.z = 0;
        cameraOffset.localPosition = newOffset;
    }
}

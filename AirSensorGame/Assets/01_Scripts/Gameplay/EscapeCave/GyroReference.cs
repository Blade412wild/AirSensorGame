using UnityEngine;

public class GyroReference : MonoBehaviour
{
    [SerializeField] private bool update;

    [SerializeField] private Transform controller;
    [SerializeField] private Transform reference; // rotates only X/Y

    void Update()
    {
        if (!update) return;
        reference.position = controller.position;
        UpdateRotation();

    }

    private void UpdateRotation()
    {
        // Get controller orientation
        Quaternion q = controller.rotation;

        // Extract forward and up vectors from the controller
        Vector3 fwd = q * controller.forward;
        Vector3 up = q * controller.up;

        // Project both onto planes so Z-rotation is removed
        fwd.z = 0;  // Keep yaw
        up.z = 0;   // Keep pitch

        fwd.Normalize();
        up.Normalize();

        // Build a corrected rotation with no roll (Z)
        Quaternion noZ = Quaternion.LookRotation(fwd, up);

        // Apply it to the reference
        transform.rotation = noZ;
    }

    private void CalculateRotation()
    {
        // Get each transform's "up" direction
        Vector3 upA = controller.up;
        Vector3 upB = reference.up;

        // Project both vectors onto the XY plane (ignore Z tilt influence)
        upA.z = 0;
        upB.z = 0;

        upA.Normalize();
        upB.Normalize();

        // Calculate signed angle around Z
        float zRotation = Vector3.SignedAngle(upB, upA, Vector3.forward);

        Debug.Log("Z Rotation: " + zRotation);

    }
}

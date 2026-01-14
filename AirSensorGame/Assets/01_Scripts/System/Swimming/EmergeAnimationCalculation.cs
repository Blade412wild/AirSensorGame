using JetBrains.Annotations;
using UnityEngine;

public class EmergeAnimationCalculation : MonoBehaviour
{
    [SerializeField] private Transform upperPlaneTarget;
    [SerializeField] private Transform lowerPlaneTarget;
    [SerializeField] private Transform orientation;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform result;

    private Plane upperPlane;
    private Plane lowerPlane;


    private Vector3 predictionPoint;

    private void Start()
    {
        CreatePlanes();
    }

    public Vector3 GetEmergePoint(Vector3 dir)
    {
        if (CalculateCrossSection(dir)) return predictionPoint;
        return Vector3.zero;
    }

    public bool CalculateCrossSection(Vector3 dir)
    {
        // Ray starting at orientation position, pointing forward
        Ray ray = new Ray(orientation.position, orientation.forward);

        if (dir == Vector3.up)
        {
            if (CheckRay(upperPlane, ray)) return true;
            return false;

        }
        else if (dir == Vector3.down)
        {
            if (CheckRay(lowerPlane, ray)) return true;
            return false;
        }
        return false;

    }

    private void CreatePlanes()
    {
        upperPlane = new Plane(Vector3.up, upperPlaneTarget.position);
        lowerPlane = new Plane(Vector3.up, lowerPlaneTarget.position);
        // Plane at Y = targetPlan.position.y, facing up
    }

    private bool CheckRay(Plane plane, Ray ray)
    {
        if (plane.Raycast(ray, out float distance))
        {
            predictionPoint = ray.GetPoint(distance);
            return true;
        }
        predictionPoint = Vector3.zero;
        return false;
    }

}

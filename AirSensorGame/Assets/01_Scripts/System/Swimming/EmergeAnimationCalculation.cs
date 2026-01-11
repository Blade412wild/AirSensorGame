using JetBrains.Annotations;
using UnityEngine;

public class EmergeAnimationCalculation : MonoBehaviour
{
    [SerializeField] private Transform targetPlane;
    [SerializeField] private Transform orientation;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform result;


    private Vector3 predictionPoint;


    public Vector3 GetEmergePoint()
    {
        if (CalculateCrossSection(out predictionPoint)) return predictionPoint;
        return Vector3.zero;
    }

    public bool CalculateCrossSection(out Vector3 intersection)
    {
        // Plane at Y = targetPlan.position.y, facing up
        Plane plane = new Plane(Vector3.up, targetPlane.position);

        // Ray starting at orientation position, pointing forward
        Ray ray = new Ray(orientation.position, orientation.forward);

        if (plane.Raycast(ray, out float distance))
        {
            intersection = ray.GetPoint(distance);
            return true;
        }

        intersection = Vector3.zero;

        
        return false;
    }

}

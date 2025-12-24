using UnityEngine;

public  class ScaleUtils
{
    public static Vector3 ScaleFromFloatRange(
        float currentProgress,
        float floatMin,
        float floatMax,
        Vector3 vectorMin,
        Vector3 vectorMax)
    {
        // Normalize the float value to 0–1
        float t = Mathf.InverseLerp(floatMin, floatMax, currentProgress);

        // Interpolate between the two Vector3 values
        return Vector3.Lerp(vectorMin, vectorMax, t);
    }
}

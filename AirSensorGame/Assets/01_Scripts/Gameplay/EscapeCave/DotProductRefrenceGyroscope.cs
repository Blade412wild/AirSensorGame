using UnityEngine;

public class DotProductRefrenceGyroscope : MonoBehaviour
{

    [SerializeField] private Transform targetTransform;

    [SerializeField] private bool update;

    // Update is called once per frame
    void Update()
    {
        if (!update) return;

        transform.position = targetTransform.position;
        //Test3(targetTransform);
        WorkingBut180Flip();
    }

    private void Test3(Transform target)
    {
        Vector3 rawForward = target.forward;

        // project forward onto X–Y plane to remove Z tilt
        Vector3 flatForward = new Vector3(rawForward.x, rawForward.y, 0f);

        // if forward gets too vertical, fall back to a sideways up direction
        Vector3 worldUp = Vector3.up;
        if (Mathf.Abs(Vector3.Dot(flatForward.normalized, worldUp)) > 0.99f)
        {
            worldUp = Vector3.right; // any stable fallback axis
        }

        transform.rotation = Quaternion.LookRotation(flatForward, worldUp);
    }

    private void WorkingBut180Flip()
    {
        Vector3 fwd = targetTransform.forward;
        Vector3 up = Vector3.up;

        transform.rotation = Quaternion.LookRotation(fwd, up);
    }

    private void First()
    {
        Vector3 rotation = new Vector3(targetTransform.rotation.eulerAngles.x, targetTransform.rotation.eulerAngles.y, 0);
        transform.eulerAngles = rotation;   
    }

}

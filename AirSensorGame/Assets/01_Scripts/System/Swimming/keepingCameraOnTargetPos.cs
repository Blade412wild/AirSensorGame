using UnityEngine;

public class keepingCameraOnTargetPos : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] Transform cam;
    [SerializeField] Transform offset;
        Vector3 newOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        newOffset = target.position - cam.position;
        offset.position += newOffset;
    }
}

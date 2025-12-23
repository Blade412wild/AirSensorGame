using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = targetTransform.position;
        //this.transform.rotation = targetTransform.rotation;
        
    }
}

using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    [SerializeField] private BodyPhysics bodyPhysics;
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += cameraTrans.forward * (speed * Time.deltaTime);
    }
}

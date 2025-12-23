using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    public Vector3 moveDir;
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
        moveDir = cameraTrans.forward;
        transform.position += moveDir * (speed * Time.deltaTime);
    } 
}

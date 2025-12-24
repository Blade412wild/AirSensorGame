using UnityEngine;

public class BodyMovement : MonoBehaviour
{
    public Vector3 moveDir;
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float speed;


    // Update is called once per frame
    void Update()
    {
        moveDir = cameraTrans.forward;
        transform.position += moveDir * (speed * Time.deltaTime);
    } 
}

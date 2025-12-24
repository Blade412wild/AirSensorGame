using UnityEngine;

public class BodyRotation : MonoBehaviour
{
    public Vector3 moveDir;
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private bool useFixedUpdate;
    [SerializeField] private int iterations;
    //[SerializeField] private float speed;
    int counter = 0;


    private void Update()
    {
        if (useFixedUpdate) return;
        transform.rotation = cameraTrans.rotation;
    }
    private void FixedUpdate()
    {
        if(!useFixedUpdate) return;

        if(counter >= iterations)
        {
                transform.rotation = cameraTrans.rotation;
            counter = 0;
        }
        else
        {
            counter++;
        }
    }
}

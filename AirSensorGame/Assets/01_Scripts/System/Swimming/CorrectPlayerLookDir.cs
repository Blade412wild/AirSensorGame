
using UnityEngine;

public class CorrectPlayerLookDir : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform ownTransform;
    [SerializeField] private float maxAngle;
    [SerializeField] private float maxHorizontalAngle;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float lookSpeed;

    //private Vector3 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetForward = targetTransform.forward;
        Vector3 ownForward = ownTransform.forward;

        Vector3 targetVertical = targetForward;
        targetVertical.x = 0;
        //targetVertical.z = 0;

        Vector3 ownVertical = ownForward;
        ownVertical.x = 0;
        //ownVertical.z = 0;

        Vector3 targetHorizontal = targetForward;
        targetHorizontal.y = 0;
        //targetHorizontal.z = 0;

        Vector3 ownHorizontal = ownForward;
        ownHorizontal.y = 0;
        //ownHorizontal.z = 0;

        float horizontalAngle = Vector3.Angle(ownHorizontal, targetHorizontal);
        float verticalAngle = Vector3.Angle(ownVertical, targetVertical);
        float angle = Vector3.Angle(targetForward, ownForward);

        if(horizontalAngle >= maxHorizontalAngle)
        {
            //Vector3 newLookDir = Quaternion.AngleAxis(horizontalAngle, targetForward) * targetTransform.right;
        }

        if(verticalAngle >= maxVerticalAngle)
        {

        }

        if (angle >= maxAngle)
        {
            Quaternion rotation = Quaternion.Slerp(ownTransform.rotation, Quaternion.LookRotation(targetForward), lookSpeed * Time.deltaTime);
            ownTransform.rotation = rotation;
        }
        

        //Debug.Log("OwnF : " + ownForward + " | ownV : " + ownVertical + " | targetV : " + targetVertical);
        //Debug.Log("angle : " + angle + " | horizontal : " + horizontalAngle + " | vertical : " + verticalAngle);
    }
}

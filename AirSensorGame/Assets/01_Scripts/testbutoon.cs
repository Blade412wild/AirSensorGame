using UnityEngine;

public class testbutoon : MonoBehaviour
{

    [SerializeField] private Transform target;
    public void Test()
    {
        target.position += Vector3.up * 1; 
    }
}

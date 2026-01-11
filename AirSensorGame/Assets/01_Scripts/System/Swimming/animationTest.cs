using UnityEngine;

public class animationTest : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float speed;
    public bool update;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}

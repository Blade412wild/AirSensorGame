using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    public float speed;
    private Transform transform;

    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, speed * Time.deltaTime);
    }
}

using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    public float speed;
    private Transform transform;
    [SerializeField] private int[] numbers;
    [SerializeField] private UnityDictionary<int,string> times = new UnityDictionary<int, string>();

    private void Start()
    {
        transform = GetComponent<Transform>();
        Debug.Log(times[0]);
        Debug.Log(times[1]);
        Debug.Log(times[2]);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0, speed * Time.deltaTime);
    }
}

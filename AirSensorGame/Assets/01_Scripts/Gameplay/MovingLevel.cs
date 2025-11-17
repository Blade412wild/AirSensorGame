using UnityEngine;

public class MovingLevel : MonoBehaviour
{
    [SerializeField] private PrototypeGameManager gameManager;

    [SerializeField] private bool start;
    [SerializeField] private bool pauze;
    [SerializeField] private bool reset;

    [Space]
    [SerializeField] private float speed;

    private Transform transform;
    private Vector3 startPos;

    private Vector3 dir = Vector3.left;
    private bool mayMove;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform = GetComponent<Transform>();
        startPos = transform.position;
        gameManager.PlayerExitedFirstRoom += () => start = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            mayMove = true;
        }

        if (pauze)
        {
            pauze = false;
            mayMove = false; 
        }

        if (reset)
        {
            reset = false;
            transform.position = startPos;
        }


        if (mayMove)
        {
            Vector3 velocity = dir * speed;
            transform.position += velocity * Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        gameManager.PlayerExitedFirstRoom -= () => start = true;
    }
}

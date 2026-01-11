
using UnityEngine;

public class Sub_EmergeAnimation : MonoBehaviour
{
    [SerializeField] private float minDistance;
    private Transform playerTransform;
    private bool update;
    private Vector3 targetPos;
    private float speed;

    private void Update()
    {
        if (!update) return;
        playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPos, speed * Time.deltaTime);

        float distance = Vector3.Distance(playerTransform.position, targetPos);

        if (distance < minDistance)
        {
            playerTransform.position = targetPos;
            AnimationFinished();
        }
    }

    public void SetAnimation(Transform player, Vector3 targetPos, float speed)
    {
        playerTransform = player;
        this.targetPos = targetPos;
        this.speed = speed;

    }

    public void StartAnimation()
    {
        update = true;
    }

    private void AnimationFinished()
    {
        update = false;
    }

}

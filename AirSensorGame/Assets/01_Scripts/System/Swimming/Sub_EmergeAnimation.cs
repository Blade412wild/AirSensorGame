
using System;
using UnityEngine;

public class Sub_EmergeAnimation : MonoBehaviour
{

    public event Action<Vector3> AnimationFinishedEvent;

    [SerializeField] private float minDistance;
    [SerializeField] private Rigidbody rigidbody;
    private Transform playerTransform;
    private bool update;
    private Vector3 targetPos;
    private float speed;
    private Vector3 currentDir;

    private void Update()
    {
        if (!update) return;

        playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPos, speed * Time.deltaTime);

        float distance = Vector3.Distance(playerTransform.position, targetPos);
        //rigidbody.Move(targetPos, Quaternion.identity);
        //rigidbody.linearVelocity = Vector3.zero;

        if (distance < minDistance)
        {
            playerTransform.position = targetPos;
            AnimationFinished();
        }
    }

    public void SetAnimation(Transform player, Vector3 targetPos, float speed, Vector3 dir)
    {
        playerTransform = player;
        this.targetPos = targetPos;
        this.speed = speed;
        currentDir = dir;
        Debug.Log("targetpos : " + targetPos);
    }

    public void StartAnimation()
    {
        update = true;
    }

    private void AnimationFinished()
    {
        update = false;
        AnimationFinishedEvent?.Invoke(currentDir);
    }

}

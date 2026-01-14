using System.Runtime.CompilerServices;
using UnityEngine;

public class SwimmingMananger : MonoBehaviour
{
    [SerializeField] private Sub_EmegereDetection sub_EmegereDetection;
    [SerializeField] private EmergeAnimationCalculation emergePoint;
    [SerializeField] private Sub_EmergeAnimation animator;
    [SerializeField] private SwimmingPhysicsTest playerSwimPhysis;
    [SerializeField] private Collider waterTopCollider;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool mayUpdate;
    private bool inTransition = false;
    private bool inAnimation = false;

    private void Start()
    {
        sub_EmegereDetection.PlayerEmergeEvent += HandlePlayerEmergeEvent;
        sub_EmegereDetection.PlayerSubmergeEvent += HandlePlayerSubmergeEvent;
        animator.AnimationFinishedEvent += HandleAnimationEventFinished;
    }

    private void Update()
    {
        if (inAnimation)
        {

        }

        if (MayUpdateDetection())
        {
            sub_EmegereDetection.OnUpdate();
        }
    }

    private void HandlePlayerEmergeEvent()
    {
        Debug.Log("emerge");

        playerSwimPhysis.StopMovement();

        Vector3 dir = Vector3.up;
        Vector3 animationEndPoint = emergePoint.GetEmergePoint(dir);
        StartAnimation(animationEndPoint, dir, playerSwimPhysis.PlayerRidigBody.linearVelocity.magnitude);

    }

    private void HandlePlayerSubmergeEvent()
    {
        Debug.Log("Subemerge");

        Vector3 dir = Vector3.down;
        Vector3 animationEndPoint = emergePoint.GetEmergePoint(dir);
        StartAnimation(animationEndPoint, dir, 2);

    }

    private bool MayUpdateDetection()
    {
        if (mayUpdate && inAnimation == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void HandleAnimationEventFinished(Vector3 dir)
    {
        Debug.Log("Aniation Finished");

        inAnimation = false;
        waterTopCollider.enabled = true;

        if(dir == Vector3.up)
        {

        }
        else if(dir == Vector3.down)
        {
            playerSwimPhysis.ContinueMovement();
        }

    }

    private void StartAnimation(Vector3 animationEndPoint, Vector3 dir, float speed)
    {
        Debug.Log("Start Animation");
        inAnimation = true;
        waterTopCollider.enabled = false;
        animator.SetAnimation(playerTransform, animationEndPoint, speed, dir);
        playerSwimPhysis.PlayerRidigBody.linearVelocity = Vector3.zero;
        animator.StartAnimation();

    }

}

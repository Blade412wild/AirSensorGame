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
        inAnimation = true;
        Vector3 animationEndPoint = emergePoint.GetEmergePoint();
        waterTopCollider.enabled = false;
        animator.SetAnimation(playerTransform, animationEndPoint, playerSwimPhysis.PlayerRidigBody.linearVelocity.magnitude);
        animator.StartAnimation();

    }

    private void HandlePlayerSubmergeEvent()
    {
        Debug.Log("Subemerge");

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

}

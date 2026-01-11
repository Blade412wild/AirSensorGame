
using UnityEngine;
using System;

public class Sub_EmegereDetection : MonoBehaviour
{
    public event Action PlayerEmergeEvent;
    public event Action PlayerSubmergeEvent;

    [SerializeField] private EmergeArea emergeArea; 
    [SerializeField] private SwimControls swimControls;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform WaterlevelTransform;

    [Space]
    [Header("submerge Parameters")]
    [SerializeField] private float minSubmergeAngle;

    [Space]
    [Header("Emerge Parameters")]
    [SerializeField] private float minEmergeAngle;

    public bool IsUnderwater { get; private set; }

    private bool update;
    private bool inEmergeArea;

    private void Start()
    {
        emergeArea.TriggerEvent += (x) => { inEmergeArea = true; };
        emergeArea.TriggerExitedEvent += (x) => { inEmergeArea = false; };
        IsUnderwater = true;
    }

    public void OnUpdate()
    {


        if (inEmergeArea)
        {
            if (orientationAngleCorrectAndMoving(minEmergeAngle, Vector3.up))
            {
                PlayerEmergeEvent?.Invoke();
                IsUnderwater = false;
            }
        }

        if (!IsUnderwater)
        {
            if (orientationAngleCorrectAndMoving(minSubmergeAngle, Vector3.down))
            {
                PlayerSubmergeEvent?.Invoke();
                IsUnderwater = true;
            }
        }
    }

    private void OnDisable()
    {
        emergeArea.TriggerEvent -= (x) => { inEmergeArea = true; };
        emergeArea.TriggerExitedEvent -= (x) => { inEmergeArea = false; };
    }

    private bool orientationAngleCorrectAndMoving(float minAngle, Vector3 refrenceAngle)
    {
        float currentAngle = Vector3.Angle(orientation.forward, refrenceAngle);
        Debug.Log("currentAngle : " + currentAngle);
        if (currentAngle <= minAngle && swimControls.leftInputInt == Vector2.up) return true;
        return false;

    }

}

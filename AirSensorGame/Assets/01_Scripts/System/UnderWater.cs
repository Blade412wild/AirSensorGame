using System;
using UnityEngine;

public class UnderWater : MonoBehaviour
{
    public event Action PlayerEnteredWaterEvent;
    public event Action PlayerExitedWaterEvent;


    [SerializeField] private TriggerArea UnderwaterTrigger;
    [SerializeField] private Collider targetCollider;

    private void Start()
    {
        UnderwaterTrigger.SetTargetCollider(targetCollider);
        UnderwaterTrigger.TriggerEvent += PlayerEnteredUnderwater;
        UnderwaterTrigger.TriggerExitedEvent += PlayerExitedWater;
    }

    private void OnDisable()
    {
        UnderwaterTrigger.TriggerEvent -= PlayerEnteredUnderwater;
        UnderwaterTrigger.TriggerExitedEvent -= PlayerExitedWater;

    }

    private void PlayerEnteredUnderwater(TriggerArea area)
    {
        PlayerEnteredWaterEvent?.Invoke();

    }
    private void PlayerExitedWater(TriggerArea area)
    {
        PlayerEnteredWaterEvent?.Invoke();
    }


}

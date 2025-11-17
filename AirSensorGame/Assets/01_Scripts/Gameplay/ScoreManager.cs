using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<sbyte> ScoreChangeEvent;
    [SerializeField] private TriggerArea[] scoringTriggers;

    private sbyte scoreCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (TriggerArea trigger in scoringTriggers)
        {
            trigger.TriggerEvent += HandleScoring;
        }
    }
    private void HandleScoring(TriggerArea triggerArea)
    {
        scoreCount++;
        triggerArea.DestroyTriggerArea();
        ScoreChangeEvent?.Invoke(scoreCount);

    }
}

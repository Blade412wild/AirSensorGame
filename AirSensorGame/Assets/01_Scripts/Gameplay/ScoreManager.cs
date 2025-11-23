using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public event Action<sbyte> ScoreChangeEvent;

    public bool playsound;

    [SerializeField] private PrototypeGameManager gameManager;
    [SerializeField] private TriggerArea[] scoringTriggers;

    private AudioSource audioSource;
    private sbyte scoreCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager.ResetLevelEvent += HandleResetLevelEvent;

        audioSource = GetComponent<AudioSource>();

        foreach (TriggerArea trigger in scoringTriggers)
        {
            trigger.TriggerEvent += HandleScoring;
        }
    }
    private void HandleScoring(TriggerArea triggerArea)
    {
        float pitch = UnityEngine.Random.Range(1.00f, 1.10f);
        audioSource.pitch = pitch;

        audioSource.Play();
        scoreCount++;
        triggerArea.DestroyTriggerArea();
        triggerArea.TriggerEvent -= HandleScoring;
        ScoreChangeEvent?.Invoke(scoreCount);

    }

    private void HandleResetLevelEvent()
    {
        scoreCount= 0;
        //ScoreChangeEvent?.Invoke(0);
        foreach (TriggerArea trigger in scoringTriggers)
        {
            trigger.ResetTriggerArea();
            trigger.TriggerEvent -= HandleScoring;
            trigger.TriggerEvent += HandleScoring;
        }
    }

    private void Update()
    {
        if (playsound)
        {
            playsound = false;
            audioSource.Play();

        }
    }
    private void OnDisable()
    {

        gameManager.ResetLevelEvent -= HandleResetLevelEvent;
        foreach (TriggerArea trigger in scoringTriggers)
        {
            trigger.TriggerEvent -= HandleScoring;
        }

    }
}

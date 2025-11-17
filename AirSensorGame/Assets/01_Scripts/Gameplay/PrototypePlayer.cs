using System;
using UnityEngine;

public class PrototypePlayer : MonoBehaviour
{

    public event Action<sbyte> PlayerHit;
    public event Action PlayerDied;
    [SerializeField] private PrototypeGameManager gameManager;
    [SerializeField] private UnitHealth healthSystem;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSystem.unitHit += HandlePlayerHit;
        healthSystem.unitDied += HandlePlayerDeadth;
        gameManager.ResetLevelEvent += HandleLevelResetEvent;

    }

    private void HandleLevelResetEvent()
    {
        movement.ResetMovement();
        healthSystem.ResetHealthSystem();
    }

    private void HandlePlayerHit()
    {
        PlayerHit?.Invoke((sbyte)healthSystem.Health);
        animator.SetTrigger("PlayerHit");
    }

    private void HandlePlayerDeadth()
    {
        PlayerDied?.Invoke();
    }

    private void OnDisable()
    {
        healthSystem.unitHit -= HandlePlayerHit;
        healthSystem.unitDied -= HandlePlayerDeadth;
        gameManager.ResetLevelEvent -= HandleLevelResetEvent;
    }
}

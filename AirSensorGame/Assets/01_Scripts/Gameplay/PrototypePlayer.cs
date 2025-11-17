using System;
using UnityEngine;

public class PrototypePlayer : MonoBehaviour
{

    public event Action<sbyte> PlayerHit;
    public event Action PlayerDied;

    [SerializeField] private UnitHealth healthSystem;
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSystem.unitHit += HandlePlayerHit;
        healthSystem.unitDied += HandlePlayerDeadth;


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
}

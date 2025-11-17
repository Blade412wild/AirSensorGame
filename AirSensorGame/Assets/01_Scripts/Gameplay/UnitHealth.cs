using System;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IDamageable
{
    public event Action unitDied;
    public event Action unitHit;
    public int Health { get; set; }

    [SerializeField] private float TakeDamageCooldownTime;
    [SerializeField] private int StartingHealth;


    private bool mayTakeDamage;
    private float currentTime;

    private void Awake()
    {
        Health = StartingHealth;
    }

    private void Update() //TODo Create Timer manager class or TimerClass
    {
        if (!mayTakeDamage)
        {
            if (currentTime >= TakeDamageCooldownTime)
            {
                currentTime = 0;
                mayTakeDamage = true;
            }
            currentTime += Time.deltaTime;

        }
    }


    public void TakeDamage(int dmg)
    {
        if (!mayTakeDamage) return;

        Health -= dmg;
        if (Health <= 0)
        {
            unitDied?.Invoke();
        }
        {
            mayTakeDamage = false;
            unitHit?.Invoke();

        }

    }
}

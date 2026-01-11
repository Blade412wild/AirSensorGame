using System;
using UnityEngine;

public class TriggerAreaBase : MonoBehaviour
{
    public event Action<TriggerAreaBase> TriggerEvent;
    public event Action<TriggerAreaBase> TriggerExitedEvent;
    [field: SerializeField] protected Collider targetCollider;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(targetCollider == other)
        {
            TriggerEvent?.Invoke(this);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (targetCollider == other)
        {
            TriggerExitedEvent?.Invoke(this);
        }
    }
}

using System;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public event Action<TriggerArea> TriggerEvent;
    public event Action<TriggerArea> TriggerExitedEvent;
    public Collider targetCollider;
    private void OnTriggerEnter(Collider other)
    {
        if(other == targetCollider)
        {
            Debug.Log("target Entered");
            TriggerEvent?.Invoke(this);
        }

        if (other.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            Debug.Log("Player Entered");
            TriggerEvent?.Invoke(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == targetCollider)
        {
            Debug.Log("target Exited");
            TriggerExitedEvent?.Invoke(this);
        }
    }


    public void DestroyTriggerArea()
    {
        gameObject.SetActive(false);
    }

    public void ResetTriggerArea()
    {
        gameObject.SetActive(true);

    }
    public void SetTargetCollider(Collider targetCollider)
    {
        this.targetCollider = targetCollider;

    }
}

using System;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public event Action<TriggerArea> TriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            Debug.Log("Player Entered");
            TriggerEvent?.Invoke(this);
        }
    }

    public void DestroyTriggerArea()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class Obstakel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out IDamageable damgeable))
        {
            damgeable.TakeDamage(1);
        }
    }


}

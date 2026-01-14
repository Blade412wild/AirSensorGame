using UnityEngine;

public class EmergeArea : TriggerAreaBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        //Debug.Log(other.gameObject.name + " Entered collider");
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        //Debug.Log(other.gameObject.name + " Exited collider");

    }
}

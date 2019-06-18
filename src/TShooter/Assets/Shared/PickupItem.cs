using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter : " + other.name);
        if (other.tag != "Player")
            return;

        Pickup(other.transform);
    }

    public virtual void OnPickup(Transform item)
    {
        // nothing for now
    }

    void Pickup(Transform item)
    {
        OnPickup(item);
    }
}

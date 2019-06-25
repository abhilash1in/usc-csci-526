using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter : " + other.name);

        if (other.tag == "Player")
        {
            Pickup(other.transform);
        }
        if (other.tag != "Player")
        {
            GameObject potentialPlayerGO = FindParentWithTag(other.gameObject, "Player");
            if(potentialPlayerGO == null)
            {
                return;
            }
            Pickup(potentialPlayerGO.transform);
        }
    }

    public virtual void OnPickup(Transform item)
    {
        // nothing for now
    }

    void Pickup(Transform item)
    {
        OnPickup(item);
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }
}

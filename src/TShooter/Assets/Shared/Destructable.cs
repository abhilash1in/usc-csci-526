using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Collider))]
public class Destructable : MonoBehaviour
{
    [SerializeField] float hitPoints;
    public event System.Action OnDeath;
    public event System.Action OnDamageReceived;

    private float damagaeTaken;

    public float HitPointsRemaining
    {
        get
        {
            return hitPoints - damagaeTaken;
        }
    }

    public bool IsAlive
    {
        get
        {
            return HitPointsRemaining > 0;
        }
    }

    public virtual void Die()
    {
        if (IsAlive)
        {
            return;
        }

        if(OnDeath != null)
        {
            OnDeath();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        damagaeTaken += amount;

        if(OnDamageReceived != null)
        {
            OnDamageReceived();
        }

        if(HitPointsRemaining <= 0)
        {
            Die();
        }
    }

    public void Reset()
    {
        damagaeTaken = 0;
    }
}

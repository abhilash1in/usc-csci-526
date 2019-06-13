using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Destructable
{
    [SerializeField] float repawnTime;

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Respawner.Despawn(gameObject, repawnTime);
    }

    private void OnEnable()
    {
        Reset(); 
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        // print("Health Remaining: " + HitPointsRemaining);
    }
}

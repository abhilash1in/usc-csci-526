using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRespawnRandom : Destructable
{
    [SerializeField] float repawnTime;

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Respawner.DespawnRandom(gameObject, repawnTime);
        Reset();
    }

    private void OnEnable()
    {
        Reset();
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        print("Health Remaining: " + HitPointsRemaining);
    }
}

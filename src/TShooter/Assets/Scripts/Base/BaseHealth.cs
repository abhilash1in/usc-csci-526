using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : Destructable
{
    [SerializeField] float remainingHealthBeforeRespawn;
    public event System.Action OnRespawnBase;

    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

    private void Start()
    {
        remainingHealthBeforeRespawn = Random.Range(0, HitPointsRemaining);
        print("Base health start");
    }

    //public override void TakeDamage(float amount)
    //{
    //    base.TakeDamage(amount);
    //    print("Health Remaining: " + HitPointsRemaining);

    //    if (!IsAlive)
    //        return;

    //    remainingHealthBeforeRespawn -= amount;

    //    if(remainingHealthBeforeRespawn <= 0)
    //    {
    //        remainingHealthBeforeRespawn = Random.Range(0, HitPointsRemaining);
    //        if (OnRespawnBase != null)
    //            OnRespawnBase();
    //    }
    //}
}
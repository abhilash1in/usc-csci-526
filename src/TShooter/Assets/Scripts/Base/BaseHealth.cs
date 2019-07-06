using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : Destructable
{
    [SerializeField] float remainingHealthBeforeRespawn;
    Base mBase;

    public event System.Action OnRespawnBase;

    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        //remainingHealthBeforeRespawn = Random.Range(0, HitPointsRemaining);
        remainingHealthBeforeRespawn = 2;
        mBase = GetComponent<Base>();
        this.OnDamageReceived += Handle_OnDamageReceived;
    }

    void Handle_OnDamageReceived(float amount)
    {
        remainingHealthBeforeRespawn -= amount;
        if(remainingHealthBeforeRespawn <= 0)
        {
            //remainingHealthBeforeRespawn = Random.Range(0, HitPointsRemaining);
            remainingHealthBeforeRespawn = 2;
            if (isServer)
            {
                // respawn if base has not died
                mBase.SetSpawnPoint(mBase.BaseSpawner.GetNewSpawnPointTransform().name);
            }
        }
    }
}
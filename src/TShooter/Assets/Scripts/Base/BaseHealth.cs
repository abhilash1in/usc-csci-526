using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : Destructable
{
    [SerializeField] float remainingHealthBeforeRespawn;
    Base mBase;
    BaseSpawner spawner;

    public event System.Action OnRespawnBase;

    public override void Die()
    {
        base.Die();
        Destroy(this.gameObject);
    }

    private void Awake()
    {
        remainingHealthBeforeRespawn = Random.Range(0, HitPointsRemaining);
        mBase = GetComponent<Base>();
        spawner = GameObject.Find("BaseSpawner")?.GetComponent<BaseSpawner>();
        this.OnDamageReceived += Handle_OnDamageReceived;
        print("Base health start");
    }

    void Handle_OnDamageReceived(float amount)
    {
        remainingHealthBeforeRespawn -= amount;
        if(remainingHealthBeforeRespawn <= 0)
        {
            remainingHealthBeforeRespawn = Random.Range(0, HitPointsRemaining);
            // respawn if base has not died
            mBase.SetSpawnPoint(spawner.GetNewSpawnPointTransform().name);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseNetwork : NetworkBehaviour
{
    BaseHealth baseHealth;

    private void Awake()
    {
        baseHealth = GetComponent<BaseHealth>();
        baseHealth.OnDeath += Health_OnDeath;
        baseHealth.OnRespawnBase += BaseHealth_OnRespawnBase;

    }

    void Health_OnDeath()
    {
    }

    void BaseHealth_OnRespawnBase()
    {
    }
}

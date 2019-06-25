using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    [SerializeField] Destructable[] targets;

    int targetsDestroyedCounter;

    void Start()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].OnDeath += Handle_OnDeath;
        }
    }

    void Handle_OnDeath()
    {
        targetsDestroyedCounter++;

        if (targetsDestroyedCounter == targets.Length)
        {
            print("We won");
            GameManager.Instance.EventBus.RaiseEvent("OnAllEnemiesKilled");
        }
    }

}

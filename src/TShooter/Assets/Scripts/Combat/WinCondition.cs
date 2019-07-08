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
            // targets[i].OnDeath += Handle_OnDeath;
            targets[i].OnAdvancedDeath += Handle_OnAdvancedDeath;
        }
    }

    void Handle_OnAdvancedDeath(Destructable obj)
    {
        if(obj.TeamID == CustomNetworkBehviour.ETeamID.RED)
            GameManager.Instance.EventBus.RaiseEvent("OnBlueTeamWin");

        if (obj.TeamID == CustomNetworkBehviour.ETeamID.BLUE)
            GameManager.Instance.EventBus.RaiseEvent("OnRedTeamWin");
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

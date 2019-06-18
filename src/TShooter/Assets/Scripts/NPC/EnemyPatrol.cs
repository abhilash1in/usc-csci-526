using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyPlayer))]

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] WaypointController waypointController;
    [SerializeField] float waitTimeMin;
    [SerializeField] float waitTimeMax;

    PathFinder pathFinder;

    private EnemyPlayer m_EnemyPlayer;
    public EnemyPlayer EnemyPlayer
    {
        get
        {
            if (m_EnemyPlayer == null)
                m_EnemyPlayer = GetComponent<EnemyPlayer>();
            return m_EnemyPlayer;
        }
    }

    private void Start()
    {
        waypointController.SetNextWayPoint();
    }

    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.OnDestinationReached += PathFinder_OnDestinationReached;
        waypointController.OnWaypointChanged += WaypointController_OnWaypointChanged;
        EnemyPlayer.EnemyHealth.OnDeath += EnemyHealth_OnDeath;
        EnemyPlayer.OnTargetSelected += EnemyPlayer_OnTargetSelected;
    }

    void EnemyPlayer_OnTargetSelected(Player obj)
    {
        print("EnemyPlayer_OnTargetSelected");
        pathFinder.Agent.isStopped = true;
    }


    void EnemyHealth_OnDeath()
    {
        pathFinder.Agent.isStopped = true;
    }

    // TODO - Custom
    //private void OnDisable()
    //{
    //    EnemyHealth_OnDeath();
    //}

    //private void OnEnable()
    //{
    //    pathFinder.Agent.isStopped = false;
    //}


    void WaypointController_OnWaypointChanged(Waypoint waypoint)
    {
        pathFinder.SetTarget(waypoint.transform.position);
    }


    void PathFinder_OnDestinationReached()
    {
        // assume we are patrolling
        GameManager.Instance.Timer.Add(waypointController.SetNextWayPoint, 
            Random.Range(waitTimeMin, waitTimeMax));
    }

}

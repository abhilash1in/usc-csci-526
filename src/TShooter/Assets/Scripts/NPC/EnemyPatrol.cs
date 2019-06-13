using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] WaypointController waypointController;
    [SerializeField] float waitTimeMin;
    [SerializeField] float waitTimeMax;

    PathFinder pathFinder;

    private void Start()
    {
        waypointController.SetNextWayPoint();
    }

    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.OnDestinationReached += PathFinder_OnDestinationReached;
        waypointController.OnWaypointChanged += WaypointController_OnWaypointChanged;
    }

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

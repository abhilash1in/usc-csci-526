using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
public class EnemyPlayer : MonoBehaviour
{
    PathFinder pathFinder;
    [SerializeField] Scanner playerScanner;

    Player priorityTarget;
    List<Player> myTargets;

    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        playerScanner.OnScanReady += Scanner_OnScanReady;
        Scanner_OnScanReady();
    }

    void Scanner_OnScanReady()
    {
        if (priorityTarget != null)
            return;

        myTargets = playerScanner.ScanForTargets<Player>();

        if (myTargets.Count == 1)
            priorityTarget = myTargets[0];
        else
            SelectClosestTarget();

        if (priorityTarget != null)
            SetDestinationToPriorityTarget();
    }

    void SetDestinationToPriorityTarget()
    {
        pathFinder.SetTarget(priorityTarget.transform.position);
    }

    void SelectClosestTarget()
    {
        float closestTarget = playerScanner.ScanRange;
        foreach (var possibleTarget in myTargets)
        {
            if (Vector3.Distance(transform.position, possibleTarget.transform.position) < closestTarget)
                priorityTarget = possibleTarget;
        }
    }
}

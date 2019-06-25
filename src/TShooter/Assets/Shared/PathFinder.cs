using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFinder : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent Agent;

    [SerializeField] float distanceRemainingThreshold;

    bool m_DestinationReached;
    bool destinationReached
    {
        get
        {
            return m_DestinationReached;
        }
        set
        {
            m_DestinationReached = value;
            if (m_DestinationReached)
            {
                if (OnDestinationReached != null)
                    OnDestinationReached();
            }
        }
    }

    public event System.Action OnDestinationReached;


    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(Vector3 target)
    {
        destinationReached = false;
        Agent.SetDestination(target);
    }

    private void Update()
    {
        if (destinationReached || !Agent.hasPath)
            return;

        if (Agent.remainingDistance < distanceRemainingThreshold)
            destinationReached = true;
    }
}

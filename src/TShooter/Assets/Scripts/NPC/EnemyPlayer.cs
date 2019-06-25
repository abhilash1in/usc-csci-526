using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyState))]
public class EnemyPlayer : MonoBehaviour
{
    PathFinder pathFinder;
    [SerializeField] public Scanner playerScanner;
    [SerializeField] SwatSoldier settings;

    Player priorityTarget;
    List<Player> myTargets;

    public event System.Action<Player> OnTargetSelected;

    private EnemyHealth m_EnemyHealth;
    public EnemyHealth EnemyHealth
    {
        get
        {
            if (m_EnemyHealth == null)
                m_EnemyHealth = GetComponent<EnemyHealth>();
            return m_EnemyHealth;
        }
    }


    private EnemyState m_EnemyState;
    public EnemyState EnemyState
    {
        get
        {
            if (m_EnemyState == null)
                m_EnemyState = GetComponent<EnemyState>();
            return m_EnemyState;
        }
    }


    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.Agent.speed = settings.WalkSpeed;
        playerScanner.OnScanReady += Scanner_OnScanReady;
        Scanner_OnScanReady();

        EnemyHealth.OnDeath += EnemyHealth_OnDeath;
        EnemyState.OnModeChanged += EnemyState_OnModeChanged;
    }

    void EnemyState_OnModeChanged(EnemyState.EMode state)
    {
        pathFinder.Agent.speed = settings.WalkSpeed;

        if (state == EnemyState.EMode.AWARE)
            pathFinder.Agent.speed = settings.RunSpeed;
    }

    void CheckEaseWeapon()
    {
        // check if we can ease our weapon (stop aiming) 
        if (priorityTarget != null)
            return;

        this.EnemyState.CurrentMode = EnemyState.EMode.UNAWARE;
    }

    void CheckContinuePatrol()
    {
        // check if we can continue out patrol
        if(priorityTarget != null)
            return;

        pathFinder.Agent.isStopped = false;
    }

    internal void ClearTargetAndScan()
    {
        priorityTarget = null;
        GameManager.Instance.Timer.Add(CheckEaseWeapon, UnityEngine.Random.Range(3, 6));
        GameManager.Instance.Timer.Add(CheckContinuePatrol, UnityEngine.Random.Range(10, 15));
        Scanner_OnScanReady();
    }

    void EnemyHealth_OnDeath()
    {

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
        {
            if(OnTargetSelected != null)
            {
                this.EnemyState.CurrentMode = EnemyState.EMode.AWARE;
                OnTargetSelected(priorityTarget); 
            }
        }
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

    private void Update()
    {
        if (priorityTarget == null || !EnemyHealth.IsAlive)
            return;

        transform.LookAt(priorityTarget.transform.position);
    }
}

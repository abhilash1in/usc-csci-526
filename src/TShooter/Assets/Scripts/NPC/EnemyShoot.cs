using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Extensions;

[RequireComponent(typeof(EnemyPlayer))]
public class EnemyShoot : WeaponController
{
    [SerializeField] float shootingSpeed;
    [SerializeField] float burstDurationMin;
    [SerializeField] float burstDurationMax; 
    EnemyPlayer enemyPlayer;
    bool shouldFire;
    Vector3 myTarget;

    private void Start()
    {
        enemyPlayer = GetComponent<EnemyPlayer>();
        enemyPlayer.OnTargetSelected += EnemyPlayer_OnTargetSelected;
    }

    void EnemyPlayer_OnTargetSelected(Player target)
    {
        myTarget = target.transform.position;
        ActiveWeapon.SetAimPoint(target.transform.position);
        ActiveWeapon.AimTargetOffset = Vector3.up * 1.5f;
        StartBurst();
    }


    void CrouchState()
    {
        bool takeCover = Random.Range(0, 3) == 0;
        if (!takeCover)
            return;
        float distanceToTarget = Vector3.Distance(transform.position, myTarget);
        if(distanceToTarget > 15)
        {
            enemyPlayer.GetComponent<EnemyAnimation>().IsCrouching = true;
        }
    }

    void StartBurst()
    {
        if (!enemyPlayer.EnemyHealth.IsAlive || !canSeeTarget())
            return;

        CheckReload();
        CrouchState();
        shouldFire = true;

        GameManager.Instance.Timer.Add(EndBurst, Random.Range(burstDurationMin, burstDurationMax));
    }

    void EndBurst()
    {
        shouldFire = false;
        if (!enemyPlayer.EnemyHealth.IsAlive)
            return;
        CheckReload();
        CrouchState(); 

        if (canSeeTarget())
           GameManager.Instance.Timer.Add(StartBurst, shootingSpeed);
    }

    bool canSeeTarget()
    {
        if (!transform.IsInLineOfSight(myTarget, 90, enemyPlayer.playerScanner.mask, Vector3.up))
        {
            enemyPlayer.ClearTargetAndScan();
            return false;
        }
        return true;
    }

    void CheckReload()
    {
        if (ActiveWeapon.Reloader.IsReloading)
            return;

        if (ActiveWeapon.Reloader.RoundsRemainingInClip == 0)
        {
            CrouchState();
            ActiveWeapon.Reload();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldFire || !CanFire || !enemyPlayer.EnemyHealth.IsAlive)
            return;

        ActiveWeapon.Fire();
    }
}

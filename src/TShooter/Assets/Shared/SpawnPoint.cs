﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{   
    enum ESpawnPointType
    {
        BASE,
        PLAYER
    }

    [SerializeField] ESpawnPointType SpawnPointType;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.matrix = transform.localToWorldMatrix;
        if(SpawnPointType == ESpawnPointType.PLAYER)
            Gizmos.DrawWireCube(Vector3.zero + Vector3.up * 1, Vector3.one + Vector3.up * 1 );

        if (SpawnPointType == ESpawnPointType.BASE)
            Gizmos.DrawWireSphere(Vector3.zero + Vector3.up * 1, 4);
    }
}

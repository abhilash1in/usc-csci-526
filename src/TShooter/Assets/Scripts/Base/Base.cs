using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Base : CustomNetworkBehviour
{
    [SyncVar(hook = "UpdateSpawnPoint")] public string spawnPointName;

    [SerializeField]
    Material redMaterial;

    [SerializeField]
    Material blueMaterial;

    Renderer renderer;

    GameObject spawnPointContainer;

    private void Start()
    {
        UpdateMaterial();
        UpdateSpawnPoint(spawnPointName);
    }

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        spawnPointContainer = GameObject.Find("BaseSpawnPointContainer");
        //UpdateMaterial();
        print("Base start");
    }

    public void SetSpawnPoint(string spName)
    {
        spawnPointName = spName;
        UpdateSpawnPoint(spName);
    }

    public void UpdateSpawnPoint(string spName)
    {
        if(spawnPointContainer != null)
        {
            Transform spawnPointTransform = spawnPointContainer.transform.Find(spName);
            if (spawnPointTransform != null)
            {
                transform.SetParent(spawnPointTransform);
                transform.position = spawnPointTransform.position;
            }
        }
    }


    public void UpdateMaterial()
    {
        if(renderer != null)
        {
           if(TeamID == CustomNetworkBehviour.ETeamID.RED)
            {
                renderer.material = redMaterial;
            }

            if (TeamID == CustomNetworkBehviour.ETeamID.BLUE)
            {
                renderer.material = blueMaterial;
            }
        }
    }
}

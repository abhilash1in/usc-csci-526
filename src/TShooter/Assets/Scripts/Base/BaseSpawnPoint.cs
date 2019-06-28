using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawnPoint : MonoBehaviour
{
    public bool IsOccupied
    {
        get
        {
            return transform.Find("RedBase") != null || transform.Find("BlueBase") != null;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad coincontrol;
    private static bool GameManagerExists;
    // Use this for initialization
    void Start()
    {
        if (!GameManagerExists) //if GameManagerexcistst is not true --> this action will happen.
        {
            GameManagerExists = true;
            DontDestroyOnLoad(transform.gameObject); /// taken from this Tutorial https://youtu.be/x9lguwc0Pyk
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
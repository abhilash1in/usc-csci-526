using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public event System.Action<Player> OnLocalPlayerJoined;
    private GameObject gameObject;

    public bool IsPaused { get; set; }

    private bool m_IsNetworkGame;
    private bool m_IsNetworkGameChecked;

    private static GameManager m_Instance; 
    public static GameManager Instance
    {
        get
        {
            if(m_Instance == null)
            {
                m_Instance = new GameManager();
                m_Instance.gameObject = new GameObject("_gameManager");
                m_Instance.gameObject.AddComponent<InputController>();
                m_Instance.gameObject.AddComponent<Timer>();
                m_Instance.gameObject.AddComponent<Respawner>();
            }
            return m_Instance;
        }
    }

    public bool IsNetworkGame
    {
        get
        {
            if (!m_IsNetworkGameChecked)
            {
                m_IsNetworkGameChecked = true;
                m_IsNetworkGame = GameObject.Find("NetworkManager") != null;
                //m_IsNetworkGame = GameObject.Find("LobbyManager") != null;
                Debug.Log("m_IsNetworkGame: " + m_IsNetworkGame);
            }
            return m_IsNetworkGame;
        }
    }

    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if(m_InputController == null)
            {
                m_InputController = gameObject.GetComponent<InputController>();
            }

            return m_InputController;
        }
    }

    private Timer m_Timer;
    public Timer Timer
    {
        get
        {
            if (m_Timer == null)
            {
                m_Timer = gameObject.GetComponent<Timer>();
            }
            return m_Timer;
        }
    }


    private EventBus m_EventBus;
    public EventBus EventBus
    {
        get
        {
            if (m_EventBus == null)
            {
                m_EventBus = new EventBus();
            }
            return m_EventBus;
        }
    }


    private Respawner m_Respawner;
    public Respawner Respawner
    {
        get
        {
            if(m_Respawner == null)
            {
                m_Respawner = gameObject.GetComponent<Respawner>();
            }
            return m_Respawner;
        }
    }

    private Player m_LocalPLayer;
    public Player LocalPLayer
    {
        get
        {
            return m_LocalPLayer;
        }
        set
        {
            m_LocalPLayer = value;
            if(OnLocalPlayerJoined != null)
            {
                OnLocalPlayerJoined(m_LocalPLayer);
            }
        }
    }

}
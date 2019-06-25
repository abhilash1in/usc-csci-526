using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerHealth))]

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
        public bool LockMouse;
    } 

    public SwatSoldier Settings;

    [SerializeField] MouseInput MouseControl;


    public PlayerAim playerAim;
    public bool IsLocalPlayer;

    private InputController.InputState m_InputState;
    public InputController.InputState InputState
    {
        get
        {
            if (m_InputState == null)
                m_InputState = GameManager.Instance.InputController.State;
            return m_InputState;
        }
    }


    private PlayerShoot m_PlayerShoot;
    public PlayerShoot PlayerShoot
    {
        get
        {
            if (m_PlayerShoot == null)
                m_PlayerShoot = GetComponent<PlayerShoot>();
            return m_PlayerShoot;
        }
    }

    private WeaponController m_WeaponController;
    public WeaponController WeaponController
    {
        get
        {
            if (m_WeaponController == null)
                m_WeaponController = GetComponent<WeaponController>();
            return m_WeaponController;
        }
    }

    private PlayerState m_PlayerState;
    public PlayerState PlayerState
    {
        get
        {
            if (m_PlayerState == null)
                m_PlayerState = GetComponentInChildren<PlayerState>();
            return m_PlayerState;
        }
    }

    private PlayerNetwork m_PlayerNetwork;
    public PlayerNetwork PlayerNetwork
    {
        get
        {
            if (m_PlayerNetwork == null)
                m_PlayerNetwork = GetComponent<PlayerNetwork>();
            return m_PlayerNetwork;
        }
    }

    private PlayerHealth m_PlayerHealth;
    public PlayerHealth PlayerHealth
    {
        get
        {
            if (m_PlayerHealth == null)
                m_PlayerHealth = GetComponentInChildren<PlayerHealth>();
            return m_PlayerHealth;
        }
    }


    InputController playerInput;
    Vector2 mouseInput;


    // Start is called before the first frame update
    void Awake()
    {
        if (!GameManager.Instance.IsNetworkGame)
        {
            print("Not a network game");
            SetAsLocalPlayer();
        }
    }

    public void SetInputState(InputController.InputState state)
    {
        m_InputState = state;
    }

    public void SetAsLocalPlayer()
    {
        IsLocalPlayer = true;
        playerInput = GameManager.Instance.InputController;
        GameManager.Instance.LocalPLayer = this;

        if (MouseControl.LockMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerHealth.IsAlive || GameManager.Instance.IsPaused)
            return;

        if (IsLocalPlayer)
            LookAround();

    }

    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);
        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);
    }
   
}

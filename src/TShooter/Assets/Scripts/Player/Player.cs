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

    [SerializeField] SwatSoldier settings;

    [SerializeField] MouseInput MouseControl;
    [SerializeField] AudioController footsteps;
    [SerializeField] float minimumMoveThreshold;

    public PlayerAim playerAim;

    Vector3 previousPosition;

    private CharacterController m_MoveController; 
    public CharacterController MoveController
    {
        get
        {
            if(m_MoveController == null)
            {
                m_MoveController = GetComponent<CharacterController>();
            }
            return m_MoveController;
        }
    }

    private PlayerHealth m_PlayerHealth;
    public PlayerHealth PlayerHealth
    {
        get
        {
            if (m_PlayerHealth == null)
            {
                m_PlayerHealth = GetComponent<PlayerHealth>();
            }
            return m_PlayerHealth;
        }
    }

    private PlayerShoot m_PlayerShoot;
    public PlayerShoot PlayerShoot
    {
        get
        {
            if (m_PlayerShoot == null)
            {
                m_PlayerShoot = GetComponent<PlayerShoot>();
            }
            return m_PlayerShoot;
        }
    }

    //private Crosshair m_Crosshair;
    //private Crosshair Crosshair
    //{
    //    get
    //    {
    //        if(m_Crosshair == null)
    //        {
    //            m_Crosshair = GetComponentInChildren<Crosshair>();
    //        }
    //        return m_Crosshair;
    //    }
    //}

    private PlayerState m_PlayerState;
    public PlayerState PlayerState
    {
        get
        {
            if (m_PlayerState == null)
            {
                m_PlayerState = GetComponentInChildren<PlayerState>();
            }
            return m_PlayerState;
        }
    }


    InputController playerInput;
    Vector2 mouseInput;


    // Start is called before the first frame update
    void Awake()
    {
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
        if (!PlayerHealth.IsAlive)
            return;
        Move();
        LookAround();
    }

    private void Move()
    {
        float moveSpeed = settings.RunSpeed;

        if (playerInput.IsWalking)
            moveSpeed = settings.WalkSpeed;

        if (playerInput.IsSprinting)
            moveSpeed = settings.SprintSpeed;

        if (playerInput.IsCrouching)
            moveSpeed = settings.CrouchSpeed;

        if (PlayerState.MoveState == PlayerState.EMoveState.COVER)
            moveSpeed = settings.WalkSpeed;  


        Vector2 direction = new Vector2(playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);
        MoveController.SimpleMove(transform.forward * direction.x + transform.right * direction.y);

        // Commenting out since sound is now handled 
        // in PlayerSoundBridge.cs using Animation events

        //if(Vector3.Distance(transform.position, previousPosition) > minimumMoveThreshold)
        //{
        //    footsteps.Play(); 
        //}
        //previousPosition = transform.position;
    }

    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        // Crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);
    }
   
}

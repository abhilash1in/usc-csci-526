using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    InputController.InputState playerInput;

    [SerializeField] AudioController footsteps;
    [SerializeField] float minimumMoveThreshold;
    Vector3 previousPosition;

    private Player m_Player;
    public Player Player
    {
        get
        {
            if (m_Player == null)
                m_Player = GetComponent<Player>();
            return m_Player;
        }
    }

    private CharacterController m_MoveController;
    public CharacterController MoveController
    {
        get
        {
            if (m_MoveController == null)
                m_MoveController = GetComponent<CharacterController>();
            return m_MoveController;
        }
    }

    private void Awake()
    {
        playerInput = GameManager.Instance.InputController.State;
    }

    public void SetInputControllerState(InputController.InputState state)
    {
        playerInput = state;
    }

    private void Move()
    {
        if (playerInput == null)
        {
            playerInput = GameManager.Instance.InputController.State;
            if (playerInput == null)
                return;
        }
        Move(playerInput.Horizontal, playerInput.Vertical);
    }

    public void Move(float horizontal, float vertical)
    {
        float moveSpeed = Player.Settings.RunSpeed;

        if (playerInput.IsWalking)
            moveSpeed = Player.Settings.WalkSpeed;

        if (playerInput.IsSprinting)
            moveSpeed = Player.Settings.SprintSpeed;

        if (playerInput.IsCrouching)
            moveSpeed = Player.Settings.CrouchSpeed;

        if (Player.PlayerState.MoveState == PlayerState.EMoveState.COVER)
            moveSpeed = Player.Settings.WalkSpeed;


        Vector2 direction = new Vector2(vertical * moveSpeed, horizontal * moveSpeed);
        MoveController.SimpleMove(transform.forward * direction.x + transform.right * direction.y);


        // Commenting out since sound is now handled 
        // in PlayerSoundBridge.cs using Animation events

        //if(Vector3.Distance(transform.position, previousPosition) > minimumMoveThreshold)
        //{
        //    footsteps.Play(); 
        //}
        //previousPosition = transform.position;
    }

    void Update()
    {
        if (!Player.IsLocalPlayer)
            return;

        if (!Player.PlayerHealth.IsAlive || GameManager.Instance.IsPaused)
            return;

        if (!GameManager.Instance.IsNetworkGame)
            Move();
    }
}

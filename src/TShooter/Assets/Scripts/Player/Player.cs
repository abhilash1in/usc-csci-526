  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerState))]
public class Player : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 10;

    [SyncVar]
    private float currentHealth;

    [SyncVar]
    private bool _isFrozen = false;

    public static int counter = 0;

    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
        public bool LockMouse;
    }

    [SerializeField] public float runSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float sprintSpeed;

    [SerializeField] MouseInput MouseControl;
    [SerializeField] AudioController footsteps;
    [SerializeField] float minimumMoveThreshold;

    public PlayerAim playerAim;
    public bool IsLocalPlayer;
   
    Vector3 previousPosition;

    public bool isFrozen
    {
        get { return _isFrozen; }
        protected set { _isFrozen = value; }
    }

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


    private InputController.InputState m_InputState;
    public InputController.InputState InputState
    {
        get
        {
            if (m_InputState == null)
            {
                m_InputState = GameManager.Instance.InputController.State;
            }
            return m_InputState;
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


    public InputController.InputState playerInput;

    Vector2 mouseInput;


    // Start is called before the first frame update
    void Awake()
    {
        if(!GameManager.Instance.isNetworkGame)
        {
            SetAsLocalPlayer();
        }
    }

    public void setInputState(InputController.InputState state)
    {
        m_InputState = state;
    }

    public void SetAsLocalPlayer()
    {
        IsLocalPlayer = true;
        playerInput = GameManager.Instance.InputController.State;
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
        if (IsLocalPlayer)
        {
            if (!GameManager.Instance.isNetworkGame)
                Move();
            LookAround();
        }
    }

    public void Move(float Horizontal, float Vertical)
    {
        float moveSpeed = runSpeed;

        if (playerInput.IsWalking)
            moveSpeed = walkSpeed;

        if (playerInput.IsSprinting)
            moveSpeed = sprintSpeed;

        if (playerInput.IsCrouching)
            moveSpeed = crouchSpeed;


        Vector2 direction = new Vector2(Vertical * moveSpeed, Horizontal * moveSpeed);
        MoveController.Move(transform.forward * direction.x * 0.02f + transform.right * direction.y * 0.02f);
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

        // Commenting out since sound is now handled 
        // in PlayerSoundBridge.cs using Animation events

        //if(Vector3.Distance(transform.position, previousPosition) > minimumMoveThreshold)
        //{
        //    footsteps.Play(); 
        //}
        //previousPosition = transform.position;
    }

    public void SetInputController(InputController.InputState State)
    {
        playerInput = State;
    }

    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, GameManager.Instance.InputController.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, GameManager.Instance.InputController.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        // Crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);
    }

    // Networking Code

    public void PlayerSetup()
    {
        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetUpPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetUpPlayerOnAllClients()
    {
        setDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage(float _amount)
    {
        if (isFrozen)
            return;

        currentHealth -= _amount;
        Freeze();

        Debug.Log("Current Health" + currentHealth);
    }

    private void Freeze()
    {
        isFrozen = true;

        //DISABLE COMPONENTS

        GetComponent<PlayerShoot>().enabled = false;

        //CALL RESPAWN METHOD

        StartCoroutine(Reset());

        IEnumerator Reset()
        {
            yield return new WaitForSeconds(3f);

            isFrozen = false;
            GetComponent<PlayerShoot>().enabled = true;
        }

    }

    private void setDefaults()
    {
        isFrozen = false;
        currentHealth = maxHealth;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    [System.Serializable]
    public class InputState
    {
        public float Vertical;
        public float Horizontal;
        public bool Fire1;
        public bool Fire2;
        public bool Reload;
        public bool IsWalking;
        public bool IsSprinting;
        public bool IsCrouching;
        public bool CoverToggle;
        public bool IsInCover;
        public float AimAngle;
        public bool IsAiming;
    }

    public float Vertical { get { return State.Vertical; } }
    public float Horizontal { get { return State.Horizontal; } }
    public bool Fire1 { get { return State.Fire1; } }
    public bool Fire2 { get { return State.Fire2; } }
    public bool Reload { get { return State.Reload; } }
    public bool IsWalking { get { return State.IsWalking; } }
    public bool IsSprinting { get { return State.IsSprinting; } }
    public bool IsCrouching { get { return State.IsCrouching; } }
    public bool IsInCover { get { return State.IsInCover; } }
    public bool CoverToggle { get { return State.CoverToggle; } }
    public float AimAngle { get { return State.AimAngle; } }
    public bool IsAiming { get { return State.IsAiming; } }


    public Vector2 MouseInput;
    public bool Escape;
    public bool MouseWheelUp;
    public bool MouseWheelDown;


    public InputState State;


    private FixedJoystick leftJoystick;
    private FixedJoystick rightJoystick;

    void Awake()
    {
        GameObject LeftJoystickGO = GameObject.Find("LeftJoystick");
        GameObject RightJoystickGO = GameObject.Find("RightJoystick");
        if(LeftJoystickGO != null && RightJoystickGO != null)
        {
            leftJoystick = LeftJoystickGO.GetComponent<FixedJoystick>();
            rightJoystick = RightJoystickGO.GetComponent<FixedJoystick>();
        }
    }

    private void Start()
    {
        State = new InputState();
    }


    // Update is called once per frame
    void Update()
    {
        State.Vertical = Input.GetAxis("Vertical");
        State.Horizontal = Input.GetAxis("Horizontal");
        State.Fire1 = Input.GetButton("Fire1");
        State.Fire2 = Input.GetButton("Fire2");
        State.Reload = Input.GetKey(KeyCode.R);
        State.IsWalking = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftCommand);
        State.IsSprinting = Input.GetKey(KeyCode.LeftShift);
        State.IsCrouching = Input.GetKey(KeyCode.C);
        State.CoverToggle = Input.GetKeyDown(KeyCode.F);

        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Escape = Input.GetKey(KeyCode.Escape);
        MouseWheelUp = Input.GetAxis("Mouse ScrollWheel") > 0;
        MouseWheelDown = Input.GetAxis("Mouse ScrollWheel") < 0;


        if (IsJoystickEnabled(leftJoystick))
        {
            print("IsJoystickEnabled left");
            State.Vertical = leftJoystick.Vertical;
            State.Horizontal = leftJoystick.Horizontal;
        }

        if (IsJoystickEnabled(rightJoystick))
        {
            print("IsJoystickEnabled right");
            MouseInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);
            State.Fire1 = SimpleInput.GetButton("Fire");
            State.Fire2 = SimpleInput.GetButton("Aim");
        }
    }

    bool IsJoystickEnabled(FixedJoystick joystick)
    {
        if (joystick != null && joystick.enabled)
            return true;
        return false;
    }
}

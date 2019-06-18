using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour
{
    public float Vertical; 
    public float Horizontal;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Fire2;
    public bool Reload;
    public bool IsWalking;
    public bool IsSprinting;
    public bool IsCrouching;
    public bool CoverToggle;
    public bool MouseWheelUp;
    public bool MouseWheelDown;
    
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


    // Update is called once per frame
    void Update()
    {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("Fire2");
        Reload = Input.GetKey(KeyCode.R);
        IsWalking = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftCommand);
        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        IsCrouching = Input.GetKey(KeyCode.C);
        CoverToggle = Input.GetKeyDown(KeyCode.F);
        MouseWheelUp = Input.GetAxis("Mouse ScrollWheel") > 0;
        MouseWheelDown = Input.GetAxis("Mouse ScrollWheel") < 0;
        
        if(IsJoystickEnabled(leftJoystick))
        {
            Vertical += leftJoystick.Vertical;
            Horizontal += leftJoystick.Horizontal;
        }

        if (IsJoystickEnabled(rightJoystick))
        {
            MouseInput = new Vector2(rightJoystick.Horizontal, rightJoystick.Vertical);
            Fire1 = SimpleInput.GetButton("Fire");
            Fire2 = SimpleInput.GetButton("Aim");
        }
    }

    bool IsJoystickEnabled(FixedJoystick joystick)
    {
        if (joystick != null && joystick.enabled)
            return true;
        return false;
    }
}

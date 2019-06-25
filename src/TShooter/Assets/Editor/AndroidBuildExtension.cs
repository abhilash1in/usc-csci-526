using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;



public class BuildExtension
{
    static FixedJoystick leftJoystick, rightJoystick;

    private static void GetJoysticks()
    {
        leftJoystick = GameObject.Find("LeftJoystick").GetComponent<FixedJoystick>();
        rightJoystick = GameObject.Find("RightJoystick").GetComponent<FixedJoystick>();
    }

    private static void SetJoysticksEnabled(bool state)
    {
        GetJoysticks();
        if (leftJoystick != null)
            leftJoystick.enabled = state;

        if (rightJoystick != null)
            rightJoystick.enabled = state;
    }

    [MenuItem("MyTools/Android Build With Postprocess")]
    public static void BuildGame()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Scenes/MainScene.unity"};
        EnableJoysticks();
        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/game.apk", BuildTarget.Android, BuildOptions.AutoRunPlayer);
    }


    [MenuItem("MyTools/Enable Joysticks")]
    public static void EnableJoysticks()
    {
        SetJoysticksEnabled(true);
    }

    [MenuItem("MyTools/Disable Joysticks")]
    public static void DisableJoysticks()
    {
        SetJoysticksEnabled(false);
    }

}

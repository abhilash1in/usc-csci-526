using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;



public class AndroidBuildExtension
{
    [MenuItem("MyTools/Android Build With Postprocess")]
    public static void BuildGame()
    {
        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Scenes/MainScene.unity"};
        EnableJoysticks();
        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.apk", BuildTarget.Android, BuildOptions.None);
    }


    [MenuItem("MyTools/Enable Joysticks")]
    public static void EnableJoysticks()
    {
        GameObject.Find("LeftJoystick").GetComponent<FixedJoystick>().enabled = true;
        GameObject.Find("RightJoystick").GetComponent<FixedJoystick>().enabled = true;
    }

}

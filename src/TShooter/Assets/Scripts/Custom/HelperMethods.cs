using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods
{
    public static void ShowMessage(string message)
    {
        SSTools.ShowMessage(message, SSTools.Position.bottom, 2);
    }
}

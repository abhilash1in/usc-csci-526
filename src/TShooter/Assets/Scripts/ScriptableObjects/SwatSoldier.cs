using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwatSoldier", menuName = "Data/SwatSoldier")]
public class SwatSoldier : ScriptableObject
{
    public float RunSpeed;
    public float TurnSpeed;
    public float WalkSpeed;
    public float CrouchSpeed;
    public float SprintSpeed;
}

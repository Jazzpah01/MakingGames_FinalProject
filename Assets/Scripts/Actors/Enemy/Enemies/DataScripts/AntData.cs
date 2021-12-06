using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Actor/Enemy/Ant")]
public class AntData : ActorData
{
    public float range;
    public PathMove pathMove;
    public InstantAttack instantAttack;
}
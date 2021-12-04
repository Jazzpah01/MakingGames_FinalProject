using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Actor/Enemy/StaticTower")]
public class StaticTowerData : ActorData
{
    public float damage;
    public float range;
    public float cooldown;
}
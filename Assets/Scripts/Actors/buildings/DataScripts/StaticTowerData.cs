using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Actor/Building/StaticTower")]
public class StaticTowerData : BuildableData
{
    [Header("Static Tower")]
    public float damage;
    public float range;
    public float cooldown;
    public float projectileSpeed;
    public GameObject projectilePrefab;
}
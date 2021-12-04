using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Actor/Building/Catapult")]
public class CatapultData : BuildableData
{
    public float damage;
    public float projectileSpeed;
    public float cooldown;
    public GameObject projectilePrefab;
}
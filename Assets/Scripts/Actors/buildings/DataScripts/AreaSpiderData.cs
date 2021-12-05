using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Actor/Building/AreaSpider")]
public class AreaSpiderData : BuildableData
{
    [Header("Area Spider")]
    [Tooltip("The enemy speed will be multiplied with slow. Ex: 'slow = 0.5' means it halfes enemy speed.")]
    [Range(0.1f, 1)]
    public float speedModifyer = 1;
    [Range(0.1f, 1)]
    public float damageModifyer = 1;
    public float webRadius;
    public float damage;
    public float attackCooldown;
    public float attackDelay;
}
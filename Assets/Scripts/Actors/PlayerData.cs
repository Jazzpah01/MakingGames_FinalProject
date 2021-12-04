using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Actor/Player")]
public class PlayerData : ActorData
{
    [Header("Dash")]
    public float dashCooldown;
    public float dashLength;
    public float dashSpeed;

    [Header("Primary Attack")]
    public float primaryAttackDamage;
    public float primaryAttackCooldown;
    public float primaryAttackRange;
    public float primaryAttackDashLength;
    public float primaryAttackDashSpeed;
    public float primaryDelay;
    public GameObject primaryHitParticlePrefab;

    [Header("Secondary Attack")]
    public float secondaryAttackDamage;
    public float secondaryAttackCooldown;
    public float secondaryAttackRange;
    public float SecondaryAttackDashLength;
    public float SecondaryAttackDashSpeed;
    public float secondaryDelay;
    public GameObject secondaryHitParticlePrefab;
}
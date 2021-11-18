using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Ability
{
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public GameObject attackEffect;
    public CollisionObserver attackObserver;
    private float attackTime;
}
public class PlayerCombat : MonoBehaviour
{
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public GameObject attackEffect;
    public CollisionObserver attackObserver;
    private float attackTime;

    public float AOEAttackDamage;
    public float AOEAttackCooldown;
    public float AOEAttackRange;
    public GameObject AOEAttackEffect;
    public CollisionObserver AOEObserver;
    private float AOEAttackTime;


    internal void UpdateCooldowns()
    {
        //update cooldowns
        attackTime -= Time.deltaTime;
        AOEAttackTime -= Time.deltaTime;
    }
    //normal attack
    public void NormalAttack(RaycastHit hit)
    {
        if (attackTime <= 0 && attackObserver.Stay.Contains(hit.collider))
        {
            IActor actor = hit.transform.GetComponent<IActor>();

            if (actor != null && actor.type == ActorType.Enemy)
            {
                attackTime = attackCooldown;
                actor.Health -= attackDamage;
                Instantiate(attackEffect).transform.position = hit.point;
            }
        }
    }

    //area attack
    public void AOEAttack()
    {
        if (AOEAttackTime < 0)
        {
            Collider[] collisions = AOEObserver.Stay.ToArray();
            AOEAttackTime = AOEAttackCooldown;

            Instantiate(AOEAttackEffect).transform.position = transform.position;

            for (int i = 0; i < collisions.Length; i++)
            {
                IActor actor = collisions[i].GetComponent<IActor>();

                if (actor == null || actor.type != ActorType.Enemy)
                    continue;

                actor.Health -= AOEAttackDamage;

                if (actor.gameObject == null)
                {
                    i--;
                }
            }
        }
    }
    //TODO: damage player, normal attack, enemy health ui, player health ui

}

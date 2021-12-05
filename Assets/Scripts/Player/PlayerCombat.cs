using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public CollisionObserver primaryAttackObserver;
    public CollisionObserver secondaryObserver;

    private float primaryAttackCooldownHolder;
    private float secondaryAttackCooldownHolder;

    private PlayerController controller;
    private PlayerData data;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        data = controller.data;

        primaryAttackCooldownHolder = data.primaryAttackCooldown;
        secondaryAttackCooldownHolder = data.secondaryAttackCooldown;
    }
    internal void UpdateCooldowns()
    {
        //update cooldowns
        data.primaryAttackCooldown -= Time.deltaTime;
        data.secondaryAttackCooldown -= Time.deltaTime;
    }
    public bool PrimaryAttack(RaycastHit hit, LayerMask mask)
    {
        //if we left click on the actor mask
        if (primaryAttackObserver.Stay.Contains(hit.collider))
        {
            IActor actor = hit.transform.GetComponent<IActor>();

            if (actor != null && actor.isActorType(ActorType.Enemy))
            {
                StartCoroutine(Utility.DelayedAbility(0.1f, delegate
                {
                    if (actor.IsDestroyed())
                    {
                        return;
                    }
                    actor.Health -= data.primaryAttackDamage;

                    
                    Instantiate(data.primaryHitParticlePrefab, hit.point, Quaternion.identity);
                }));

                data.primaryAttackCooldown = primaryAttackCooldownHolder;
                return true;
            }
        }
        return false;
    }

    public bool SecondaryAttack()
    {
        StartCoroutine(DealAOEDamage());
        return true;
    }

    public bool PrimaryAttackReady()
    {
        if (0 >= data.primaryAttackCooldown)
            return true;
        else
            return false;
    }

    public bool SecondaryAttackReady()
    {
        if (0 >= data.secondaryAttackCooldown)
            return true;
        else
            return false;
    }

    IEnumerator DealAOEDamage()
    {
        yield return new WaitForSeconds(0.01f);

        Collider[] collisions = secondaryObserver.Stay.ToArray();
        for (int i = 0; i < collisions.Length; i++)
        {
            IActor actor = collisions[i].GetComponent<IActor>();
            if (actor != null && actor.isActorType(ActorType.Enemy))
            {
                StartCoroutine(Utility.DelayedAbility(0.5f, delegate
                {
                    if (actor.IsDestroyed())
                    {
                        return;
                    }
                        actor.Health -= data.secondaryAttackDamage;

                    Ray ray = new Ray(transform.position, actor.gameObject.transform.position - transform.position);
                    Debug.DrawRay(transform.position, actor.gameObject.transform.position - transform.position, Color.red, 10);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000, actor.gameObject.layer))
                    {
                        Instantiate(data.secondaryHitParticlePrefab, hit.point, Quaternion.identity);
                    }
                    //GameObject go = Instantiate(AOEEffect, transform.position, transform.localRotation);
                    //go.transform.rotation = transform.rotation;
                }));
                data.secondaryAttackCooldown = secondaryAttackCooldownHolder;
            }
        }
    }
}
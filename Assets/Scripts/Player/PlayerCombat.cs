using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public CollisionObserver primaryAttackObserver;
    public CollisionObserver secondaryObserver;

    public float hitEffectDestroyTimer = 1;

    [NonSerialized] public float primaryAttackCooldownHolder;
    [NonSerialized] public float secondaryAttackCooldownHolder;

    private PlayerController controller;
    private PlayerData data;
    public bool inAction = false;

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
        primaryAttackCooldownHolder -= Time.deltaTime;
        secondaryAttackCooldownHolder -= Time.deltaTime;
    }
    public bool PrimaryAttack(RaycastHit hit, LayerMask mask)
    {
        primaryAttackCooldownHolder = data.primaryAttackCooldown;

        //if we left click on the actor mask
        if (primaryAttackObserver.Stay.Contains(hit.collider))
        {
            IActor actor = hit.transform.GetComponent<IActor>();

            if (actor != null && actor.isActorType(ActorType.Enemy))
            {
                StartCoroutine(Utility.DelayedAbility(data.primaryDelay, delegate
                {
                    if (actor.IsDestroyed())
                    {
                        return;
                    }
                    float h = actor.Health;
                    float d = data.primaryAttackDamage;
                    
                    actor.Health -= data.primaryAttackDamage;
                    if (GameEvents.DamageDealt != null)
                    {
                        GameEvents.DamageDealt((controller, actor, data.primaryAttackDamage));
                    }

                    Destroy(Instantiate(data.primaryHitParticlePrefab, hit.point, Quaternion.identity), hitEffectDestroyTimer);
                    return;
                }));
                return true;
            }
            
        }
        return false;
    }

    public bool SecondaryAttack()
    {
        secondaryAttackCooldownHolder = data.secondaryAttackCooldown;
        
        StartCoroutine(Utility.DelayedAbility(data.secondaryDelay, delegate
        {
            // Find collisions
            Collider[] collisions = secondaryObserver.Stay.ToArray();

            bool hitsomth = false;

            for (int i = 0; i < collisions.Length; i++)
            {
                IActor actor = collisions[i].GetComponent<IActor>();
                if (actor != null && actor.isActorType(ActorType.Enemy))
                {
                    if (actor.IsDestroyed())
                    {
                        return;
                    }
                    actor.Health -= data.secondaryAttackDamage;

                    hitsomth = true;

                    Ray ray = new Ray(transform.position, actor.gameObject.transform.position - transform.position);
                    Debug.DrawRay(transform.position, actor.gameObject.transform.position - transform.position, Color.red, 10);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 1000, actor.gameObject.layer))
                    {
                        Destroy(Instantiate(data.secondaryHitParticlePrefab, hit.point, Quaternion.identity), hitEffectDestroyTimer);
                    }
                }
            }

            // Play sound, depending on hit
            if (hitsomth)
            {
                AudioManager.instance.Play("whoosh");
            } else
            {
                AudioManager.instance.Play("whooshlow");
            }
        }));
        return true;
    }

    public bool PrimaryAttackReady()
    {
        if (0 >= primaryAttackCooldownHolder && !inAction && secondaryAttackCooldownHolder <= 0)
            return true;
        else
            return false;
    }

    public bool SecondaryAttackReady()
    {
        if (0 >= secondaryAttackCooldownHolder && !inAction)
            return true;
        else
            return false;
    }
}
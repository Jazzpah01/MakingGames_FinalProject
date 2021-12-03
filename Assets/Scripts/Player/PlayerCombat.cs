using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Primary Attack Settings")]
    public float primaryAttackDamage;
    public float primaryAttackCooldown;
    public float primaryAttackRange;
    public float primaryAttackDashLength;
    public float primaryAttackDashSpeed;
    public GameObject primaryAttackEffect;
    public CollisionObserver primaryAttackObserver;
    private float primaryAttackCooldownHolder;


    [Header("Secondary Attack Settings")]
    public float secondaryAttackDamage;
    public float secondaryAttackCooldown;
    public float secondaryAttackRange;
    public float SecondaryAttackDashLength;
    public float SecondaryAttackDashSpeed;
    public GameObject secondaryAttackEffect;
    public CollisionObserver secondaryObserver;
    private float secondaryAttackCooldownHolder;

    private void Start()
    {
        primaryAttackCooldownHolder = primaryAttackCooldown;
        secondaryAttackCooldownHolder = secondaryAttackCooldown;
    }
    internal void UpdateCooldowns()
    {
        //update cooldowns
        primaryAttackCooldown -= Time.deltaTime;
        secondaryAttackCooldown -= Time.deltaTime;
    }
    public bool PrimaryAttack(Camera cam, LayerMask mask)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //if we left click on the actor mask
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
            if (primaryAttackObserver.Stay.Contains(hit.collider))
            {
                IActor actor = hit.transform.GetComponent<IActor>();

                if (actor != null && actor.isActorType(ActorType.Enemy))
                {
                    actor.Health -= primaryAttackDamage;
                    Instantiate(primaryAttackEffect,hit.point,Quaternion.identity);
                    primaryAttackCooldown = primaryAttackCooldownHolder;
                    return true;
                }
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
        if (0 >= primaryAttackCooldown)
            return true;
        else
            return false;
    }

    public bool SecondaryAttackReady()
    {
        if (0 >= secondaryAttackCooldown)
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
                actor.Health -= secondaryAttackDamage;
                Ray ray = new Ray(transform.position, actor.gameObject.transform.position - transform.position);
                Debug.DrawRay(transform.position, actor.gameObject.transform.position - transform.position, Color.red, 10);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, actor.gameObject.layer))
                {
                    Instantiate(secondaryAttackEffect, hit.point, Quaternion.identity);
                }
                secondaryAttackCooldown = secondaryAttackCooldownHolder;
            }
        }
    }
}

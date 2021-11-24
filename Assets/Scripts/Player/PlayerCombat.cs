using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    public float primaryAttackDamage;
    public float primaryAttackCooldown;
    public float primaryAttackRange;
    public GameObject primaryAttackEffect;
    public CollisionObserver primaryAttackObserver;
    private float primaryAttackTime;

    public float secondaryAttackDamage;
    public float secondaryAttackCooldown;
    public float secondaryAttackRange;
    public GameObject secondaryAttackEffect;
    public CollisionObserver secondaryObserver;
    private float secondaryAttackTime;


    internal void UpdateCooldowns()
    {
        //update cooldowns
        primaryAttackTime -= Time.deltaTime;
        secondaryAttackTime -= Time.deltaTime;
    }
    public void PrimaryAttack(Camera cam, LayerMask mask)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //if we left click on the actor mask
        if (Physics.Raycast(ray, out hit, 1000, mask))
        {
        if (PrimaryAttackReady() && primaryAttackObserver.Stay.Contains(hit.collider))
        {
            IActor actor = hit.transform.GetComponent<IActor>();

            if (actor != null && actor.type == ActorType.Enemy)
            {
                primaryAttackTime = primaryAttackCooldown;
                actor.Health -= primaryAttackDamage;
                Instantiate(primaryAttackEffect).transform.position = hit.point;
            }
        }
        }
    }

    public void SecondaryAttack()
    {
        if (SecondaryAttackReady())
        {
            secondaryAttackTime = secondaryAttackCooldown;
            Collider[] collisions = secondaryObserver.Stay.ToArray();

            Instantiate(secondaryAttackEffect).transform.position = transform.position;

            for (int i = 0; i < collisions.Length; i++)
            {
                IActor actor = collisions[i].GetComponent<IActor>();

                if (actor == null || actor.type != ActorType.Enemy)
                    continue;

                actor.Health -= secondaryAttackDamage;

            }
        }
    }

    public bool PrimaryAttackReady()
    {
        if (0 < primaryAttackCooldown)
            return true;
        else
            return false;
    }

    public bool SecondaryAttackReady()
    {
        if (0 < primaryAttackCooldown)
            return true;
        else
            return false;
    }
}

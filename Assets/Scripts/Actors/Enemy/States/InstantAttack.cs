using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An AI state which will attack the target with some cooldown and delay.
/// </summary>
[System.Serializable]
public class InstantAttack : AIState
{
    public float damage;
    public float delay;
    public float cooldown;
    public float turningSpeed = 2;

    private float delayTimer = 0;
    private bool ready = true;

    protected override void EnterAIState()
    {
        parent.agent.updateRotation = false;
        delayTimer = 0;
    }

    protected override void ExitAIState()
    {
        parent.agent.updateRotation = true;
    }
    protected override void UpdateAIState()
    {
        LookAtTarget();
        if (!ready)
            return;

        if (delayTimer < delay)
        {
            delayTimer += Time.deltaTime;
            status = StateStatus.Executing;
        } else
        {
            delayTimer = 0;
            parent.Target.Health -= damage * parent.controller.damageModifyer;
            status = StateStatus.Finished;
            ready = false;
            parent.CallInSeconds(Cooldown, cooldown);
        }
    }

    private void LookAtTarget()
    {
        Transform transform, targetTransform;
        transform = parent.transform;
        targetTransform = parent.Target.gameObject.transform;
        Vector3 targetDirection = targetTransform.position - transform.position;
        Vector3 direction = Vector3.RotateTowards(transform.forward, targetDirection, turningSpeed * Time.deltaTime, 0f);
        parent.gameObject.transform.rotation = Quaternion.LookRotation(direction);
    }

    void Cooldown()
    {
        ready = true;
    }
}
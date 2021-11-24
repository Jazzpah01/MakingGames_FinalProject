using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantAttack : AIState
{
    public float damage;
    public float delay;
    public float cooldown;

    private float delayTimer = 0;
    private bool ready = true;

    protected override void EnterAIState()
    {
        delayTimer = 0;
    }

    protected override void UpdateAIState()
    {
        if (!ready)
            return;

        if (delayTimer < delay)
        {
            delayTimer += Time.deltaTime;
            status = StateStatus.Executing;
        } else
        {
            delayTimer = 0;
            parent.Target.Health -= damage;
            status = StateStatus.Finished;
            ready = false;
            parent.CallInSeconds(Cooldown, cooldown);
        }
    }

    void Cooldown()
    {
        ready = true;
    }
}
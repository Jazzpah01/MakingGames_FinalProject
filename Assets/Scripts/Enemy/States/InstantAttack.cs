using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantAttack : AIState
{
    public float damage;
    public float delay;

    private float delayTimer = 0;

    protected override void EnterAIState()
    {
        delayTimer = 0;
    }

    protected override void UpdateAIState()
    {
        if (status != StateStatus.Executing)
            return;

        if (delayTimer < delay)
        {
            delayTimer += Time.deltaTime;
        } else
        {
            parent.target.Health -= damage;
            status = StateStatus.Finished;
        }
    }
}
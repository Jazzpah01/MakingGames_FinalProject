using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathMove : AIState
{
    public float speedModifyer = 1;
    public float stoppingDistance = 0.8f;

    protected override void EnterAIState()
    {
        parent.agent.speed = parent.controller.Speed * speedModifyer;
        parent.agent.stoppingDistance = 0.8f;
        parent.agent.updateRotation = false;
        parent.agent.isStopped = false;
    }

    protected override void ExitAIState()
    {
        parent.agent.isStopped = true;
        parent.agent.stoppingDistance = 0f;
        parent.agent.updateRotation = true;
    }

    protected override void UpdateAIState()
    {
        if (parent.target.IsDestroyed())
        {
            parent.target = null;
            parent.agent.SetDestination(parent.transform.position);
            parent.agent.isStopped = true;
            return;
        }

        parent.agent.SetDestination(parent.target.gameObject.transform.position);
    }
}
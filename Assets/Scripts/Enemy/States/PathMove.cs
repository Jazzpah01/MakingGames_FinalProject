using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An AI state that will use path finding to walk towards a target transform
/// </summary>
[System.Serializable]
public class PathMove : AIState
{
    public float speedModifyer = 1;
    public float stoppingDistance = 0.8f;

    protected override void EnterAIState()
    {
        if (parent.IsDestroyed())
            return;

        parent.agent.speed = parent.controller.Speed * speedModifyer * parent.controller.SpeedModifyer;
        parent.agent.stoppingDistance = stoppingDistance;
        parent.agent.updateRotation = false;
        parent.agent.isStopped = false;
    }

    protected override void ExitAIState()
    {
        if (parent.IsDestroyed())
            return;

        parent.agent.isStopped = true;
        parent.agent.stoppingDistance = 0f;
        parent.agent.updateRotation = true;
    }

    protected override void UpdateAIState()
    {
        if (parent.TargetTransform.IsDestroyed() || parent.IsDestroyed())
        {
            status = StateStatus.Finished;
            parent.Target = null;
            parent.agent.SetDestination(parent.transform.position);
            parent.agent.isStopped = true;
            return;
        }

        parent.agent.speed = parent.controller.Speed * speedModifyer * parent.controller.SpeedModifyer;
        parent.agent.SetDestination(parent.TargetTransform.position);
    }
}
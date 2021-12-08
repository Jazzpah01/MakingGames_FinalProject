using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An AI state that will use path finding to walk towards a target transform
/// </summary>
[System.Serializable]
public class PathMove : AIState
{
    public float speedModifyer = 1;
    public float stoppingDistance = 0.8f;

    private NavMeshPath path;

    protected override void EnterAIState()
    {
        if (parent.IsDestroyed())
            return;

        parent.agent.SetDestination(parent.TargetTransform.position);
        parent.agent.isStopped = false;
        parent.agent.speed = parent.controller.Speed * speedModifyer * parent.controller.speedModifyer;
        parent.agent.stoppingDistance = stoppingDistance;
        parent.agent.updateRotation = true;
    }

    protected override void ExitAIState()
    {
        if (parent.IsDestroyed())
            return;

        parent.agent.speed = parent.controller.Speed * speedModifyer * parent.controller.speedModifyer;
        parent.agent.isStopped = true;
        parent.agent.stoppingDistance = stoppingDistance;
        parent.agent.updateRotation = false;
        parent.agent.SetDestination(parent.transform.position);
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

        //if (Vector3.Distance(parent.transform.position, parent.TargetTransform.position) > stoppingDistance)
        //{
        //    parent.agent.updatePosition = false;
        //    parent.transform.LookAt(parent.transform.position + parent.agent.velocity, Vector3.up);
        //} else
        //{
        //    parent.agent.updatePosition = true;
        //}
            
        parent.agent.SetDestination(parent.TargetTransform.position);
        parent.agent.isStopped = false;
        parent.agent.speed = parent.controller.Speed * speedModifyer * parent.controller.speedModifyer;
    }
}
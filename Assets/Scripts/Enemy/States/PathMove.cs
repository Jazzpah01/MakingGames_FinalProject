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
        Debug.Log(parent);
        Debug.Log(parent.agent);

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
        parent.agent.SetDestination(parent.target.gameObject.transform.position);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectMove : AIState
{
    public float speedModifyer = 1;
    public float stoppingDistance = 0.8f;

    protected override void UpdateAIState()
    {
        if (parent.target.IsDestroyed())
        {
            parent.target = null;
            return;
        }

        Vector3 difference = (parent.target.gameObject.transform.position - parent.transform.position);

        if (difference.magnitude <= stoppingDistance)
            return;

        Vector3 targetVelocity = difference.normalized;
        targetVelocity *= parent.controller.Speed * speedModifyer;

        targetVelocity.y = 0;

        parent.agent.velocity = targetVelocity;
    }
}
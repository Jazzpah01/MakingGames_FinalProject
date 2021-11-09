using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBStateMachine : MonoBehaviour
{
    private IState currentState;

    protected void Update()
    {
        currentState.UpdateState();
    }

    protected void LateUpdate()
    {
        currentState.LateUpdateState();
    }

    protected void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
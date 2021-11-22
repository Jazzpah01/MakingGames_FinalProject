using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : IState
{
    public enum StateStatus
    {
        None,
        Executing,
        Finished
    }

    public StateStatus status { get; protected set; }

    protected AIStateMachine parent;

    public void Initialize(AIStateMachine parent)
    {
        this.parent = parent;
        status = StateStatus.None;
    }

    public void EnterState() {
        if (parent == null)
            throw new System.Exception("Cannot enter a state that hasn't been initialized!");

        if (!parent.initialized)
            throw new System.Exception("Cannot enter a state, when the state machine hasn't been initialized!");

        status = StateStatus.Executing;
        EnterAIState();
    }
    public void ExitState() {
        status = StateStatus.None;
        ExitAIState();
    }
    public void UpdateState(){
        UpdateAIState();
    }
    public void LateUpdateState(){
        LateUpdateAIState();
    }

    protected virtual void EnterAIState() { }
    protected virtual void ExitAIState() { }
    protected virtual void UpdateAIState() { }
    protected virtual void LateUpdateAIState() { }
}
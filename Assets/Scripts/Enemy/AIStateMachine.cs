using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIStateMachine : MBStateMachine
{
    public IActor controller;
    public IActor target;
    [HideInInspector] public NavMeshAgent agent;

    public bool initialized = false;

    protected void Initialize()
    {
        print("Test start");
        controller = GetComponent<IActor>();
        agent = GetComponent<NavMeshAgent>();
    }
}
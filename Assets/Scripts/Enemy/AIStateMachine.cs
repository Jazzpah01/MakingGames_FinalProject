using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIStateMachine : MBStateMachine
{
    public IActor controller;
    public IActor target;
    [HideInInspector] public NavMeshAgent agent;

    public bool Initialized { get; private set; }

    protected void Initialize()
    {
        print("Test start");
        Initialized = true;
        controller = GetComponent<IActor>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void CallInSeconds(Action method, float seconds)
    {
        StartCoroutine(CallInSecondsCoroutine(method, seconds));
    }

    IEnumerator CallInSecondsCoroutine(Action method, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        method();
    }
}
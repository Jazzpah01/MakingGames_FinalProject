using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIStateMachine : MBStateMachine
{
    public IActor controller;

    private IActor target;
    private Transform targetTransform;

    public IActor Target { get => target;
        set
        {
            target = value;
            if (value != null)
            {
                targetTransform = value.gameObject.transform;
            } else
            {
                targetTransform = null;
            }
        }
    }
    public Transform TargetTransform
    {
        get => targetTransform;
        set
        {
            target = null;
            targetTransform = value;
        }
    }

    [HideInInspector] public NavMeshAgent agent;

    public bool Initialized { get; private set; }

    protected void Initialize()
    {
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
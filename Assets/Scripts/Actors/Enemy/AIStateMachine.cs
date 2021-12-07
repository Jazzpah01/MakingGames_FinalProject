using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIStateMachine : MBStateMachine
{
    [NonSerialized]public ActorData actorData;

    public IActor controller;

    private IActor target;
    private Transform targetTransform;

    // Target of the AI. The states will use this as reference.
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
    // Target transform of the AI. The states will use this as reference.
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

    protected void Initialize(ActorData data)
    {
        controller = GetComponent<IActor>();
        agent = GetComponent<NavMeshAgent>();

        print("" + data.maxHealth);

        controller.MaxHealth = data.maxHealth;
        controller.Health = data.maxHealth;
        controller.Speed = data.speed;
        controller.speedModifyer = 1;
        controller.damageModifyer = 1;

        Initialized = true;
    }

    /// <summary>
    /// Method to call a delegate after a given amount of seconds. Useful for cooldowns.
    /// </summary>
    /// <param name="method"></param>
    /// <param name="seconds"></param>
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
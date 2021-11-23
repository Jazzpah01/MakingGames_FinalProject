using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : AIStateMachine
{
    public DirectMove move;
    public InstantAttack attack;

    public CollisionObserver detectCarrots;
    public CollisionObserver detectAttack;

    private bool inRange = false;

    protected void Start()
    {
        Initialize();
        move.Initialize(this);
        attack.Initialize(this);

        target = null;

        //detectCarrots.Subscribe(detectCarrot_Enter, CollisionObserver.CollisionType.Enter);
        detectAttack.Subscribe(detectAttack_Enter, CollisionObserver.CollisionType.Enter);
        detectAttack.Subscribe(detectAttack_Exit, CollisionObserver.CollisionType.Exit);

        SetCarrotTarget();

        ChangeState(move);
    }

    void Update()
    {
        if (target == null)
            SetCarrotTarget();

        if (currentState == move)
        {
            if (inRange)
            {
                ChangeState(attack);
            }
        } else if (currentState == attack)
        {
            if (attack.status == AIState.StateStatus.Finished)
            {
                ChangeState(move);
            }
        }

        base.Update();
    }

    void SetCarrotTarget()
    {
        foreach (Collider other in detectCarrots.Stay)
        {
            IActor a = other.gameObject.GetComponent<IActor>();

            if (a != null && a.type == ActorType.Crops)
            {
                target = a;
                break;
            }
        }
    }

    void detectAttack_Enter(Collider other)
    {
        IActor otherActor = other.GetComponent<IActor>();

        if (otherActor != null && otherActor == target)
        {
            inRange = true;
        }
    }

    void detectAttack_Exit(Collider other)
    {
        IActor otherActor = other.GetComponent<IActor>();

        if (otherActor != null && otherActor == target)
        {
            inRange = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : AIStateMachine
{
    public PathMove move;
    public PathMove flee;
    public InstantAttack attack;

    public CollisionObserver detectCarrots;
    public CollisionObserver detectAttack;

    private bool inRange = false;

    protected void Start()
    {
        Initialize();
        move.Initialize(this);
        flee.Initialize(this);
        attack.Initialize(this);

        target = null;

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
            if (target == null)
            {
                ChangeState(flee);
            }

            if (inRange)
            {
                ChangeState(attack);
            }
        } else if (currentState == attack)
        {
            if (target == null)
            {
                ChangeState(flee);
            }

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
            if (other == null)
                continue;

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
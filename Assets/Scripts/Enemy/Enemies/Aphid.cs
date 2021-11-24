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

        Target = null;

        detectAttack.Subscribe(detectAttack_Enter, CollisionObserver.CollisionType.Enter);
        detectAttack.Subscribe(detectAttack_Exit, CollisionObserver.CollisionType.Exit);

        SetCarrotTarget();

        ChangeState(move);
    }

    void Update()
    {
        if (Target.IsDestroyed())
        {
            inRange = false;
            if (currentState != flee)
            {
                SetCarrotTarget();
            }
        }

        if (currentState == move)
        {
            if (Target.IsDestroyed())
            {
                TargetTransform = ((IEnemy)controller).spawnPoint.transform;
                ChangeState(flee);
            }

            if (inRange)
            {
                ChangeState(attack);
            }
        } else if (currentState == attack)
        {
            if (Target.IsDestroyed())
            {
                TargetTransform = ((IEnemy)controller).spawnPoint.transform;
                ChangeState(flee);
            }

            if (attack.status == AIState.StateStatus.Finished)
            {
                ChangeState(move);
            }
        } else if (currentState == flee)
        {
            Vector3 dist = (TargetTransform.position - transform.position);
            dist.y = 0;
            if ((dist).magnitude < 4)
            {
                Destroy(this.gameObject);
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
                Target = a;
                break;
            }
        }
    }

    void detectAttack_Enter(Collider other)
    {
        IActor otherActor = other.GetComponent<IActor>();

        if (otherActor != null && otherActor == Target)
        {
            inRange = true;
        }
    }

    void detectAttack_Exit(Collider other)
    {
        IActor otherActor = other.GetComponent<IActor>();

        if (otherActor != null && otherActor == Target)
        {
            inRange = false;
        }
    }
}
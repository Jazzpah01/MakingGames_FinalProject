using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : AIStateMachine
{
    public AphidData data;

    public CollisionObserver detectCarrots;
    public CollisionObserver detectAttack;

    private bool inRange = false;

    protected void Start()
    {
        // Find a crops to follow
        IActor[] crops = GameManager.instance.buildingController.buildablesParent.gameObject.GetComponentsInChildren<IActor>();

        foreach (IActor item in crops)
        {
            if (item.isActorType(ActorType.Crops))
            {
                Target = item;
            }
        }

        // If crops doesn't exit: remove this
        if (Target.IsDestroyed())
        {
            Target = GameController.instance.baseController;
        }

        data = Instantiate(data);

        // Initialize states and machine
        Initialize(data);
        data.move.Initialize(this);
        data.attack.Initialize(this);

        // Subscribe to elements
        detectAttack.Subscribe(detectAttack_Enter, CollisionObserver.CollisionType.Enter);
        detectAttack.Subscribe(detectAttack_Exit, CollisionObserver.CollisionType.Exit);

        ChangeState(data.move);
    }

    void Update()
    {
        if (Target.IsDestroyed())
        {
            inRange = false;
            SetCarrotTarget();
        }

        if (currentState == data.move)
        {
            // Move state. Move to a crops actor
            if (Target.IsDestroyed())
            {
                Target = GameController.instance.baseController;
            }

            if (inRange)
            {
                ChangeState(data.attack);
            }
        } else if (currentState == data.attack)
        {
            // In state data.attack. Attack target
            if (Target.IsDestroyed())
            {
                Target = GameController.instance.baseController;
                ChangeState(data.move);
            }

            if (data.attack.status == AIState.StateStatus.Finished)
            {
                ChangeState(data.move);
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

            if (a != null && a.isActorType(ActorType.Crops))
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : AIStateMachine
{
    GameManager gameManager;

    public AphidData data;

    public CollisionObserver detectCarrots;
    public CollisionObserver detectAttack;

    public Animator animator;
    public GameObject model;

    private bool inRange = false;

    protected void Start()
    {
        // Find a crops to follow
        SetCarrotTarget();

        // If crops doesn't exit: remove this
        if (Target.IsDestroyed())
        {
            Target = gameManager.baseController;
        }

        data = Instantiate(data);

        // Initialize states and machine
        Initialize(data);
        data.pathMove.Initialize(this);
        data.instantAttack.Initialize(this);

        // Subscribe to elements
        detectAttack.Subscribe(detectAttack_Enter, CollisionObserver.CollisionType.Enter);
        detectAttack.Subscribe(detectAttack_Exit, CollisionObserver.CollisionType.Exit);

        agent.updateRotation = false;

        ChangeState(data.pathMove);
    }

    void Update()
    {
        if (Target.IsDestroyed())
        {
            inRange = false;
            SetCarrotTarget();
        }

        if (currentState == data.pathMove)
        {
            // Move state. Move to a crops or base
            if (Target.IsDestroyed())
            {
                Target = gameManager.baseController;
            }

            animator.SetTrigger("Walking");

            if (inRange)
            {
                ChangeState(data.instantAttack);
            }
        }
        else if (currentState == data.instantAttack)
        {
            // In state data.attack. Attack target
            if (Target.IsDestroyed())
            {
                Target = gameManager.baseController;
                ChangeState(data.pathMove);
            }

            if (data.instantAttack.status == AIState.StateStatus.Finished)
            {
                ChangeState(data.pathMove);
            }
        }

        base.Update();
    }

    void SetCarrotTarget()
    {
        // Find a crops to follow
        IActor[] crops = gameManager.buildingController.buildablesParent.gameObject.GetComponentsInChildren<IActor>();

        IActor closest = null;
        float currentDistance = float.MaxValue;

        foreach (IActor item in crops)
        {
            float distance = Vector3.Distance(item.gameObject.transform.position, transform.position);
            if (!item.IsDestroyed() &&
                item.isActorType(ActorType.Crops) &&
                distance < currentDistance)
            {
                closest = item;
                currentDistance = distance;
            }
        }

        if (!closest.IsDestroyed())
        {
            Target = closest;
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
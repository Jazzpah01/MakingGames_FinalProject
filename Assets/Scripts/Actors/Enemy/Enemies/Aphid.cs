using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : AIStateMachine
{
    public AphidData data;

    public CollisionObserver detectCarrots;
    public CollisionObserver detectAttack;

    public Animator animator;
    public GameObject model;

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
        data.pathMove.Initialize(this);
        data.instantAttack.Initialize(this);

        // Subscribe to elements
        detectAttack.Subscribe(detectAttack_Enter, CollisionObserver.CollisionType.Enter);
        detectAttack.Subscribe(detectAttack_Exit, CollisionObserver.CollisionType.Exit);

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
            // Move state. Move to a crops actor
            if (Target.IsDestroyed())
            {
                Target = GameController.instance.baseController;
            }

            animator.SetTrigger("Walking");
            model.transform.LookAt(Target.gameObject.transform, Vector3.up);

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
                Target = GameController.instance.baseController;
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
        IActor[] crops = GameManager.instance.buildingController.buildablesParent.gameObject.GetComponentsInChildren<IActor>();

        foreach (IActor item in crops)
        {
            if (item.isActorType(ActorType.Crops))
            {
                Target = item;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slug will attack Player and Building, and any obstacle in the way
/// </summary>
public class Slug : AIStateMachine
{
    public SlugData data;

    public CollisionObserver detectObstruction;
    public CollisionObserver detectAttack;

    private float range = 0;

    private void Start()
    {
        // Initialize data
        data = Instantiate(data);

        // Initialize
        Initialize(data);
        data.directMove.Initialize(this);
        data.instantAttack.Initialize(this);

        // Set target to base initially
        Target = GameController.instance.baseController;

        // Setup collision observers
        detectObstruction.Subscribe(detectObstruction_Enter, CollisionObserver.CollisionType.Enter);

        // Change state
        ChangeState(data.directMove);
    }

    private void Update()
    {
        if (currentState == data.directMove)
        {
            // Move state, data.move towards target
            if ((transform.position - GameController.instance.player.transform.position).magnitude <=
            (transform.position - Target.gameObject.transform.position).magnitude)
            {
                Target = GameController.instance.player.GetComponent<IActor>();
            }

            if ((transform.position - GameController.instance.baseController.transform.position).magnitude <=
            (transform.position - Target.gameObject.transform.position).magnitude)
            {
                Target = GameController.instance.baseController;
            }

            if (detectAttack.Stay.Contains(Target.gameObject.GetComponent<Collider>()))
            {
                ChangeState(data.instantAttack);
            }
        } else if (currentState == data.instantAttack)
        {
            // Attack state, deal damage to target
            if (Target.IsDestroyed())
            {
                Target = GameController.instance.player.GetComponent<IActor>();
                ChangeState(data.directMove);
                return;
            }

            if (detectAttack.Exit.Contains(Target.gameObject.GetComponent<Collider>()))
            {
                ChangeState(data.directMove);
            }
        }

        base.Update();
    }

    void detectObstruction_Enter(Collider other)
    {
        IActor otherActor = other.GetComponent<IActor>();

        if (otherActor == null)
            return;

        if ((otherActor.gameObject.transform.position - transform.position).magnitude <
            (Target.gameObject.transform.position - transform.position).magnitude &&
            (otherActor.isActorType(ActorType.Obstacle)))
        {
            Target = otherActor;
            ChangeState(data.directMove);
        }
    }
}
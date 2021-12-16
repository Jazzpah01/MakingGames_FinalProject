using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Slug will attack Player and Building, and any obstacle in the way
/// </summary>
public class NewSlug : AIStateMachine
{
    GameManager gameManager;
    PlayerManager playerManager;

    public SlugData data;
    public CollisionObserver detectObstruction;
    public CollisionObserver detectAttack;
    public Animator animator;
    public GameObject model;

    private void Start()
    {
        gameManager = GameManager.instance;
        playerManager = PlayerManager.instance;
        // Initialize data
        data = Instantiate(data);

        // Initialize
        Initialize(data);
        data.pathMove.Initialize(this);
        data.instantAttack.Initialize(this);

        // Set target to base initially
        Target = gameManager.baseController;

        // Setup collision observers
        detectObstruction.Subscribe(detectObstruction_Enter, CollisionObserver.CollisionType.Enter);

        agent.updateRotation = false;

        // Change state
        ChangeState(data.pathMove);
    }

    private void Update()
    {
        animator.SetTrigger("Walking");
        if (currentState == data.pathMove)
        {
            if (Target.IsDestroyed())
            {
                Target = playerManager.player.GetComponent<IActor>();
                return;
            }

            if ((transform.position - gameManager.baseController.transform.position).magnitude <=
            (transform.position - Target.gameObject.transform.position).magnitude)
            {
                Target = gameManager.baseController;
            }

            if (detectAttack.Stay.Contains(Target.gameObject.GetComponent<Collider>()))
            {
                ChangeState(data.instantAttack);
            }
        }
        else if (currentState == data.instantAttack)
        {
            // Attack state, deal damage to target
            if (Target.IsDestroyed())
            {
                Target = playerManager.player.GetComponent<IActor>();
                ChangeState(data.pathMove);
                return;
            }

            // Moves out of range
            if (detectAttack.Exit.Contains(Target.gameObject.GetComponent<Collider>()))
            {
                Target = gameManager.baseController;
                ChangeState(data.pathMove);
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
            ((otherActor.isActorType(ActorType.Obstacle)) ||
            (otherActor.isActorType(ActorType.Player))))
        {
            Target = otherActor;
            ChangeState(data.pathMove);
        }
    }
}
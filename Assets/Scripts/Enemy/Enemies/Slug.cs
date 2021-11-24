using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : AIStateMachine
{
    public DirectMove move;
    public InstantAttack attack;

    public CollisionObserver detectObstruction;
    public CollisionObserver detectAttack;

    private float range = 0;

    private void Start()
    {
        Initialize();
        move.Initialize(this);
        attack.Initialize(this);

        Target = GameController.instance.baseController;

        ChangeState(move);
    }

    private void Update()
    {
        if (currentState == move)
        {
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
                ChangeState(attack);
            }
        } else if (currentState == attack)
        {
            if (detectAttack.Exit.Contains(Target.gameObject.GetComponent<Collider>()))
            {
                ChangeState(move);
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
            otherActor.type == ActorType.Obstacle)
        {
            Target = otherActor;
            ChangeState(move);
        }
    }
}
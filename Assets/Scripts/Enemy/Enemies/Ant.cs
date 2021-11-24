using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ant will attack the player exclusively.
/// </summary>
public class Ant : AIStateMachine
{
    public PathMove pathMove;
    public InstantAttack attack;

    public float range = 3;

    // Start is called before the first frame update
    protected void Start()
    {
        Initialize();
        pathMove.Initialize(this);
        attack.Initialize(this);

        Target = GameController.instance.player.GetComponent<IActor>();

        ChangeState(pathMove);
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = (transform.position - Target.gameObject.transform.position).magnitude;

        if (currentState == pathMove)
        {
            if (targetDistance <= range)
            {
                ChangeState(attack);
            }
        } else 
        if (currentState == attack)
        {
            if (attack.status == AIState.StateStatus.Finished &&
                targetDistance > range)
            {
                ChangeState(pathMove);
            }
        }

        base.Update();
    }
}
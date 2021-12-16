using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ant will attack the player exclusively.
/// </summary>
public class Ant : AIStateMachine
{
    public AntData data;
    public Animator animator;
    public GameObject model;

    // Start is called before the first frame update
    protected void Start()
    {
        data = Instantiate(data);

        Initialize(data);
        data.pathMove.Initialize(this);
        data.instantAttack.Initialize(this);

        Target = PlayerManager.instance.player.GetComponent<IActor>();

        agent.updateRotation = false;

        ChangeState(data.pathMove);
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = (transform.position - Target.gameObject.transform.position).magnitude;

        if (currentState == data.pathMove)
        {
            animator.SetTrigger("Walking");

            if (targetDistance <= data.range)
            {
                ChangeState(data.instantAttack);
            }
        } else 
        if (currentState == data.instantAttack)
        {
            animator.SetTrigger("Walking");

            if (data.instantAttack.status == AIState.StateStatus.Finished &&
                targetDistance > data.range)
            {
                ChangeState(data.pathMove);
            }
        }

        base.Update();
    }
}
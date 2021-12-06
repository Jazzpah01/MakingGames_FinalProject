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

    public float range = 3;

    // Start is called before the first frame update
    protected void Start()
    {
        Initialize(data);
        data.pathMove.Initialize(this);
        data.instantAttack.Initialize(this);

        Target = GameController.instance.player.GetComponent<IActor>();

        ChangeState(data.pathMove);
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = (transform.position - Target.gameObject.transform.position).magnitude;

        if (currentState == data.pathMove)
        {
            animator.SetTrigger("Walking");
            model.transform.LookAt(Target.gameObject.transform, Vector3.up);
            if (targetDistance <= range)
            {
                ChangeState(data.instantAttack);
            }
        } else 
        if (currentState == data.instantAttack)
        {
            animator.SetTrigger("Walking");
            model.transform.LookAt(Target.gameObject.transform);
            if (data.instantAttack.status == AIState.StateStatus.Finished &&
                targetDistance > range)
            {
                ChangeState(data.pathMove);
            }
        }

        base.Update();
    }
}
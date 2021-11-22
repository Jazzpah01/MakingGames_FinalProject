using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : AIStateMachine
{
    public PathMove pathMove;
    public InstantAttack attack;

    public float cooldown = 0.5f;
    public float range = 3;

    private float cooldownTimer = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        Initialize();
        pathMove.Initialize(this);
        attack.Initialize(this);

        target = GameController.instance.player.GetComponent<PlayerController>();

        ChangeState(pathMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer < cooldown)
            cooldownTimer += Time.deltaTime;

        if (currentState == pathMove)
        {
            if ((transform.position - target.gameObject.transform.position).magnitude < range &&
                cooldownTimer >= cooldown)
            {
                ChangeState(attack);
            }
        } else 
        if (currentState == attack)
        {
            if (attack.status == AIState.StateStatus.Finished)
            {
                ChangeState(pathMove);
            }
        }

        base.Update();
    }
}
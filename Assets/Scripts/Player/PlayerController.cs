using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IActor, IState
{
    PlayerManager playerManager;
    GameController gameController;
    PlayerCombat combat;
    PlayerMotor motor;

    public LayerMask movementMask;
    public LayerMask actorMask;
    public float maxHealth = 100;

    [HideInInspector]
    private Camera cam;
    private float health;

    public ActorType type => ActorType.Player;
    //this was made to satisfy implementation of IActor, may need some refactoring
    private float speed;

    [Header("References")]
    public Animator animator;

    public float Speed { get => speed; set { speed = value; } }
    public float MaxHealth => maxHealth;
    public float Health
    {
        get => health; set
        {
            health = value;
            if (health <= 0)
            {
                gameController.GameOver();
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageReduction { get; set; }

    private void Start()
    {
        playerManager = PlayerManager.instance;
        gameController = GameController.instance;
        motor = GetComponent<PlayerMotor>();
        combat = GetComponent<PlayerCombat>();
        cam = playerManager.cam;
        health = maxHealth;
        blockDamage = false;
    }

    private void Update()
    {
        if (motor.isMoving)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    public void UpdateState()
    {
        combat.UpdateCooldowns();

        //return if the pointer 
        if (InteractableUI.OnUI)
        {
            return;
        }

        //left click
        if (Input.GetMouseButton(0) && combat.PrimaryAttackReady())
        {
            animator.SetBool("moving", false);
            animator.SetTrigger("attack");
            motor.blockMoving = true;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, actorMask))
            {
                //prepare to attack by turning to look at this direction
                motor.TurnTowardsTarget(hit.point);
                //attempt to do the attack
                if (combat.PrimaryAttack(hit, actorMask))
                {
                    StartCoroutine(DelayedAbility(0.1f, delegate
                    {
                        //if attack is successful, dash
                        motor.Dash(combat.primaryAttackDashSpeed,
                            Mathf.Min(combat.primaryAttackDashLength, Vector3.Distance(transform.position, hit.point)));
                        motor.blockMoving = false;
                    }));
                }
            } else
            {
                motor.blockMoving = false;
            }
        }

        //right click
        if (Input.GetMouseButtonDown(1) && combat.SecondaryAttackReady())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            animator.SetBool("moving", false);
            animator.SetTrigger("slash");
            motor.blockMoving = true;

            if (Physics.Raycast(ray, out hit, 1000, movementMask))
            {
                //prepare to attack by turning to look at this direction
                motor.TurnTowardsTarget(hit.point);
                //attempt to do the attack
                if (combat.SecondaryAttack())
                {
                    StartCoroutine(Utility.DelayedAbility(0.5f, delegate {
                        //if attack is successful, dash
                        //motor.Dash(combat.SecondaryAttackDashSpeed, 
                        //    Mathf.Min(combat.SecondaryAttackDashLength, Vector3.Distance(transform.position, hit.point)));
                        motor.Dash(combat.SecondaryAttackDashSpeed, combat.SecondaryAttackDashLength);
                        motor.blockMoving = false;
                    }));
                }
            }
            else
            {
                motor.blockMoving = false;
            }
        }
    }

    private IEnumerator DelayedAbility(float delay, Action function)
    {
        yield return new WaitForSeconds(delay);

        function();
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }

    public void LateUpdateState()
    {

    }
}

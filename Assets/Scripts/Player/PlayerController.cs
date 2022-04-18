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

    [HideInInspector]
    private Camera cam;

    public PlayerData data;

    public ActorType actorType => ActorType.Player;
    private float currentHealth;

    [Header("References")]
    public Animator animator;

    private float health;

    public float Speed { get; set; }
    public float MaxHealth { get; set; }
    public float Health
    {
        get => health; set
        {
            health = value;
            animator.SetTrigger("damaged");
            if (health <= 0)
            {
                animator.SetTrigger("dead");
                if (GameEvents.ActorKilled != null)
                {
                    GameEvents.ActorKilled(this);
                }
                gameController.GameOver();
            }
            else if (health > MaxHealth)
            {
                health = MaxHealth;
                Debug.Log("Health flowover in " + gameObject.name);
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageModifyer { get; set; }
    public float speedModifyer { get; set; }

    private void OnDestroy()
    {
        GameEvents.ActorDestroyed(this);
    }

    private void Start()
    {
        playerManager = PlayerManager.instance;
        gameController = GameController.instance;
        motor = GetComponent<PlayerMotor>();
        combat = GetComponent<PlayerCombat>();
        cam = playerManager.cam;

        MaxHealth = data.maxHealth;
        Health = data.maxHealth;
        Speed = data.speed;
        damageModifyer = 1;
        speedModifyer = 1;

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
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000, actorMask))
            {
                //prepare to attack by turning to look at this direction
                motor.TurnTowardsTarget(hit.point);
                //attempt to do the attack
                if (combat.PrimaryAttack(hit, actorMask))
                {
                    animator.SetBool("moving", false);
                    animator.SetTrigger("attack");
                    motor.blockMoving = true;
                    combat.inAction = true;
                    StartCoroutine(DelayedAbility(data.primaryDelay, delegate
                    {
                        //if attack is successful, dash
                        motor.Dash(data.primaryAttackDashSpeed,
                            Mathf.Min(data.primaryAttackDashLength, Vector3.Distance(transform.position, hit.point)/2));
                        AudioManager.instance.Play("whack");
                        motor.blockMoving = false;
                        combat.inAction = false;
                    }));
                }
            }
            else
            {
                combat.primaryAttackCooldownHolder = data.primaryAttackCooldown;
                animator.SetTrigger("attack");
                motor.blockMoving = true;
                StartCoroutine(Utility.DelayedAbility(data.primaryDelay, delegate
                {
                    motor.Dash(data.primaryAttackDashSpeed,
                        Mathf.Min(data.primaryAttackDashLength, Vector3.Distance(transform.position, hit.point)));
                    motor.blockMoving = false;
                    combat.inAction = false;
                }
                ));
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
                combat.inAction = true;
                //attempt to do the attack
                if (combat.SecondaryAttack())
                {
                    StartCoroutine(Utility.DelayedAbility(data.secondaryDelay, delegate {

                        //if attack is successful, dash
                        //motor.Dash(combat.SecondaryAttackDashSpeed, 
                        //    Mathf.Min(combat.SecondaryAttackDashLength, Vector3.Distance(transform.position, hit.point)));
                        motor.Dash(data.SecondaryAttackDashSpeed, data.SecondaryAttackDashLength);
                        motor.blockMoving = false;
                        combat.inAction = false;
                    }));
                } else
                {
                    combat.inAction = false;
                }
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

    public bool isActorType(ActorType type)
    {
        return (type & actorType) != ActorType.None;
    }
}

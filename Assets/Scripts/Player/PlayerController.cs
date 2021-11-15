using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IActor, IState
{
    public LayerMask movementMask;
    public LayerMask actorMask;

    [HideInInspector]
    private Camera cam;

    PlayerMotor motor;
    PlayerManager playerManager;

    public float maxHealth = 100;
    private float health;

    // These attack moves are hard-coded and should be refactored.
    [Header("Combat Stuff that should have it's own class:")]
    public float attackDamage;
    public float attackCooldown;
    public float attackRange;
    public GameObject attackEffect;
    public CollisionObserver attackObserver;
    private float attackTime;

    public float AOEAttackDamage;
    public float AOEAttackCooldown;
    public float AOEAttackRange;
    public GameObject AOEAttackEffect;
    public CollisionObserver AOEObserver;
    private float AOEAttackTime;

    public ActorType type => ActorType.Player;
    //this was made to satisfy implementation of IActor, may need some refactoring
    private float speed;

    public float Speed { get => speed; set { speed = value; } }
    public float MaxHealth => maxHealth;
    public float Health
    {
        get => health; set
        {
            health = value;
            if (health <= 0)
            {
                SceneManager.LoadScene(1); //hard coded bad
            }
        }
    }

    private void Start()
    {
        playerManager = PlayerManager.instance;
        cam = playerManager.camera;
        health = maxHealth;
    }

    public void UpdateState()
    {
        //update cooldowns
        attackTime -= Time.deltaTime;
        AOEAttackTime -= Time.deltaTime;

        //return if the pointer 
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //left click
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //if we left click on the movement mask
            if (Physics.Raycast(ray, out hit, 1000, movementMask))
            {
                ClickToMove(hit);
            }
            //if we left click on the actor mask
            if (Physics.Raycast(ray, out hit, 1000, actorMask))
            {
                NormalAttack(hit);
            }
        }

        //right click
        if (Input.GetMouseButtonDown(1))
        {
            AOEAttack();
        }
    }

    private void ClickToMove(RaycastHit hit)
    {
        if (!playerManager.keyboardControl)
        {
            motor.MoveToPoint(hit.point);
        }
    }

    //normal attack
    private void NormalAttack(RaycastHit hit)
    {
        if (attackTime <= 0 && attackObserver.Stay.Contains(hit.collider))
        {
            IActor actor = hit.transform.GetComponent<IActor>();

            if (actor != null && actor.type == ActorType.Enemy)
            {
                attackTime = attackCooldown;
                actor.Health -= attackDamage;
                Instantiate(attackEffect).transform.position = hit.point;
            }
        }
    }

    //area attack
    private void AOEAttack()
    {
        if (AOEAttackTime < 0)
        {
            Collider[] collisions = AOEObserver.Stay.ToArray();
            AOEAttackTime = AOEAttackCooldown;

            Instantiate(AOEAttackEffect).transform.position = transform.position;

            for (int i = 0; i < collisions.Length; i++)
            {
                IActor actor = collisions[i].GetComponent<IActor>();

                if (actor == null || actor.type != ActorType.Enemy)
                    continue;

                actor.Health -= AOEAttackDamage;

                if (actor.gameObject == null)
                {
                    i--;
                }
            }
        }
    }

    //TODO: damage player, normal attack, enemy health ui, player health ui

    public void EnterState()
    {
        motor = GetComponent<PlayerMotor>();
        playerManager = PlayerManager.instance;
    }

    public void ExitState()
    {

    }

    public void LateUpdateState()
    {

    }
}

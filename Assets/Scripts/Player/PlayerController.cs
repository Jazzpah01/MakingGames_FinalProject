﻿using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IActor, IState
{
    // proper attack things

    public float maxVelocityChange = 10.0f;
    public LayerMask movementMask;
    public LayerMask actorMask;
    public Interactable focus;

    public Camera cam;
    PlayerMotor motor;
    GameController playerManager;
    NavMeshAgent navMeshAgent;

    private Vector3 velocity;
    private float directionX,directionZ;

    private float health;

    // These attack moves are hard-coded and should be refactored.
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

    public float Health { get => health; set => health = value; }

    void Start()
    {
        //cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        playerManager = GameController.instance;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void UpdateState()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        //left click
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                if (!playerManager.keyboardControl)
                {
                    motor.MoveToPoint(hit.point);
                }
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                } else
                {
                    RemoveFocus();
                }
            }
        }
        //right click
        attackTime -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hasHit = false;

            if (Physics.Raycast(ray, out hit, 1000, actorMask))
            {
                hasHit = true;
            } else if (Physics.Raycast(ray, out hit, 1000, movementMask))
            {
                //hasHit = true;
            }

            //normal attack
            if (hasHit && attackTime <= 0 && attackObserver.Stay.Contains(hit.collider))//(hit.transform.position - transform.position).magnitude <= attackRange)
            {
                IActor actor = hit.transform.GetComponent<IActor>();

                if (actor != null && actor.type == ActorType.Enemy)
                {
                    attackTime = attackCooldown;
                    actor.Health -= attackDamage;
                    Instantiate(attackEffect).transform.position = hit.transform.position;
                }
            }
        }
        //keyboard movement
        if (playerManager.keyboardControl)
        {
            directionX = 0;
            directionZ = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                directionZ = -1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                directionZ = 1;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                directionX = -1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                directionX = 1;
            }

            Vector3 targetVelocity = new Vector3(directionX, 0, directionZ);
            //targetVelocity = transform.TransformDirection(targetVelocity);
            targetVelocity *= navMeshAgent.speed;

            velocity = navMeshAgent.velocity;
            Vector3 velocityChange = targetVelocity;//(targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;

            navMeshAgent.velocity = velocityChange;
        }
        //area attack
        AOEAttackTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && AOEAttackTime < 0)
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

    private void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }
            focus = newFocus;
            motor.FollowTarget(newFocus);
        }
        newFocus.OnFocused(transform);
    }

    private void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnDefocused();
        }
        focus = null;
        motor.StopFollowingTarget();
    }

    public void EnterState()
    {
        cam.gameObject.SetActive(true);
    }

    public void ExitState()
    {
        cam.gameObject.SetActive(true);
    }

    public void LateUpdateState()
    {

    }
}

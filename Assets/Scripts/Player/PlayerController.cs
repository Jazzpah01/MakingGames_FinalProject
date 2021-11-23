using System;
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

    private void Start()
    {
        playerManager = PlayerManager.instance;
        gameController = GameController.instance;
        motor = GetComponent<PlayerMotor>();
        combat = GetComponent<PlayerCombat>();
        cam = playerManager.cam;
        health = maxHealth;
    }

    public void UpdateState()
    {
        combat.UpdateCooldowns();

        //return if the pointer 
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //left click
        if (Input.GetMouseButton(0))
        {
            combat.PrimaryAttack(cam, actorMask);
        }

        //right click
        if (Input.GetMouseButtonDown(1))
        {
            motor.AttackDash();
            combat.SecondaryAttack();
        }
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

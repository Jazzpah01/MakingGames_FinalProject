using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour, IActor
{
    GameController gameController;
    public float maxHealth = 500;
    private float health;

    public ActorType actorType => ActorType.Obstacle;

    private float speed = 0;
    public float Speed { get; set; }
    public float MaxHealth { get; set; }

    public float Health { get => health; 
        set
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
        health = maxHealth;
        gameController = GameManager.instance.gameController;
        blockDamage = false;
    }

    public bool isActorType(ActorType type)
    {
        return type.HasFlag(actorType);
    }
}
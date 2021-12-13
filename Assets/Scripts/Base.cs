using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour, IActor
{
    GameController gameController;
    public float health = 500;

    public ActorType actorType => ActorType.Obstacle;
    public float Speed { get; set; }
    public float MaxHealth { get; set; }

    public float Health { get => health; 
        set
        {
            health = value;

            if (health <= 0)
            {
                gameController.GameOver();
            }else if (health > MaxHealth)
            {
                health = MaxHealth;
                Debug.Log("Health flowover in " + gameObject.name);
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageModifyer { get; set; }
    public float speedModifyer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        gameController = GameManager.instance.gameController;
        blockDamage = false;
        MaxHealth = health;
        Health = MaxHealth;
    }

    public bool isActorType(ActorType type)
    {
        return type.HasFlag(actorType);
    }
}
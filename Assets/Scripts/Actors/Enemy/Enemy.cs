using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    public HealthBar healthBar;
    public GameObject deathEffect;
    public float deathEffectDestroyTimer = 1;

    private float health;

    public EnemyType enemyType { get; set; }

    public ActorType actorType => ActorType.Enemy;
    public float Speed { get; set; }
    public float MaxHealth { get; set; }

    public float Health
    {
        get => health;
        set
        {
            if (value <= 0)
            {
                Destroy(Instantiate(deathEffect, transform.position, Quaternion.identity), deathEffectDestroyTimer);
                Destroy(gameObject);
            }
            else if (value != health)
            {
                health = value;
                healthBar.SetHealthbar(value / MaxHealth);
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageModifyer { get; set; }

    public GameObject spawnPoint { get; set; }
    public float speedModifyer { get; set; }

    public bool isActorType(ActorType type)
    {
        return (type & actorType) != ActorType.None;
    }

    private void Start()
    {
        healthBar.SetHealthImageColour(Color.red);
    }
}
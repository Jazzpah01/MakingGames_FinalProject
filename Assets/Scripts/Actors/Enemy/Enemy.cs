using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    public HealthBar healthBar;
    public GameObject deathEffect;

    private float maxHealth;
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
            health = value;
            //TODO: implement hiteffects
            //PlayHitEffects();
            if (health <= 0)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else
            {
                healthBar.SetHealthbar(health / maxHealth);
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
}
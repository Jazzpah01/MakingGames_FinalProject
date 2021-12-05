using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    public HealthBar healthBar;
    public GameObject deathEffect;

    private float maxHealth;

    private NavMeshAgent agent;
    private float health;
    private float speed;

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
            healthBar.SetHealthbar(health / maxHealth);
            if (health <= 0)
            {
                Destroy(this.gameObject);
                Instantiate(deathEffect).transform.position = transform.position;
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageModifyer { get; set; }

    public GameObject spawnPoint { get; set; }
    public float speedModifyer { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isActorType(ActorType type)
    {
        return (type & actorType) != ActorType.None;
    }
}
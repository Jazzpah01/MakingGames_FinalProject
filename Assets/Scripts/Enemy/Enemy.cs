using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    public HealthBar healthBar;
    public GameObject deathEffect;

    public float maxHealth;

    private NavMeshAgent agent;
    private float health;
    public float speed;

    public EnemyType enemyType { get; set; }

    public ActorType type => ActorType.Enemy;

    public float Speed { get => speed; set => speed = value; }

    public float MaxHealth => MaxHealth;

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
    public float damageReduction { get; set; }

    public GameObject spawnPoint { get; set; }
    public float SpeedModifyer { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        blockDamage = false;
        Health = maxHealth;
        SpeedModifyer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
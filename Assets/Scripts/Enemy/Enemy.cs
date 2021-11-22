using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IEnemy
{
    public float maxHealth;

    private NavMeshAgent agent;
    private float health;

    public EnemyType enemyType { get; set; }

    public ActorType type => ActorType.Enemy;

    public float Speed { get => agent.speed; set => agent.speed = value; }

    public float MaxHealth => MaxHealth;

    public float Health { get => health;
        set
        {
            health = value;

            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
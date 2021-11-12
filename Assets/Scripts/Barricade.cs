using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barricade : MonoBehaviour, IActor
{
    public float maxHealth;
    public HealthBar healthbar;

    private float currentHealth;

    public ActorType type => ActorType.Obstacle;

    private float speed = 0;
    public float Speed { get => speed; set { speed = value;}}
    public float MaxHealth => maxHealth;
    public float Health { get => currentHealth; 
        set
        {
            currentHealth = value;
            healthbar.SetHealthbar(currentHealth/maxHealth);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

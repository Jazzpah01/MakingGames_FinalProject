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

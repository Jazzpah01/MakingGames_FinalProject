using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour, IActor
{
    public string buildingName;
    public float maxHealth;
    public float speed = 0;
    public HealthBar healthbar;
    public ProjectorController projectorController;
    public Light spotlight;

    private float currentHealth;

    public ActorType type => ActorType.Obstacle;

    public float Speed { get => speed; set { speed = value; } }
    public float MaxHealth => maxHealth;
    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            healthbar.SetHealthbar(currentHealth / maxHealth);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
    }

    private void Update()
    {
        
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}


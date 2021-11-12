using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour, IActor
{
    public float maxHealth = 500;
    private float health;

    public ActorType type => ActorType.Obstacle;

    private float speed = 0;
    public float Speed { get => speed; set { speed = value;}}
    public float MaxHealth => maxHealth;

    public float Health { get => health; 
        set
        {
            health = value;
            print(health);

            if (health <= 0)
            {
                SceneManager.LoadScene(1); //hard coded bad
            }
        }
    }

    private void Start()
    {
        health = maxHealth;
    }
}
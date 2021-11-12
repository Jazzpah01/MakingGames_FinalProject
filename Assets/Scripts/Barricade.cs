using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barricade : MonoBehaviour, IActor
{
    public float maxHealth = 100;
    public Image healthbar;

    private float currentHealth;

    public ActorType type => ActorType.Obstacle;

    private float speed = 0;
    public float Speed { get => speed; set { speed = value;}}
    public float MaxHealth => maxHealth;
    public float Health { get => currentHealth; 
        set
        {
            currentHealth = value;
            StartCoroutine(SmoothSliderDecrease(currentHealth / maxHealth, healthbar));
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

    IEnumerator SmoothSliderDecrease(float amount, Image image)
    {
        if (image != null)
            while (image.fillAmount > amount)
        {
            if(image != null)
            image.fillAmount -= 0.01f;

            yield return new WaitForSeconds(0.000001f);
        }
    }
}

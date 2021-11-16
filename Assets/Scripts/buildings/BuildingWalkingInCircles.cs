using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingWalkingInCircles : MonoBehaviour, IActor
{
    double StartPos;
    public float speed;
    private float rotation = 0.1f;
    public float damage;
    public CollisionObserver detectionCollision;
    public CollisionObserver damagerCollision;
    public Image healthbar;
    public float maxHealth = 100;
    private float currentHealth;

    public ActorType type => ActorType.Obstacle;
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

    public float Speed { get => speed;
        set {
            speed = value;
        }}


    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position.x;
        detectionCollision.Subscribe(Detection_Stay, CollisionObserver.CollisionType.Stay);
        detectionCollision.Subscribe(Detection_Exit, CollisionObserver.CollisionType.Exit);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        transform.Rotate(0.0f, rotation, 0.0f, Space.Self);
    }

    private void Detection_Stay(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;

        switch (actor.type)
        {
            case ActorType.Enemy:
                actor.Health -= damage;
                break;
        }
    }

    private void Detection_Exit(Collider other)
    {
        IActor actor = other.GetComponent<IActor>();
        if (actor == null)
            return;
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
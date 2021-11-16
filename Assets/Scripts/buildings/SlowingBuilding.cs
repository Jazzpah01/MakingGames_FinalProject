using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowingBuilding : MonoBehaviour, IActor
{

    float walktimer = 5;
    float gastimer = 0.2f;
    float yRotation;
    Vector3 gasPosition;

    public string buildingName;
    public GameObject slowGas;
    public HealthBar healthbar;
    public float maxHealth = 100;

    private float speed = 3.0f;
    private float currentHealth;
    public ActorType type => ActorType.Obstacle;
    public float MaxHealth => maxHealth;
    public float Health { get => currentHealth; 
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
    public float Speed { get => speed; set { speed = value;}}

    void Start()
    {
        yRotation = transform.rotation.y;
        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
    }

    void Update()
    {
        gasPosition = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        gastimer -= Time.deltaTime;
        if (walktimer > 0)
        {
            walktimer -= Time.deltaTime;
            transform.Translate(Vector3.forward * (Time.deltaTime * speed));
        } else {
            TurnAround();
        }

        if(gastimer < 0)
        {
            Instantiate(slowGas, gasPosition, Quaternion.identity);
            gastimer = 0.2f;
        }
    }

    void TurnAround(){

        transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
        yRotation = transform.rotation.y;
        walktimer = 5;
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

    private void Die()
    {
        Destroy(gameObject);
    }
}

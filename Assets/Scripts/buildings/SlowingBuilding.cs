using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlowingBuilding : MonoBehaviour, IActor
{

    public ActorType type => ActorType.Obstacle;
    public GameObject slowGas;
    public Image healthbar;
    float walktimer = 5;
    float gastimer = 0.2f;
    Vector3 gasPosition;
    public float maxHealth = 100;
    private float currentHealth;
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
    private float speed = 3.0f;
    public float Speed { get => speed; set { speed = value;}}

    float yRotation;



    // Start is called before the first frame update
    void Start()
    {
        yRotation = transform.rotation.y;
    }

    // Update is called once per frame
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

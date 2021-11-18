using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingResource : MonoBehaviour, IActor
{
    public string buildingName;
    public float health;
    public float growthTime = 2.0f;
    public float harvestTimer = 5.0f;
    public HealthBar healthbar;

    private float timer;
    private int growthStage;
    private int maxResourcesGained = 5;
    private int currentResourcesGained;
    private Vector3 growth = new Vector3(0.1f,0.1f,0.1f);
    public ActorType type => ActorType.Obstacle;
    public float Speed { get => Speed; set => Speed = 0; }
    public float MaxHealth => 100;
    public float Health { get => health; set => health = value; }
    

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.inBattle == true)
        {
            timer += Time.deltaTime;
            checkHealth();
            checkGrowth();
            checkFullyGrown();
        }
    }

    void checkGrowth()
    {
        if(timer > growthTime && growthStage < 10)
        {
            Debug.Log("It grew");
            growthStage++;
            this.gameObject.transform.GetChild(1).transform.localScale += growth;
            timer = 0;
        }
    }

    void checkFullyGrown()
    {
        if(growthStage == 10 && timer > harvestTimer)
        {
            calculateResourcesGained();
            GameManager.instance.resource += currentResourcesGained;
            Destroy(this.gameObject);
        }
    }

    void harvestManual()
    {
        if(growthStage == 1);
    }

    void calculateResourcesGained()
    {
        float percentageLifeRemaining = (health/MaxHealth);
        int resourceValue = (int)(maxResourcesGained * percentageLifeRemaining);
        currentResourcesGained = resourceValue;
    }

    void checkHealth()
    {
        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

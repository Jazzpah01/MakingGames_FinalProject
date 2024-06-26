using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingResource : MonoBehaviour, IActor
{
    public string buildingName;
    public float health;
    public float growthTime = 0.5f;
    public float harvestTimer = 0.5f;
    public int maxResourcesGained = 5;
    public HealthBar healthbar;
    private Vector3 growth = new Vector3(0.1f,0.1f,0.1f);
    public ActorType type => ActorType.Obstacle;
    public float Speed { get => Speed; set => Speed = 0; }
    public float MaxHealth => 100;
    public float Health { get => health; set => health = value; }
    
    private float timer;
    private int growthStage;
    private int currentResourcesGained;
    private GameObject model;

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
        foreach(Transform child in transform)
        {
            if(child.name == "Model")
            {
                model = child.gameObject;
            }
        }
        
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
            growthStage++;
            model.transform.localScale += growth;
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

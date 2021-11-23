using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingResource : MonoBehaviour
{
    public float growthTime = 0.5f;
    public float harvestTimer = 0.5f;
    public int maxResourcesGained = 5;

    public string buildingName;
    public float health;
    public int maxResourcesGained;

    private Vector3 growth = new Vector3(0.1f,0.1f,0.1f);
    public int additionalWavesBeforeHarvest;
    private int currentResourcesGained;
    private int plantedWave;
    private GameManager gameManager;
    private Buildable buildable;
    private GameObject model;
    private float health, maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        buildable = GetComponent<Buildable>();
        maxHealth = buildable.MaxHealth;
        gameManager = GameManager.instance;
        plantedWave = gameManager.gameController.getNextWave();
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
        health = buildable.Health;
        if(gameManager.inBattle == false && plantedWave + additionalWavesBeforeHarvest < gameManager.gameController.getNextWave())
        {
            harvest();
        }
        /*
        if(GameManager.instance.inBattle == true)
        {
            timer += Time.deltaTime;
            checkHealth();
            checkGrowth();
            checkFullyGrown();
        }*/
    }

    void harvest()
    {
            calculateResourcesGained();
            gameManager.resource += currentResourcesGained;
            Destroy(this.gameObject);
    }
/*
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
    }*/

    void calculateResourcesGained()
    {
        float percentageLifeRemaining = (health/maxHealth);
        int resourceValue = (int)(maxResourcesGained * percentageLifeRemaining);
        currentResourcesGained = resourceValue;
    }
}

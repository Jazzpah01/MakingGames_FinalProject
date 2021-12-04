using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingResource : MonoBehaviour, IBuildingBehavior
{
    public float growthTime = 0.5f;
    public float harvestTimer = 0.5f;

    private Vector3 growth = new Vector3(0.1f,0.1f,0.1f);

    private int currentResourcesGained;
    private int plantedWave;
    private GameManager gameManager;
    private GameObject model;

    private Buildable buildable;
    private ResourceBuildingData data;

    // Start is called before the first frame update
    void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (ResourceBuildingData) buildable.data;

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
        if(gameManager.gameController.state == GameController.GameState.Strategy && 
            plantedWave + data.wavesToHarvest < gameManager.gameController.getNextWave())
        {
            harvest();
        }
    }

    void harvest()
    {
            calculateResourcesGained();
            gameManager.currentResource += currentResourcesGained;
            Destroy(this.gameObject);
    }

    void calculateResourcesGained()
    {
        float percentageLifeRemaining = (buildable.Health/buildable.MaxHealth);
        int resourceValue = (int)(data.resourceGain * percentageLifeRemaining);
        currentResourcesGained = resourceValue;
    }
}

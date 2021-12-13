using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingResource : MonoBehaviour, IBuildingBehavior
{
    public float growthTime = 0.5f;
    public float harvestTimer = 0.5f;

    private int plantedWave;
    private GameManager gameManager;

    private Buildable buildable;
    private ResourceBuildingData data;
    private string text;

    // Start is called before the first frame update
    void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (ResourceBuildingData) buildable.data;

        gameManager = GameManager.instance;
        plantedWave = gameManager.gameController.currentWave;

    }

    // Update is called once per frame
    void Update()
    {
        text = calculateResourcesGained() + " nectar in " + (data.wavesToHarvest - (gameManager.gameController.currentWave - plantedWave)) + " waves";
        buildable.healthbar.textBox.text = text;
        if(gameManager.gameController.state == GameController.GameState.Strategy && 
            plantedWave + data.wavesToHarvest <= gameManager.gameController.currentWave)
        {
            harvest();
        }
    }

    void harvest()
    {
            gameManager.currentResource += calculateResourcesGained();
            Destroy(gameObject);
    }

    public int calculateResourcesGained()
    {
        float percentageLifeRemaining = (buildable.Health/buildable.MaxHealth);
        int resourceValue = (int)(data.resourceGain * percentageLifeRemaining);
        return resourceValue;
    }
}

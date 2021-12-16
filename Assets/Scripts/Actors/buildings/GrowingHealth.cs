using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingHealth : MonoBehaviour, IBuildingBehavior
{

    private int plantedWave;
    private GameManager gameManager;

    private Buildable buildable;
    private HealthBuildingData data;
    private string text;
    private void Start()
    {
        buildable = GetComponent<Buildable>();
        data = (HealthBuildingData) buildable.data;

        gameManager = GameManager.instance;
        plantedWave = gameManager.gameController.currentWave;

    }

    private void Update()
    {
        text = calculatePlayerHealthGained() + "% health in " + (data.wavesToHarvest - (gameManager.gameController.currentWave - plantedWave)) + " waves";
        buildable.healthbar.textBox.text = text;
        if(gameManager.gameController.state == GameController.GameState.Strategy && 
            plantedWave + data.wavesToHarvest <= gameManager.gameController.currentWave)
        {
            harvest();
        }
    }

    private void harvest()
    {
        PlayerManager.instance.player.GetComponent<PlayerController>().Health += PlayerManager.instance.player.GetComponent<PlayerController>().MaxHealth * calculatePlayerHealthGained() * 0.01f; ;
        gameManager.baseController.Health += gameManager.baseController.MaxHealth * calculateBaseHealthGained() * 0.01f;
        Destroy(gameObject);
    }

    public int calculateBaseHealthGained()
    {
        float percentageLifeRemaining = (buildable.Health/buildable.MaxHealth);
        int resourceValue = (int)(data.baseHealthGain * percentageLifeRemaining);
        return resourceValue;
    }

    public int calculatePlayerHealthGained()
    {
        float percentageLifeRemaining = (buildable.Health / buildable.MaxHealth);
        int resourceValue = (int)(data.playerHealthGain * percentageLifeRemaining);
        return resourceValue;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    public GameObject strategyHUDPrefab;
    public GameObject levelDescriptionPrefab;

    private PlayerHUD playerHUD;
    private StrategyHUD strategyHUD;
    private GameObject levelDescription;

    GameManager gameManager;

    private void Start()
    {
        //Instantiate the HUD Prefabs
        playerHUD = Instantiate(playerHUDPrefab,transform).GetComponent<PlayerHUD>();
        strategyHUD = Instantiate(strategyHUDPrefab,transform).GetComponent<StrategyHUD>();
        levelDescription = Instantiate(levelDescriptionPrefab, transform);

        gameManager = GameManager.instance;

        levelDescription.SetActive(false);
        playerHUD.gameObject.SetActive(false);
    }
    public void UpdateHUD()
    {
        strategyHUD.gameObject.SetActive(gameManager.gameController.state == GameController.GameState.Strategy);
        playerHUD.gameObject.SetActive(gameManager.gameController.state == GameController.GameState.Combat);
    }
}
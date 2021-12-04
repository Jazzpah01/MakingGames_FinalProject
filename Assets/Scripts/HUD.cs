using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    public GameObject strategyHUDPrefab;
    public GameObject levelDescriptionPrefab;
    public GameObject pauseMenuPrefab;

    private PlayerHUD playerHUD;
    private StrategyHUD strategyHUD;
    private GameObject levelDescription;
    private GameObject pauseMenu;
    private bool battle;

    GameManager gameManager;

    private void Start()
    {
        //Instantiate the HUD Prefabs
        playerHUD = Instantiate(playerHUDPrefab,transform).GetComponent<PlayerHUD>();
        strategyHUD = Instantiate(strategyHUDPrefab,transform).GetComponent<StrategyHUD>();
        levelDescription = Instantiate(levelDescriptionPrefab, transform);
        pauseMenu = Instantiate(pauseMenuPrefab, transform);

        gameManager = GameManager.instance;

        levelDescription.SetActive(false);
        pauseMenu.SetActive(false);

        UpdateHUD();
    }
    private void Update()
    {
        if (gameManager.inBattle != battle)
        {
            battle = gameManager.inBattle;
            UpdateHUD();
        }
    }

    private void UpdateHUD()
    {
        strategyHUD.gameObject.SetActive(!gameManager.inBattle);
        playerHUD.gameObject.SetActive(gameManager.inBattle);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    public GameObject strategyHUDPrefab;
    public GameObject startMenuPrefab;
    public GameObject levelDescriptionPrefab;
    public GameObject ingameMenuPrefab;
    public GameObject controlsPrefab;
    public GameObject settingsPrefab;
    public GameObject creditsPrefab;
    public GameObject gameoverPrefab;

    [HideInInspector]
    public bool isLevelDescriptionActive;

    private PlayerHUD playerHUD;
    private StrategyHUD strategyHUD;
    private GameObject startMenuUI;
    private GameObject levelDescriptionUI;
    private GameObject ingameMenu;
    private GameObject controlsUI;
    private GameObject settingsUI;
    private GameObject creditsUI;
    private GameObject gameOverUI;

    private bool gameStarted;

    [HideInInspector]
    public bool mouseOver; 

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;

        //Instantiate the HUD Prefabs
        playerHUD = Instantiate(playerHUDPrefab,transform).GetComponent<PlayerHUD>();
        strategyHUD = Instantiate(strategyHUDPrefab,transform).GetComponent<StrategyHUD>();
        startMenuUI = Instantiate(startMenuPrefab, transform);
        levelDescriptionUI = Instantiate(levelDescriptionPrefab, transform);
        ingameMenu = Instantiate(ingameMenuPrefab, transform);
        controlsUI = Instantiate(controlsPrefab, transform);
        settingsUI = Instantiate(settingsPrefab, transform);
        creditsUI = Instantiate(creditsPrefab, transform);
        gameOverUI = Instantiate(gameoverPrefab, transform);

        strategyHUD.gameObject.SetActive(false);
        levelDescriptionUI.gameObject.SetActive(false);
        playerHUD.gameObject.SetActive(false);
        ingameMenu.gameObject.SetActive(false);
        controlsUI.gameObject.SetActive(false);
        settingsUI.gameObject.SetActive(false);
        creditsUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }
    public void UpdateHUD()
    {
        strategyHUD.gameObject.SetActive(gameManager.gameController.state == GameController.GameState.Strategy);
        playerHUD.gameObject.SetActive(gameManager.gameController.state == GameController.GameState.Combat);
    }

    public void GameOver()
    {
        gameOverUI.gameObject.SetActive(true);
    }

    public void IngameMenuButton()
    {
        ingameMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void IngameMenuResumeButton()
    {
        ingameMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void IngameMenuControlsButton()
    {
        ingameMenu.SetActive(false);
        controlsUI.SetActive(true);
    }
    public void IngameMenuSettingsButton()
    {
        ingameMenu.SetActive(false);
        settingsUI.SetActive(true);
    }
    public void IngameMenuCreditsButton()
    {
        ingameMenu.SetActive(false);
        creditsUI.SetActive(true);
    }
    public void IngameMenuStartMenuButton()
    {
        ingameMenu.SetActive(false);
        strategyHUD.gameObject.SetActive(false);
        startMenuUI.SetActive(true);
        gameStarted = false;
    }
    public void ControlsBackButton()
    {
        controlsUI.SetActive(false);
        if (gameStarted)
            ingameMenu.SetActive(true);
        else
            startMenuUI.SetActive(true);
    }
    public void SettingsBackButton()
    {
        settingsUI.SetActive(false);
        ingameMenu.SetActive(true);
    }
    public void CreditsBackButton()
    {
        creditsUI.SetActive(false);
        if (gameStarted)
            ingameMenu.SetActive(true);
        else
            startMenuUI.SetActive(true);
    }

    public void StartMenuStartButton()
    {
        Time.timeScale = 1;
        gameStarted = true;
        levelDescriptionUI.SetActive(true);
        strategyHUD.gameObject.SetActive(true);
        startMenuUI.SetActive(false);
    }

    public void StartMenuControlsButton()
    {
        controlsUI.SetActive(true);
        startMenuUI.SetActive(false);
    }

    public void StartMenuCreditsButton()
    {
        creditsUI.SetActive(true);
        startMenuUI.SetActive(false);
    }

    public void GameOverMenuRetry()
    {
        LevelManager.instance.RestartLevel();
    }

    public void GameOverMenuMainMenu()
    {
        LevelManager.instance.RestartAllLevels();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
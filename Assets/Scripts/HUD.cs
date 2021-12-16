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
    [HideInInspector]
    public bool isStartMenuActive;

    private PlayerHUD playerHUD;
    private StrategyHUD strategyHUD;
    private GameObject startMenuUI;
    protected GameObject levelDescriptionUI;
    private GameObject ingameMenuUI;
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
        playerHUD = Instantiate(playerHUDPrefab, transform).GetComponent<PlayerHUD>();
        strategyHUD = Instantiate(strategyHUDPrefab, transform).GetComponent<StrategyHUD>();
        startMenuUI = Instantiate(startMenuPrefab, transform);
        levelDescriptionUI = Instantiate(levelDescriptionPrefab, transform);
        ingameMenuUI = Instantiate(ingameMenuPrefab, transform);
        controlsUI = Instantiate(controlsPrefab, transform);
        settingsUI = Instantiate(settingsPrefab, transform);
        creditsUI = Instantiate(creditsPrefab, transform);
        gameOverUI = Instantiate(gameoverPrefab, transform);

        playerHUD.gameObject.SetActive(false);
        startMenuUI.SetActive(false);
        isStartMenuActive = false;
        ingameMenuUI.SetActive(false);
        controlsUI.SetActive(false);
        settingsUI.SetActive(false);
        creditsUI.SetActive(false);
        gameOverUI.SetActive(false);
        isLevelDescriptionActive = true;

        if (LevelManager.instance.getCurrentLevel() == 1)
        {
            isStartMenuActive = true;
            startMenuUI.SetActive(true);
            levelDescriptionUI.SetActive(false);
            isLevelDescriptionActive = false;
            strategyHUD.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) 
            && strategyHUD.gameObject.activeSelf 
            && !playerHUD.gameObject.activeSelf 
            && !startMenuUI.activeSelf 
            && !levelDescriptionUI.activeSelf 
            && !ingameMenuUI.activeSelf
            && !controlsUI.activeSelf
            && !settingsUI.activeSelf
            && !creditsUI.activeSelf
            && !gameOverUI.activeSelf)
        {
            IngameMenuButton();
        }
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
        ingameMenuUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void IngameMenuResumeButton()
    {
        ingameMenuUI.SetActive(false);
        Time.timeScale = 1;
    }
    public void IngameMenuControlsButton()
    {
        ingameMenuUI.SetActive(false);
        controlsUI.SetActive(true);
    }
    public void IngameMenuSettingsButton()
    {
        ingameMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }
    public void IngameMenuCreditsButton()
    {
        ingameMenuUI.SetActive(false);
        creditsUI.SetActive(true);
    }
    public void IngameMenuStartMenuButton()
    {
        LevelManager.instance.RestartLevel();
    }
    public void ControlsBackButton()
    {
        controlsUI.SetActive(false);
        if (gameStarted)
        {
            ingameMenuUI.SetActive(true);
        }
        else
        {
        startMenuUI.SetActive(true);
        isStartMenuActive = true;
        }
    }
    public void SettingsBackButton()
    {
        settingsUI.SetActive(false);
        ingameMenuUI.SetActive(true);
    }
    public void CreditsBackButton()
    {
        creditsUI.SetActive(false);
        if (gameStarted)
        {
            ingameMenuUI.SetActive(true);
        }
        else
        {
            startMenuUI.SetActive(true);
            isStartMenuActive = true;
        }
    }

    public void StartMenuStartButton()
    {
        Time.timeScale = 1;
        gameStarted = true;
        levelDescriptionUI.SetActive(true);
        isLevelDescriptionActive = true;
        strategyHUD.gameObject.SetActive(true);
        startMenuUI.SetActive(false);
        isStartMenuActive = false;
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

    public void CloseLevelDescription()
    {
        levelDescriptionUI.SetActive(false);
        isLevelDescriptionActive = false;
    }
}
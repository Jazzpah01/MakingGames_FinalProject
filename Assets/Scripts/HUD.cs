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
    public GameObject telemetryPromptPrefab;
    

    private PlayerHUD playerHUD;
    private StrategyHUD strategyHUD;
    [HideInInspector]
    public GameObject startMenuUI;
    [HideInInspector]
    public GameObject levelDescriptionUI;
    private GameObject ingameMenuUI;
    private GameObject controlsUI;
    private GameObject settingsUI;
    private GameObject creditsUI;
    private GameObject gameOverUI;
    private GameObject telemetryPrompt;

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
        telemetryPrompt = Instantiate(telemetryPromptPrefab, transform);

        playerHUD.gameObject.SetActive(false);
        startMenuUI.SetActive(false);
        ingameMenuUI.SetActive(false);
        controlsUI.SetActive(false);
        settingsUI.SetActive(false);
        creditsUI.SetActive(false);
        gameOverUI.SetActive(false);
        telemetryPrompt.SetActive(false);

        if (!TelemetryPrompt.choiseMade)
        {
            StartTelemetryPrompt();
        } else
        {
            StartMenuSetup();
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

    public void StartMenuSetup()
    {
        if (LevelManager.instance.getCurrentLevel() == 1)
        {
            startMenuUI.SetActive(true);
            levelDescriptionUI.SetActive(false);
            strategyHUD.gameObject.SetActive(false);
            telemetryPrompt.SetActive(false);
        }
    }

    public void StartTelemetryPrompt()
    {
        startMenuUI.SetActive(false);
        levelDescriptionUI.SetActive(false);
        strategyHUD.gameObject.SetActive(false);
        telemetryPrompt.SetActive(true);
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
    public void IngameMenuSettingsButton()
    {
        ingameMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }
    public void IngameMenuQuitButton()
    {
        if (SessionManager.instance == null)
        {
            if (GameEvents.GameClosed != null)
                GameEvents.GameClosed();
            LevelManager.instance.RestartAllLevels();
        } else
        {
            if (GameEvents.GameClosed != null)
                GameEvents.GameClosed();
            Application.OpenURL(
                $"https://docs.google.com/forms/d/e/1FAIpQLSfV0Pjggpm2oLKwJRBTGYpNwiC614W3wKjGU-kLUDW_L8TN0g/viewform?usp=pp_url&entry.512054640={SessionManager.instance.playerId}&entry.1864559782={SessionManager.gameVersion}");
            LevelManager.instance.RestartAllLevels();
        }
    }
    public void IngameMenuRestartMenuButton()
    {
        LevelManager.instance.RestartLevel();
    }
    public void ControlsBackButton()
    {
        controlsUI.SetActive(false);
        startMenuUI.SetActive(true);        
    }
    public void SettingsBackButton()
    {
        settingsUI.SetActive(false);
        ingameMenuUI.SetActive(true);
    }
    public void CreditsBackButton()
    {
        creditsUI.SetActive(false);
        startMenuUI.SetActive(true);        
    }

    public void StartMenuStartButton()
    {
        Time.timeScale = 1;
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
        if (GameEvents.GameClosed != null)
            GameEvents.GameClosed();
        if (SessionManager.instance != null)
        {
            Application.OpenURL(
                    $"https://docs.google.com/forms/d/e/1FAIpQLSfV0Pjggpm2oLKwJRBTGYpNwiC614W3wKjGU-kLUDW_L8TN0g/viewform?usp=pp_url&entry.512054640={SessionManager.instance.playerId}&entry.1864559782={SessionManager.gameVersion}");
        }
        LevelManager.instance.RestartAllLevels();
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CloseLevelDescription()
    {
        levelDescriptionUI.SetActive(false);
    }
}
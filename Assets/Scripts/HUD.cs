using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject playerHUDPrefab;
    public GameObject strategyHUDPrefab;
    public GameObject levelDescriptionPrefab;
    public GameObject ingameMenuPrefab;
    public GameObject controlsPrefab;
    public GameObject settingsPrefab;
    public GameObject creditsPrefab;

    private PlayerHUD playerHUD;
    private StrategyHUD strategyHUD;
    private GameObject levelDescription;
    private GameObject ingameMenu;
    private GameObject controlsUI;
    private GameObject settingsUI;
    private GameObject creditsUI;

    [HideInInspector]
    public bool mouseOver; 

    GameManager gameManager;

    private void Start()
    {
        //Instantiate the HUD Prefabs
        playerHUD = Instantiate(playerHUDPrefab,transform).GetComponent<PlayerHUD>();
        strategyHUD = Instantiate(strategyHUDPrefab,transform).GetComponent<StrategyHUD>();
        levelDescription = Instantiate(levelDescriptionPrefab, transform);
        ingameMenu = Instantiate(ingameMenuPrefab, transform);
        controlsUI = Instantiate(controlsPrefab, transform);
        settingsUI = Instantiate(settingsPrefab, transform);
        creditsUI = Instantiate(creditsPrefab, transform);

        gameManager = GameManager.instance;

        playerHUD.gameObject.SetActive(false);
        ingameMenu.gameObject.SetActive(false);
        controlsUI.gameObject.SetActive(false);
        settingsUI.gameObject.SetActive(false);
        creditsUI.gameObject.SetActive(false);
    }
    public void UpdateHUD()
    {
        strategyHUD.gameObject.SetActive(gameManager.gameController.state == GameController.GameState.Strategy);
        playerHUD.gameObject.SetActive(gameManager.gameController.state == GameController.GameState.Combat);
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
    public void ControlsBackButton()
    {
        controlsUI.SetActive(false);
        ingameMenu.SetActive(true);
    }
    public void SettingsBackButton()
    {
        settingsUI.SetActive(false);
        ingameMenu.SetActive(true);
    }
    public void CreditsBackButton()
    {
        creditsUI.SetActive(false);
        ingameMenu.SetActive(true);
    }
}
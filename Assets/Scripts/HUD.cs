using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public PlayerHUD playerHUD;
    public StrategyHUD strategyHUD;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }
    private void Update()
    {
        strategyHUD.gameObject.SetActive(!gameManager.inBattle);
        playerHUD.gameObject.SetActive(gameManager.inBattle);
    }

}
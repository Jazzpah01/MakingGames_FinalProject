using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public PlayerHUD playerHUD;
    public StrategyHUD strategyHUD;

    [HideInInspector] PlayerController playerController;
    [HideInInspector] StrategyController strategyController;

    private void Start()
    {
        strategyController = GameController.instance.strategyController;
        playerController = GameController.instance.player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Disable strategy hud, if in combat mode
        if (GameController.instance.state == GameController.GameState.Strategy)
        {
            strategyHUD.gameObject.SetActive(true);
        } else
        {
            strategyHUD.gameObject.SetActive(false);
        }
    }
}
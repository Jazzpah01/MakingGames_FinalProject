using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public StrategyController strategyController;
    [HideInInspector] public Base baseS;

    public Slider playerHealthBar;
    public Slider baseHealthBar;
    public TextMeshProUGUI resourceCounter;

    private void Start()
    {
        strategyController = GameController.instance.strategyController;
        playerController = GameController.instance.player.GetComponent<PlayerController>();
        baseS = GameController.instance.baseController;
    }

    private void Update()
    {
        // Update player values on the hud
        playerHealthBar.value = playerController.Health / playerController.MaxHealth;
        baseHealthBar.value = baseS.Health / baseS.MaxHealth;

        //resourceCounter.text = "Nectar Essence: " + strategyController.resource.ToString();
    }
}
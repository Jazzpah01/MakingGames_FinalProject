using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public PlayerController playerController;
    public StrategyController strategyController;
    public Base baseS;

    public Slider playerHealthBar;
    public Slider baseHealthBar;
    public TextMeshProUGUI resourceCounter;

    private void Update()
    {
        playerHealthBar.value = playerController.Health / playerController.MaxHealth;
        baseHealthBar.value = baseS.Health / baseS.MaxHealth;

        resourceCounter.text = strategyController.resource.ToString();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    PlayerController playerController;
    GameManager gameManager;

    public Base baseController;
    public Slider playerHealthBar;
    public Slider baseHealthBar;
    public TextMeshProUGUI resourceCounter;

    private void Start()
    {
        playerController = PlayerManager.instance.playerController;
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        // Update player values on the hud
        playerHealthBar.value = playerController.Health / playerController.MaxHealth;
        baseHealthBar.value = baseController.Health / baseController.MaxHealth;

        resourceCounter.text = gameManager.resource.ToString();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    PlayerController playerController;
    GameManager gameManager;

    private Base baseController;
    public Slider playerHealthBar;
    public Slider baseHealthBar;
    public Image baseHealthFill;
    public Image playerHealthFill;
    public TextMeshProUGUI resourceCounter;
    public float damageIndicatorTimer;

    private float oldBaseHealth;
    private float oldPlayerHealth;

    private void Start()
    {
        playerController = PlayerManager.instance.playerController;
        gameManager = GameManager.instance;
        baseController = GameController.instance.baseController;
        oldPlayerHealth = playerController.Health;
        oldBaseHealth = baseController.Health;
    }

    private void Update()
    {
        
        // Update player values on the hud
        if(oldPlayerHealth > playerController.Health)
        {
            StartCoroutine(playerHealthBarDamageIndicator());
        }
        playerHealthBar.value = playerController.Health / playerController.MaxHealth;
        if(oldBaseHealth > baseController.Health)
        {
            StartCoroutine(baseHealthBarDamageIndicator());
        }
        baseHealthBar.value = baseController.Health / baseController.MaxHealth;

        resourceCounter.text = gameManager.currentResource.ToString();

        oldBaseHealth = baseController.Health;
        oldPlayerHealth = playerController.Health;
    }


    
    IEnumerator baseHealthBarDamageIndicator()
    {
        baseHealthFill.color = new Color32(255,0,0,255);
        yield return new WaitForSeconds(damageIndicatorTimer);
        baseHealthFill.color = new Color32(133,160,39,255);
    }

    IEnumerator playerHealthBarDamageIndicator()
    {
        playerHealthFill.color = new Color32(255,0,0,255);
        yield return new WaitForSeconds(damageIndicatorTimer);
        playerHealthFill.color = new Color32(133,160,39,255);
    }
}
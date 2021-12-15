using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    PlayerController playerController;
    GameController gameController;
    GameManager gameManager;
    Base baseController;

    public TextMeshProUGUI resourceCounter;
    public TextMeshProUGUI waveCounter;
    public Image playerHealthFill;
    public Image baseHealthFill;
    public float damageFlashDelay;
    public Image playerHealthGlow;
    public Image baseHealthGlow;
    [Range(0, 100)]
    [Header("The sum of all % damage taken to trigger glow effect (0-100% health)")]
    public float playerTriggerGlowAmount;
    [Range(0, 100)]
    public float baseTriggerGlowAmount;
    public float glowSpeedDelay;
    public float glowCount;

    private float oldPlayerHealth;
    private float oldBaseHealth;
    private float playerDamageTaken;
    private float baseDamageTaken;
    private bool playerFlashCoroutine;
    private bool baseFlashCoroutine;
    private bool playerGlowCoroutine;
    private bool baseGlowCoroutine;
    private float playerGlowQueue;
    private float baseGlowQueue;
    private Coroutine playerFCoroutine;
    private Coroutine baseFCoroutine;
    private Coroutine playerGCoroutine;
    private Coroutine baseGCoroutine;
    private Color basic = new Color(255,255,255,255);

    private void Start()
    {
        playerController = PlayerManager.instance.playerController;
        gameManager = GameManager.instance;
        gameController = GameController.instance;
        baseController = GameController.instance.baseController;
        oldPlayerHealth = playerController.Health;
        oldBaseHealth = baseController.Health;
        Color cp = playerHealthGlow.color;
        cp.a = 0;
        playerHealthGlow.color = cp;
        Color cb = baseHealthGlow.color;
        cb.a = 0;
        baseHealthGlow.color = cb;
        playerHealthFill.fillAmount = playerController.Health / playerController.MaxHealth;
        baseHealthFill.fillAmount = baseController.Health / baseController.MaxHealth;
    }

    private void OnEnable()
    {
        Color cp = playerHealthGlow.color;
        cp.a = 0;
        playerHealthGlow.color = cp;
        Color cb = baseHealthGlow.color;
        cb.a = 0;
        baseHealthGlow.color = cb;

        playerHealthFill.color = basic;
        baseHealthFill.color = basic;
    }

    private void Update()
    {
        // Update player UI
        playerHealthFill.fillAmount = playerController.Health / playerController.MaxHealth;
        if (oldPlayerHealth > playerController.Health)
        {
            if (!playerFlashCoroutine)
            {
                playerFlashCoroutine = true;
                playerFCoroutine = StartCoroutine(HealthBarFlashIndicator(playerHealthFill, false));
            }
            playerDamageTaken += (oldPlayerHealth - playerController.Health) / playerController.MaxHealth;
            if (playerDamageTaken >= playerTriggerGlowAmount / 100)
            {
                if (!playerGlowCoroutine)
                {
                    playerGlowCoroutine = true;
                    playerGCoroutine = StartCoroutine(HealthBarGlowIndicator(playerHealthGlow, false));
                }
                else
                {
                    playerGlowQueue++;
                }
            }
        }

        //Update base UI
        baseHealthFill.fillAmount = baseController.Health / baseController.MaxHealth;
        if (oldBaseHealth > baseController.Health)
        {
            if (!baseFlashCoroutine)
            {
                baseFlashCoroutine = true;
                baseFCoroutine = StartCoroutine(HealthBarFlashIndicator(baseHealthFill, true));
            }
            baseDamageTaken += (oldBaseHealth - baseController.Health) / baseController.MaxHealth;
            if (baseDamageTaken >= baseTriggerGlowAmount / 100)
            {
                if (!baseGlowCoroutine)
                {
                    baseGlowCoroutine = true;
                    baseGCoroutine = StartCoroutine(HealthBarGlowIndicator(baseHealthGlow, true));
                }
                else
                {
                    baseGlowQueue++;
                }
            }
        }

        if (!playerGlowCoroutine && (playerGlowQueue > 0 || playerHealthFill.fillAmount < 0.2f))
        {
            playerGlowQueue--;
            playerGlowCoroutine = true;
            playerGCoroutine = StartCoroutine(HealthBarGlowIndicator(playerHealthGlow, false));
        }
        if (!baseGlowCoroutine && (baseGlowQueue > 0 || baseHealthFill.fillAmount < 0.2f))
        {
            baseGlowQueue--;
            baseGlowCoroutine = true;
            baseGCoroutine = StartCoroutine(HealthBarGlowIndicator(baseHealthGlow, true));
        }

        if (playerDamageTaken > 0 && playerDamageTaken < playerTriggerGlowAmount / 100)
        {
            playerDamageTaken -= Time.deltaTime * 0.01f;
        }
        else if (playerDamageTaken > playerTriggerGlowAmount / 100)
        {
            playerDamageTaken -= Time.deltaTime * 0.1f;
        }

        if (baseDamageTaken > 0 && baseDamageTaken < baseTriggerGlowAmount / 100)
        {
            baseDamageTaken -= Time.deltaTime * 0.01f;
        }
        else if (baseDamageTaken > baseTriggerGlowAmount / 100)
        {
            baseDamageTaken -= Time.deltaTime * 0.1f;
        }

        //resource text update
        resourceCounter.text = "Nectar Essence: " + gameManager.currentResource.ToString();

        //wave text update
        waveCounter.text = "Wave\n" +  (gameController.currentWave + 1).ToString() + " of " + FindObjectOfType<SpawnController>().waves.Count.ToString();

        oldPlayerHealth = playerController.Health;
        oldBaseHealth = baseController.Health;
    }

    IEnumerator HealthBarFlashIndicator(Image healthbar, bool flagFalseIsPlayer)
    {
        Color c = healthbar.color;
        healthbar.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(damageFlashDelay);
        healthbar.color = c;
        if (flagFalseIsPlayer)
        {
            baseFlashCoroutine = false;
        }
        else
        {
            playerFlashCoroutine = false;
        }
    }

    IEnumerator HealthBarGlowIndicator(Image glow, bool flagFalseIsPlayer)
    {
        Color c = glow.color;
        bool direction = true;
        float count = glowCount * 2;
        while (true)
        {
            if (direction)
                c.a += 0.07f;
            else
                c.a -= 0.07f;

            glow.color = c;

            if (glow.color.a >= 1 || glow.color.a <= 0)
            {
                direction = !direction;
                count--;
            }
            if (count <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(glowSpeedDelay);
        }
        c.a = 0;
        glow.color = c;
        if (flagFalseIsPlayer)
        {
            baseGlowCoroutine = false;
        }
        else
        {
            playerGlowCoroutine = false;
        }
    }
    private void OnDisable()
    {
        if (playerFCoroutine != null)
        {
            StopCoroutine(playerFCoroutine);
        }
        if (baseFCoroutine != null)
        {
            StopCoroutine(baseFCoroutine);
        }
        if (playerGCoroutine != null)
        {
            StopCoroutine(playerGCoroutine);
        }
        if (baseGCoroutine != null)
        {
            StopCoroutine(baseGCoroutine);
        }
    }
}
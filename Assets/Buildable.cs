using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour, IActor
{
    public string buildingName;
    public float maxHealth;
    public float speed = 0;
    public HealthBar healthbar;
    //public ProjectorController projectorController;
    public Light spotlight;

    public GameObject buildingProjections;

    public ActorType actorType;

    private float currentHealth;
    private IBuildingBehavior buildingBehavior;

    public ActorType type => actorType;

    public float Speed { get => speed; set { speed = value; } }
    public float MaxHealth => maxHealth;
    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            healthbar.SetHealthbar(currentHealth / maxHealth);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    private void Awake()
    {
        buildingBehavior = GetComponent<IBuildingBehavior>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
    }

    private void Update()
    {
        
    }

    public void OnPlacing()
    {
        buildingBehavior.enabled = false;
    }

    public void OnBuild()
    {
        if (buildingProjections != null)
        {
            IgnoreParent[] ignoreParentsList = buildingProjections.GetComponentsInChildren<IgnoreParent>();

            foreach (IgnoreParent ip in ignoreParentsList)
            {
                ip.ignoreParentPosition = true;
                ip.ignoreParentRotation = true;
                ip.ResetTransformValues();
            }

            IgnoreParent ignoreParents = buildingProjections.GetComponent<IgnoreParent>();

            if (ignoreParents != null)
            {
                ignoreParents.ignoreParentPosition = true;
                ignoreParents.ignoreParentRotation = true;
                ignoreParents.ResetTransformValues();
            }
        }

        buildingBehavior.enabled = true;
        healthbar.gameObject.SetActive(true);

        GameController.instance.OnChangeToBuilding += OnBuildingMode;
        GameController.instance.OnChangeToCombat += OnCombatMode;
    }

    public void OnBuildingMode()
    {
        if (buildingProjections != null)
            buildingProjections.SetActive(true);
    }

    public void OnCombatMode()
    {
        if (buildingProjections != null)
            buildingProjections.SetActive(false);
    }

    private void Die()
    {
        GameController.instance.OnChangeToBuilding -= OnBuildingMode;
        GameController.instance.OnChangeToCombat -= OnCombatMode;
        Destroy(gameObject);
    }
}
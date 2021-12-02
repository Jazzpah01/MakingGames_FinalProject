using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Buildable : MonoBehaviour, IActor
{
    public string buildingName;
    public float maxHealth;
    public float speed = 0;
    public HealthBar healthbar;
    //public ProjectorController projectorController;
    public Light spotlight;
    public ActorType setType;

    public GameObject buildingProjections;
    public GameObject ignoreOnBuild;

    public ActorType actorType;

    private float currentHealth;
    private IBuildingBehavior buildingBehavior;

    public ActorType type => setType;

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

    public bool blockDamage { get; set; }
    public float damageReduction { get; set; }
    public float SpeedModifyer { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        buildingBehavior = GetComponent<IBuildingBehavior>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        blockDamage = false;
        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Call as this building is being placed. Before it is places.
    /// </summary>
    public void OnPlacing()
    {
        buildingBehavior.enabled = false;
        NavMeshObstacle ob = GetComponent<NavMeshObstacle>();
        if (ob != null)
            ob.enabled = false;
    }

    /// <summary>
    /// Call when this building is places onto the map.
    /// </summary>
    public void OnBuild()
    {
        // Set some game objects to ignore parent transform
        if (buildingProjections != null)
        {
            IgnoreInObject(buildingProjections);
        }
        if (ignoreOnBuild != null)
        {
            IgnoreInObject(ignoreOnBuild);
        }

        buildingBehavior.enabled = true;
        healthbar.gameObject.SetActive(true);

        NavMeshObstacle ob = GetComponent<NavMeshObstacle>();
        if (ob != null)
            ob.enabled = true;

        GameController.instance.OnChangeToBuilding += OnBuildingMode;
        GameController.instance.OnChangeToCombat += OnCombatMode;
    }

    /// <summary>
    /// Call when entering StrategyState
    /// </summary>
    public void OnBuildingMode()
    {
        if (buildingProjections != null)
            buildingProjections.SetActive(true);
    }

    /// <summary>
    /// Call when entering CombatState
    /// </summary>
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


    /// <summary>
    /// Find and enable all components IgnoreParent within a GameObject and children.
    /// </summary>
    /// <param name="toIgnore">GameObject which IgnoreParent components should be activated.</param>
    private void IgnoreInObject(GameObject toIgnore)
    {
        IgnoreParent[] ignoreParentsList = toIgnore.GetComponentsInChildren<IgnoreParent>();

        foreach (IgnoreParent ip in ignoreParentsList)
        {
            ip.ignoreParentPosition = true;
            ip.ignoreParentRotation = true;
            ip.ResetTransformValues();
        }

        IgnoreParent ignoreParents = toIgnore.GetComponent<IgnoreParent>();

        if (ignoreParents != null)
        {
            ignoreParents.ignoreParentPosition = true;
            ignoreParents.ignoreParentRotation = true;
            ignoreParents.ResetTransformValues();
        }
    }
}
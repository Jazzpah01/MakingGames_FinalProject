using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Buildable : MonoBehaviour, IActor, IBuildingCollider
{
    public BuildableData data;

    private string buildingName;

    [Header("References")]
    public HealthBar healthbar;
    public Light spotlight;
    public GameObject crossProjector;

    [Header("Optional References")]
    public GameObject buildingProjections;
    public GameObject ignoreOnBuild;
    public CollisionObserver placementCollider;

    private float currentHealth;
    private IBuildingBehavior buildingBehavior;
    private IBuildingRestrictions buildingRestrictions;
    private Rigidbody body;

    GameManager gameManager;

    public ActorType actorType => data.actorType;

    public float Speed { get; set; }
    public float MaxHealth { get; set; }
    public float Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            healthbar.SetHealthbar(currentHealth / MaxHealth);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageModifyer { get; set; }
    public float speedModifyer { get; set; }

    private void Awake()
    {
        buildingBehavior = GetComponent<IBuildingBehavior>();
        buildingRestrictions = GetComponent<IBuildingRestrictions>();
        if (placementCollider == null)
            placementCollider = GetComponent<CollisionObserver>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        if (placementCollider == null)
            placementCollider = GetComponent<CollisionObserver>();

        MaxHealth = data.maxHealth;
        currentHealth = MaxHealth;
        Speed = data.speed;

        blockDamage = false;

        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
    }

    private void Update()
    {
        
    }

    public bool isActorType(ActorType type)
    {
        return (type & actorType) != ActorType.None;
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
        if (placementCollider != null)
        {
            body = placementCollider.gameObject.AddComponent<Rigidbody>();
            body.isKinematic = true;
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
        if (placementCollider != null && placementCollider.gameObject != this.gameObject)
        {
            placementCollider.gameObject.SetActive(true);
        }

        if (buildingProjections != null)
            buildingProjections.SetActive(true);
    }

    /// <summary>
    /// Call when entering CombatState
    /// </summary>
    public void OnCombatMode()
    {
        if (placementCollider != null && placementCollider.gameObject != this.gameObject)
        {
            placementCollider.gameObject.SetActive(false);
        }

        if (buildingProjections != null)
            buildingProjections.SetActive(false);
    }

    /// <summary>
    /// Set building type to update name.
    /// </summary>
    /// <param name="buildingType">Type of building.</param>
    public void SetBuildingType(BuildingType buildingType)
    {
        buildingName = buildingType.name;
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

    public PlacementOutput CheckPlacement()
    {
        PlacementOutput output = new PlacementOutput();
        output.validPlacement = true;

        if (placementCollider == null)
        {
            Debug.Log("No placement collider is set. Please set placement collider in Buildable of " + this.gameObject);
            return output;
        }

        if (buildingRestrictions != null)
        {
            output = buildingRestrictions.CheckPlacement();
        }
        if (output.validPlacement && ((output.options & PlacementOptions.ReplaceDefaultRestrictions) == PlacementOptions.None))
        {
            foreach (Collider collider in placementCollider.Stay)
            {
                Buildable colll = collider.gameObject.GetComponent<Buildable>();
                Buildable parentcoll = collider.gameObject.GetComponentInParent<Buildable>();

                if ((colll != null && colll.placementCollider.Collider == collider) ||
                    (parentcoll != null && parentcoll.placementCollider.Collider == collider))
                {
                    output.validPlacement = false;
                    break;
                }
            }
        }
        return output;
    }

    public void SetValidPlacement(bool validity)
    {
        gameManager.gameCursor.setCrossCursor(!validity);
        spotlight.gameObject.SetActive(!validity);
        //crossProjector.gameObject.SetActive(!validity);
    }
}
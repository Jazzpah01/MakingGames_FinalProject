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
    public GameObject crossProjectorPrefab;

    public bool hasProjector;
    public Material projectorMaterialGreen;
    public Material projectorMaterialRed;
    [Header("Optional References")]
    public GameObject buildingProjections;
    public GameObject ignoreOnBuild;
    public CollisionObserver placementCollionObserver;

    private float currentHealth;
    private IBuildingBehavior buildingBehavior;
    private IBuildingRestrictions buildingRestrictions;
    private Rigidbody body;
    private GameObject crossProjector;
    private Projector projector;

    GameManager gameManager;

    public ActorType actorType => data.actorType;

    public float Speed { get; set; }
    public float MaxHealth { get; set; }
    public float Health
    {
        get => currentHealth;
        set
        {
            if (currentHealth <= 0f)
            {
                Die();
            } else
            {
                currentHealth = value;
                healthbar.SetHealthbar(currentHealth / MaxHealth);
            }
        }
    }

    public bool blockDamage { get; set; }
    public float damageModifyer { get; set; }
    public float speedModifyer { get; set; }

    public Collider placementCollider => placementCollionObserver.Collider;

    private void Awake()
    {
        buildingBehavior = GetComponent<IBuildingBehavior>();

        if (placementCollionObserver == null)
            placementCollionObserver = GetComponent<CollisionObserver>();
    }

    private void Start()
    {
        healthbar.gameObject.SetActive(false);
        buildingRestrictions = GetComponent<IBuildingRestrictions>();

        crossProjector = Instantiate(crossProjectorPrefab,transform);
        gameManager = GameManager.instance;

        data = Instantiate(data);

        MaxHealth = data.maxHealth;
        currentHealth = MaxHealth;
        Speed = data.speed;

        blockDamage = false;

        healthbar.textBox.text = buildingName;
        healthbar.SetHealthImageColour(Color.green);
        if (buildingProjections != null)
            projector = buildingProjections.GetComponent<Projector>();
    }

    public bool isActorType(ActorType type)
    {
        return (type & actorType) != ActorType.None;
    }

    //private void Update()
    //{
    //    time -= Time.deltaTime;
    //    Material m = projectorMaterial;
    //    if (f == 0)
    //    {
    //        m.color = Color.white;
    //    }
    //    if (f == 1)
    //    {
    //        m.color = Color.green;
    //    }
    //    if (f == 2)
    //    {
    //        m.color = Color.blue;
    //    }
    //    if (f == 3)
    //    {
    //        m.color = Color.grey;
    //    }
    //    if (f == 4)
    //    {
    //        m.color = Color.red;
    //    }
    //    if (f == 5)
    //    {
    //        m.color = Color.yellow;
    //    }
    //    if (f == 6)
    //    {
    //        m.color = Color.cyan;
    //    }
    //    projectorMaterial = m;

    //    if(time < 0)
    //    {
    //    f++;
    //        time += 0.2f;
    //    }

    //    if (f > 6)
    //        f = 0;
    //}

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
        if (placementCollionObserver != null)
        {
            body = placementCollionObserver.gameObject.AddComponent<Rigidbody>();
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
        if (placementCollionObserver != null && placementCollionObserver.gameObject != this.gameObject)
        {
            placementCollionObserver.gameObject.SetActive(true);
        }

        if (buildingProjections != null)
            buildingProjections.SetActive(true);
    }

    /// <summary>
    /// Call when entering CombatState
    /// </summary>
    public void OnCombatMode()
    {
        if (placementCollionObserver != null && placementCollionObserver.gameObject != this.gameObject)
        {
            placementCollionObserver.gameObject.SetActive(false);
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

    public PlacementOutput CheckPlacement(Ray ray)
    {
        PlacementOutput output = new PlacementOutput(true);

        if (placementCollionObserver == null)
        {
            Debug.Log("No placement collider is set. Please set placement collider in Buildable of " + this.gameObject);
            return output;
        }

        if (buildingRestrictions != null)
        {
            output = buildingRestrictions.CheckPlacement(ray);
        }
        if (output.validPlacement && ((output.options & PlacementOptions.ReplaceDefaultRestrictions) == PlacementOptions.None))
        {
            foreach (Collider collider in placementCollionObserver.Stay)
            {
                IBuildingCollider colll = collider.gameObject.GetComponent<IBuildingCollider>();

                if (colll != null && colll.placementCollider.transform.parent != this.transform)
                {
                    output = new PlacementOutput(false);
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
        crossProjector.gameObject.SetActive(!validity);
        if (hasProjector)
            setProjectorColor(!validity);
    }

    private void setProjectorColor(bool b)
    {
        if (b)
        {
            projector.material = projectorMaterialRed;
        }
        else
        {
            projector.material = projectorMaterialGreen;
        }
    }

}
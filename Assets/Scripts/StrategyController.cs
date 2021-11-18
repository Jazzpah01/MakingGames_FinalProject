using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class StrategyController : MonoBehaviour, IState
{
    [HideInInspector]
    public Camera cam;
    public LayerMask groundLayerMask;
    public int roundResource = 2;
    public Transform buildablesParent;
    public float startRotation = 0;
    public float rotationAngle = 45;
    public StrategyHUD strategyHUD;

    [HideInInspector]
    private GameObject GO = null;
    private float rotation;
    private bool isBuilding = false;
    private GameObject prefab;
    private LayerMask containmentLayerMask;

    private int currentCost = 0;

    private void Start()
    {
        cam = PlayerManager.instance.camera;
        GameManager.instance.resource = roundResource;
        rotation = startRotation;
        prefab = null;
    }

    public void UpdateState()
    {
        // Stop update while hovering the HUD.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            RemoveDummy();
            return;
        }
        // Cancel placement of building
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetMouseButtonDown(1))
        {
            isBuilding = false;

            RemoveDummy();
        }

        if (isBuilding)
        {
            // Change rotation with Q and E
            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotation += rotationAngle;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                rotation -= rotationAngle;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // See if there is a raycast hit on Ground layer
            if (Physics.Raycast(ray, out hit, 1000, groundLayerMask))
            {
                Vector3 spawnpoint = new Vector3(hit.point.x, hit.point.y + (prefab.transform.localScale.y / 2), hit.point.z);
                if (GO == null)
                {
                    // Create transparent dummy-gameobject
                    GO = Instantiate(prefab, spawnpoint, Quaternion.identity, buildablesParent);
                    ChangeGOAlfa(0.5f);

                    NavMeshObstacle ob = GO.GetComponent<NavMeshObstacle>();
                    if (ob != null)
                        ob.enabled = false;

                    GO.GetComponent<IActor>().enabled = false;
                }
                else
                {
                    //if the dummy-gameobject is within the containmentLayerMask
                    if (Physics.Raycast(ray, out hit, 1000, containmentLayerMask))
                    {
                        //hacky fix until red models are made
                        GO.transform.GetChild(0).gameObject.SetActive(false);

                        // Transform dummy-gameobject to an actual building
                        if (Input.GetMouseButtonDown(0) && GameManager.instance.resource >= currentCost)
                        {
                            SpawnPrefab();
                            // Make it so the player can place multiple buildings
                            if (GameManager.instance.resource < currentCost)
                            {
                                isBuilding = false;
                            }
                            return;
                        }
                    }
                    else
                    {
                        //indication that GO cannot be placed here
                        //hacky fix until red models are made
                        GO.transform.GetChild(0).gameObject.SetActive(true);
                    }
                }
                GO.transform.position = spawnpoint;
                GO.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                RemoveDummy();
            }
        }
    }

    /// <summary>
    /// Set the building that the player builds.
    /// </summary>
    /// <param name="type">Type of building.</param>
    public void SelectPrefab(BuildingType type)
    {
        if (GameManager.instance.resource < type.cost)
        {
            // Code for when you don't have enough resources
            return;
        }
        this.prefab = type.prefab;
        this.containmentLayerMask = type.containmentLayermask;
        this.currentCost = (int)type.cost;
        isBuilding = true;
    }

    private void ChangeGOAlfa(float alpha)
    {
        Renderer[] GORenderer = GO.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer rend in GORenderer)
        {
            Color tempColor = rend.material.color;
            tempColor.a = alpha;
            rend.material.color = tempColor;
        }
    }

    private void SpawnPrefab()
    {
        GO.GetComponent<NavMeshObstacle>().enabled = true;
        GO.GetComponent<IActor>().enabled = true;
        //TODO hacky enable healthbar
        GO.transform.GetChild(2).gameObject.SetActive(true);
        GameManager.instance.resource -= currentCost;
        strategyHUD.UpdateAlfa();
        ChangeGOAlfa(1);
        GO = null;
    }

    private void RemoveDummy()
    {
        if (GO != null)
        {
            Destroy(GO);
            GO = null;
        }
    }

    public void LateUpdateState()
    {

    }

    public void EnterState()
    {

    }

    public void ExitState()
    {
        isBuilding = false;
        RemoveDummy();
    }
}
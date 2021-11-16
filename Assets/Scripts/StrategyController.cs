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
    public LayerMask layerMask;
    public int roundResource = 2;
    public Transform buildablesParent;
    public float startRotation = 0;
    public float rotationAngle = 45;

    [HideInInspector] 
    public int resource;
    private GameObject GO = null;
    private float rotation;
    private bool isBuilding = false;
    private GameObject prefab;

    private int currentCost = 0;

    private void Start()
    {
        cam = PlayerManager.instance.camera;
        resource = roundResource;
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
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                Vector3 spawnpoint = new Vector3(hit.point.x, hit.point.y + (prefab.transform.localScale.y/2), hit.point.z);
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
                    // Transform dummy-gameobject to an actual building
                    if (Input.GetMouseButtonDown(0) && resource >= currentCost)
                    {
                        SpawnPrefab();

                        // Make it so the player can place multiple buildings
                        if (resource < currentCost)
                        {
                            isBuilding = false;
                        }

                        return;
                    }
                }
                GO.transform.position = spawnpoint;
                GO.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                // If not isBuilding, remove dummy-gameobject
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
        if (resource < type.cost) {
            // Code for when you don't have enough resources
            return;
        }
        this.prefab = type.prefab;
        this.layerMask = type.layermask;
        this.currentCost = (int)type.cost;
        isBuilding = true;
    }

    private void ChangeGOAlfa(float alpha)
    {
        Renderer GORenderer = GO.GetComponentInChildren<Renderer>();
        Color tempColor = GORenderer.material.color;
        tempColor.a = alpha;
        GORenderer.material.color = tempColor;
    }

    private void SpawnPrefab()
    {
        GO.GetComponent<NavMeshObstacle>().enabled = true;
        GO.GetComponent<IActor>().enabled = true;
        resource -= currentCost;
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
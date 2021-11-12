using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class StrategyController : MonoBehaviour, IState
{
    [HideInInspector]
    public Camera camera;
    public LayerMask layerMask;
    public int roundResource = 2;
    public GameObject prefab;
    public Transform buildablesParent;
    public float startRotation = 0;
    public float rotationAngle = 45;

    [HideInInspector] public int resource;
    private GameObject GO = null;
    private float rotation;
    private bool building = false;

    private int currentCost = 0;

    private void Start()
    {
        camera = PlayerManager.instance.camera;
        resource = roundResource;
        rotation = startRotation;
    }

    public void UpdateState()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Destroy(GO);
            GO = null;
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            building = false;
            Destroy(GO);
            GO = null;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            building = false;
            if (GO != null)
            {
                Destroy(GO);
                GO = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            building = !building;
            if (GO != null)
            {
                Destroy(GO);
                GO = null;
            }
        }
        if (building)
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotation += rotationAngle;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                rotation -= rotationAngle;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                Vector3 spawnpoint = new Vector3(hit.point.x, hit.point.y + (prefab.transform.localScale.y/2), hit.point.z);
                if (GO == null)
                {
                    GO = Instantiate(prefab, spawnpoint, Quaternion.identity, buildablesParent);
                    ChangeGOAlfa(0.5f);

                    NavMeshObstacle ob = GO.GetComponent<NavMeshObstacle>();
                    if (ob != null)
                        ob.enabled = false;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) && resource >= currentCost)
                    {
                        GO.GetComponent<NavMeshObstacle>().enabled = true;
                        resource -= currentCost;
                        ChangeGOAlfa(1);
                        building = false;
                        GO = null;

                        if (resource >= currentCost)
                        {
                            // Make it so the player can place multiple buildings
                            building = true;
                        }

                        return;
                    }
                }
                GO.transform.position = spawnpoint;
                GO.transform.rotation = Quaternion.Euler(0, rotation, 0);
            }
            else
            {
                if (GO != null)
                {
                    Destroy(GO);
                    GO = null;
                }
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
        this.currentCost = (int)type.cost;
        building = true;
    }

    private void ChangeGOAlfa(float alpha)
    {
        Renderer GORenderer = GO.GetComponentInChildren<Renderer>();
        Color tempColor = GORenderer.material.color;
        tempColor.a = alpha;
        GORenderer.material.color = tempColor;
    }

    public void LateUpdateState()
    {

    }

    public void EnterState()
    {

    }

    public void ExitState()
    {
        building = false;
        if (GO != null)
        {
            Destroy(GO);
            GO = null;
        }
    }
}
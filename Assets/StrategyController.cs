using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyController : MonoBehaviour, IState
{
    public Camera cam;
    public LayerMask layerMask;
    public int roundResource = 2;
    public GameObject prefab;
    public Transform buildablesParent;
    public float startRotation = 0;
    public float rotationSpeed = 10;

    private int resource;
    private GameObject GO = null;
    private float rotation;
    private bool building = false;

    private void Start()
    {
        resource = roundResource;
        rotation = startRotation;
    }

    public void UpdateState()
    {
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

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                rotation += rotationSpeed;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                rotation -= rotationSpeed;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                Vector3 spawnpoint = new Vector3(hit.point.x, hit.point.y + (prefab.transform.localScale.y/2), hit.point.z);
                if (GO == null)
                {
                    GO = Instantiate(prefab, spawnpoint, Quaternion.identity, buildablesParent);
                    ChangeGOAlfa(0.5f);
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) && resource > 0)
                    {
                        resource--;
                        ChangeGOAlfa(1);
                        building = false;
                        GO = null;
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

    private void ChangeGOAlfa(float alpha)
    {
        Renderer GORenderer = GO.GetComponent<Renderer>();
        Color tempColor = GORenderer.material.color;
        tempColor.a = alpha;
        GORenderer.material.color = tempColor;
    }

    public void LateUpdateState()
    {

    }

    public void EnterState()
    {
        resource = roundResource;
        cam.gameObject.SetActive(true);
    }

    public void ExitState()
    {
        cam.gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyController : MonoBehaviour, IState
{
    public Camera cam;
    public LayerMask layerMask;
    public int roundResource = 2;
    private int resource;
    public GameObject prefab;

    private void Start()
    {
        resource = roundResource;
    }

    public void UpdateState()
    {
        if (Input.GetMouseButtonDown(0) && resource > 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                Instantiate(prefab).transform.position = hit.point;
                resource--;
            }
        }
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
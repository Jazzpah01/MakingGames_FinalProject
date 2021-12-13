using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGround : MonoBehaviour
{
    [Tooltip("The point at which the flower building should be places.")] public GameObject point;
    [Tooltip("The flower building currently occupying this plant zone.")] public GameObject occupied;

    private void Start()
    {
        if (occupied != null)
            occupied.transform.position = point.transform.position;
    }

    public void SetObject(GameObject go)
    {
        occupied = go;
    }

    public bool IsFree(GameObject go)
    {
        return (occupied == null || occupied == go) ? true : false;
    }

    public Vector3 GetCenter()
    {
        return point.transform.position;
    }
}
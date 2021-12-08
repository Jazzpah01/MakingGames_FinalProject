using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGround : MonoBehaviour
{
    public GameObject point;
    GameObject occupied;

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
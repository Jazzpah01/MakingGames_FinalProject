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

    public bool IsFree()
    {
        return (occupied == null) ? true : false;
    }

    public Vector3 GetCenter()
    {
        if (occupied == null)
        {
            return point.transform.position;
        } else
        {
            return point.transform.position;
        }
    }
}
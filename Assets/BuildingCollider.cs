using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingCollider : MonoBehaviour, IBuildingCollider
{
    Collider collider;

    public Collider placementCollider => collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
}
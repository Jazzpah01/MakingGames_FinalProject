using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeRestrictions : MonoBehaviour, IBuildingRestrictions
{
    public PlacementOutput CheckPlacement()
    {
        throw new System.Exception("not implemented");
    }

    public PlacementOutput CheckPlacement(Ray ray)
    {
        throw new System.NotImplementedException();
    }
}
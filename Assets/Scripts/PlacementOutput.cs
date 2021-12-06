using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlacementOutput
{
    public bool validPlacement;
    public PlacementOptions options;
    public Vector3 position;
}


[System.Flags]
public enum PlacementOptions
{
    None = 0,
    ReplaceDefaultRestrictions = 1,
    OverridePosition = 2
}
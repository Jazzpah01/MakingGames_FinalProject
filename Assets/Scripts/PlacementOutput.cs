using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnPlaced();

public struct PlacementOutput
{
    readonly public bool validPlacement;
    readonly public PlacementOptions options;
    readonly public Vector3 position;
    readonly public OnPlaced onPlaced;

    public PlacementOutput(bool validPlacement, PlacementOptions options, Vector3 position, OnPlaced onPlaced = null)
    {
        this.validPlacement = validPlacement;
        this.options = options;
        this.position = position;
        this.onPlaced = onPlaced;
    }

    public PlacementOutput(bool validPlacement)
    {
        this.validPlacement = validPlacement;
        this.options = PlacementOptions.None;
        this.position = Vector3.zero;
        this.onPlaced = null;
    }
}


[System.Flags]
public enum PlacementOptions
{
    None = 0,
    ReplaceDefaultRestrictions = 1,
    OverridePosition = 2
}
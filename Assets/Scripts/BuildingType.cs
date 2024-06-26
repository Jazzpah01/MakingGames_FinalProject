using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingType
{
    public string name = "";
    public float cost = 0;
    public GameObject prefab;
    public LayerMask containmentLayermask;
    public string description = "";
    public Sprite icon;
}

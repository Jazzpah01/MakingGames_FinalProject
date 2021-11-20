using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/BuildingList")]
public class BuildingList : ScriptableObject
{
    public List<BuildingType> buildings;

    public BuildingType this[int key]
    {
        get => buildings[key];
    }

    public int Count => buildings.Count;
}
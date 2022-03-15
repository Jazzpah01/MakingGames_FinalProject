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

    public BuildingType ElementOf(BuildableData data)
    {
        foreach (BuildingType type in buildings)
        {
            if (type.data == data)
            {
                return type;
            }
        }

        throw new System.Exception("Could not find building type of that data.");
    }

    public int Count => buildings.Count;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Actor/Building/Seed")]
public class ResourceBuildingData : BuildableData
{
    [Header("Resource Building")]
    public int resourceGain;
    public int wavesToHarvest;
}
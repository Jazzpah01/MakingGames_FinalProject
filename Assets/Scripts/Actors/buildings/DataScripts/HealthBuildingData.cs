using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Assets/Actor/Building/SeedHealth")]
public class HealthBuildingData : BuildableData
{
    [Header("% Health healed after wave")]
    [Range(0,100)]
    public int baseHealthGain;
    [Range(0, 100)]
    public int playerHealthGain;

    public int wavesToHarvest;
}
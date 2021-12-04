using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Actor/Building/LineSpider(Stanky)")]
public class LineSlowerData : BuildableData
{
    [Header("Line Slower")]
    [Range(0.1f, 1)]
    public float speedModifyer = 1;
    public float slowObjectLifeTime;
    public float slowObjectEveryXSecond;
    public float walkingDistance;
}
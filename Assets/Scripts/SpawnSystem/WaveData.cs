using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public float totalValue;
    public float combatValue;
    public List<EnemyWeight> enemyWeights;

    public float GetNormalizedWeight(int index)
    {
        float totalWeight = 0;
        for (int i = 0; i < enemyWeights.Count; i++)
        {
            totalWeight += enemyWeights[i].weight;
        }
        return enemyWeights[index].weight / totalWeight;
    }
}
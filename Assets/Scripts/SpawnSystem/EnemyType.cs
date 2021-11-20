using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject prefab;
    public float value;
    [HideInInspector] public int identifyer;
}
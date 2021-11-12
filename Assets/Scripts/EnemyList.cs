using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<EnemyType> enemyList;
}
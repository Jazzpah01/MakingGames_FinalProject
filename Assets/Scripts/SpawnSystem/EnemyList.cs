using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/EnemyList")]
public class EnemyList : ScriptableObject
{
    public List<EnemyType> enemyList;

    public EnemyType this[GameObject prefab]
    {
        get
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].identifyer = i;
                if (prefab == enemyList[i].prefab)
                {
                    return enemyList[i];
                }
            }

            throw new System.Exception("Prefab does not exist in level EnemyList!");
        }
    }
}
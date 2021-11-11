using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public int enemyAmount;
    public GameObject[] enemyPrefab;
    public Transform player;
    public Transform inititalTarget;
    public Transform enemyParent;
    public Transform[] spawnPoints;
    void Start()
    {
        player = PlayerManager.instance.player.transform;
    }

    public void SpawnEnemies() {
        int c = 0;
        for (int i = 0; i < enemyAmount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab[i %2],spawnPoints[c].position,Quaternion.identity,enemyParent);
            //big enemy goes for the player
            if (enemy.name.Contains("Big"))
            {
                enemy.GetComponent<EnemyController>().primaryTarget = player;
            }
            else
            {
                enemy.GetComponent<EnemyController>().primaryTarget = inititalTarget;
            }
            c++;
            if (c >= spawnPoints.Length-1)
            {
                c = 0;
            }
        }
    }
}

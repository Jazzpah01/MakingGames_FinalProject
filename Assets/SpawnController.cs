using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public int enemyAmount;
    public GameObject enemyPrefab;
    public Transform inititalTarget;
    public Transform enemyParent;
    public Transform[] spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnEnemies() {
        int c = 0;
        for (int i = 0; i < enemyAmount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab,spawnPoints[c].position,Quaternion.identity,enemyParent);
            enemy.GetComponent<EnemyController>().primaryTarget = inititalTarget;
            c++;
            if (c >= spawnPoints.Length-1)
            {
                c = 0;
            }
        }
    }
}

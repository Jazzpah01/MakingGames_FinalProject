using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy: IActor
{
    EnemyType enemyType { get; set; }
    GameObject spawnPoint { get; set; }
}
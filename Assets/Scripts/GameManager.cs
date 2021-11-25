using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public bool inBattle = false;

    public int resource;

    [Header("Level Data")]
    public BuildingList buildingTypes;
    public EnemyList enemyTypes;

    [Header("Controller References")]
    public GameController gameController;
    public SpawnController spawnController;
    public BuildingController buildingController;


    void Awake()
    {
        instance = this;
    }
}

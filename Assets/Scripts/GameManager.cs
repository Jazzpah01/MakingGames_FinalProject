    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameController.GameState GameState => gameController.state;

    [Header("Resources")]
    public float initialResource;
    [Tooltip("Each round these will be added to resources. NOTE: For the first building phase, ONLY the intial resource is added.")]
    public float roundResource;

    [System.NonSerialized] public float currentResource;

    [Header("Sound")]
    public bool sfxOn = true;
    public bool musicOn = true;

    [Header("Level Data")]
    public BuildingList buildingTypes;
    public EnemyList enemyTypes;

    [Header("Controller References")]
    public GameController gameController;
    public SpawnController spawnController;
    public BuildingController buildingController;

    public List<(GameObject GO, float Cost)> waveBuildingList;


    void Awake()
    {
        instance = this;
        waveBuildingList = new List<(GameObject GO, float Cost)>();
    }

    private void Start()
    {
        currentResource = initialResource;
    }
}

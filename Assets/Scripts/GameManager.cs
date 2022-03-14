    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using UnityEngine;
    using UnityEngine.Networking;

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
    public LevelDescriptionList levelDescriptionData;

    [Header("Controller References")]
    public Base baseController;
    public GameController gameController;
    public SpawnController spawnController;
    public BuildingController buildingController;
    public HUD hud;
    [HideInInspector]
    public GameCursor gameCursor;

    public List<(GameObject GO, float Cost)> waveBuildingList;
    public List<GameObject> placedBuildings;



    void Awake()
    {
        instance = this;
        waveBuildingList = new List<(GameObject GO, float Cost)>();
        gameCursor = hud.gameObject.GetComponent<GameCursor>();
    }

    private void Start()
    {
        currentResource = initialResource;
    }

    
}

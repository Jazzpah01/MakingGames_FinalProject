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

    public struct LevelData
    {
        public int playerID;
        public int GameSessionId;
        public int levelSessionID;
        public string type;
        public string data;
        public int level;
        public float damage;
        public bool IsDestroyed;
        public int wavePlaced;
        public int waveDestroyed;
        public float cost;
        public int lifeTime;
    }

    //Telemetry setup
    private const string GoogleFormBaseUrl = "https://docs.google.com/forms/d/e/1FAIpQLSdCiBthYndKGuRt82xpBR_nIHBV4n1CbiZO44WurpEagw4STw/";

    private const string form_playerID = "entry.175486278";
    private const string form_gameSessionID = "entry.152759416";
    private const string form_LevelSessionID = "entry.309323523";
    private const string form_type = "entry.284250750";
    private const string form_data = "entry.340410309";
    private const string form_level = "entry.900037523";
    private const string form_damage = "entry.399265859";
    private const string form_IsDestroyed = "entry.693700941";
    private const string form_wavePlaced = "entry.329000091";
    private const string form_waveDestroyed = "entry.844842889";
    private const string form_cost = "entry.2062664417";
    private const string form_lifeTime = "entry.1860082768";

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

    public void collectData()
    {
        foreach(var GO in waveBuildingList)
        {
            LevelData data = new LevelData
            {
            playerID = SessionManager.instance.playerId,
            GameSessionId = SessionManager.instance.GameSessionId,
            levelSessionID = -1,
            type = GO.GO.name,
            data = "what",
            level = LevelManager.instance.getCurrentLevel(),
            damage = GO.GO.GetComponent<BuildingData>().damage,
            IsDestroyed = GO.GO.GetComponent<BuildingData>().IsDestroyed,
            wavePlaced = GO.GO.GetComponent<BuildingData>().wavePlaced,
            waveDestroyed = GO.GO.GetComponent<BuildingData>().waveDestroyed,
            cost = GO.Cost,
            lifeTime = GO.GO.GetComponent<BuildingData>().waveDestroyed - GO.GO.GetComponent<BuildingData>().wavePlaced
            };

            sendData(data);
        }
        
    }

    IEnumerator sendData(LevelData data)
    {
        CultureInfo ci = CultureInfo.GetCultureInfo("en-GB");
        Thread.CurrentThread.CurrentCulture = ci;

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";

        WWWForm form = new WWWForm();

        form.AddField(form_playerID, data.playerID);
        form.AddField(form_gameSessionID, data.GameSessionId);
        form.AddField(form_LevelSessionID, data.levelSessionID);
        form.AddField(form_type, data.type);
        form.AddField(form_data, data.data);
        form.AddField(form_level, data.level);
        form.AddField(form_damage, data.damage.ToString());
        form.AddField(form_IsDestroyed, data.IsDestroyed.ToString());
        form.AddField(form_wavePlaced, data.wavePlaced);
        form.AddField(form_waveDestroyed, data.waveDestroyed);
        form.AddField(form_cost, data.cost.ToString());
        form.AddField(form_lifeTime, data.lifeTime);

        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();

            yield return null;
            print("Requeust sent");
        }
    }
}

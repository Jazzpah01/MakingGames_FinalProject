using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using System.Threading;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    public int playerId = 0;
    public int GameSessionId;
    public int levelSessionID;
    public int levelID;
    public int waveID;

    public LevelSheet currentLevelSheet;
    public SessionSheet currentSessionSheet;

    public Dictionary<Buildable, PlacementSheet> placementSheets = new Dictionary<Buildable, PlacementSheet>();

    public IActor lastDamagingActor;

    public float levelStartTime = 0;

    void Awake()
    {
        instance = this;
        if (playerId == 0)
            playerId = Random.Range(1, 1000000);
        GameSessionId = Random.Range(1, 1000000);
        levelSessionID = Random.Range(50001, 1000000);
        waveID = 0;
        levelID = 0;

        currentSessionSheet = new SessionSheet();
        currentSessionSheet.gameSessionID = GameSessionId;
        currentSessionSheet.playerID = playerId;

        currentLevelSheet = new LevelSheet();
        currentLevelSheet.levelID = 0;
        currentLevelSheet.gameSessionID = GameSessionId;
        currentLevelSheet.playerID = playerId;
        currentLevelSheet.levelSessionID = levelSessionID;

        levelStartTime = Time.time;

    }

    private void Start()
    {
        GameEvents.BuildablePlacement += PlaceBuildable;
        GameEvents.ActorKilled += ActorDied;
        GameEvents.ActorDestroyed += ActorDestroyed;
        GameEvents.DamageDealt += ActorDamages;
        GameEvents.LevelChanged += LevelChanged;
        GameEvents.WaveFinished += WaveFinished;
        GameEvents.UndoPlacement += PlacementUndo;
        GameEvents.GameClosed += GameClose;
    }

    private void Update()
    {
        foreach (PlacementSheet sheet in placementSheets.Values)
        {
            sheet.lifeTime += Time.deltaTime;
        }
    }

    public void PlaceBuildable((Buildable buildable, bool success) input)
    {
        if (input.success)
        {
            Buildable buildable = input.buildable;
            placementSheets.Add(buildable, new PlacementSheet());

            placementSheets[buildable].playerID = playerId;
            placementSheets[buildable].gameSessionID = GameSessionId;
            placementSheets[buildable].levelSessionID = levelSessionID;
            placementSheets[buildable].levelID = levelID;
            placementSheets[buildable].type = buildable.data.name;
            placementSheets[buildable].cost = GameManager.instance.buildingTypes.ElementOf(buildable.data).cost;
            placementSheets[buildable].wavePlaced = waveID;
            //placementSheet.data; ???
        }
        else
        {
            currentLevelSheet.failedAttemptsToPlace++;
        }
    }

    public void ActorDamages((IActor source, IActor target, float amount) input)
    {
        Buildable buildable = input.source as Buildable;
        PlayerController player = input.source as PlayerController;

        lastDamagingActor = input.source;

        if (buildable != null)
        {
            placementSheets[buildable].damage += input.amount;
        } else if (player != null)
        {
            currentLevelSheet.damage += input.amount;
        }
    }

    public void ActorDied(IActor actor)
    {
        Buildable buildable = actor as Buildable;
        PlayerController player = actor as PlayerController;
        Base basec = actor as Base;

        if (buildable != null)
        {
            placementSheets[buildable].isDestroyed = true;
        }
        else if (player != null || basec != null)
        {
            // If player or base is killed, they failed and we increment losses
            switch (levelID)
            {
                case 0:
                    currentSessionSheet.lvl1Loss++;
                    break;
                case 1:
                    currentSessionSheet.lvl2Loss++;
                    break;
                case 2:
                    currentSessionSheet.lvl3Loss++;
                    break;
            }
        }
    }

    public void ActorDestroyed(IActor actor)
    {
        Buildable buildable = actor as Buildable;
        PlayerController player = actor as PlayerController;
        Base basec = actor as Base;

        if (actor is Buildable && buildable.wasPlaced)
        {
            print("Building destroyed!");

            placementSheets[buildable].waveDestroyed = waveID;

            PlacementSheet retval = placementSheets[buildable];
            placementSheets.Remove(buildable);

            StartCoroutine(SendData(retval));

            return;
        }
    }

    public void LevelChanged(int newLevelID)
    {
        Debug.Log("Changed level!");

        bool newLevel = (newLevelID > levelID);

        if (newLevel)
        {
            switch (newLevelID)
            {
                case 0:
                    currentSessionSheet.lvl1CompletionTime = Time.time - levelStartTime;
                    break;
                case 1:
                    currentSessionSheet.lvl2CompletionTime = Time.time - levelStartTime;
                    break;
                case 2:
                    currentSessionSheet.lvl3CompletionTime = Time.time - levelStartTime;
                    break;
            }
        }

        if (currentLevelSheet != null)
        {
            StartCoroutine( SendData(currentLevelSheet) );
        }

        levelID = newLevelID;
        waveID = 0;
        levelSessionID = Random.Range(50001, 1000000);

        currentLevelSheet = new LevelSheet();
        currentLevelSheet.levelID = levelID;
        currentLevelSheet.gameSessionID = GameSessionId;
        currentLevelSheet.playerID = playerId;
        currentLevelSheet.levelSessionID = levelSessionID;

        levelStartTime = Time.time;
    }

    public void WaveFinished(int finishedID)
    {
        waveID++;
    }

    public void PlacementUndo(Buildable building)
    {
        currentLevelSheet.numberOfUndos++;
    }

    public void GameClose()
    {
        if (currentSessionSheet != null)
        {
            StartCoroutine(SendData(currentSessionSheet));
            currentSessionSheet = null;
        }
        if (currentLevelSheet != null)
        {
            StartCoroutine(SendData(currentLevelSheet));
            currentLevelSheet = null;
        }
            

        Awake();
    }

    //Telemetry setup

    //placement
    //https://docs.google.com/forms/d/e/1FAIpQLSdCiBthYndKGuRt82xpBR_nIHBV4n1CbiZO44WurpEagw4STw/viewform?usp=sf_link
    //level
    //https://docs.google.com/forms/d/e/1FAIpQLSdheL4fZf8IAcD3q6ZXI2MqhgnHkWFTK6cayFnjvKm8v6C0sA/viewform?usp=sf_link
    //session
    //https://docs.google.com/forms/d/e/1FAIpQLSd10m2X0FPLRKzaHPffMNzFeCSeeQdK_8tfIjMk--P7Ujurdg/viewform?usp=sf_link

    public void collectData()
    {

    }

    public void AddPlacementData()
    {

    }

    IEnumerator SendData(PlacementSheet data)
    {
        print("Placement Data starting...");

        CultureInfo ci = CultureInfo.GetCultureInfo("en-GB");
        Thread.CurrentThread.CurrentCulture = ci;

        string urlGoogleFormResponse = PlacementSheet.url + "formResponse";

        WWWForm form = new WWWForm();

        form.AddField(PlacementSheet.form_playerID, data.playerID);
        form.AddField(PlacementSheet.form_gameSessionID, data.gameSessionID);
        form.AddField(PlacementSheet.form_levelSessionID, data.levelSessionID);
        form.AddField(PlacementSheet.form_type, data.type);
        form.AddField(PlacementSheet.form_data, data.data);
        form.AddField(PlacementSheet.form_levelID, data.levelID);
        form.AddField(PlacementSheet.form_damage, data.damage.ToString());
        form.AddField(PlacementSheet.form_isDestroyed, data.isDestroyed.ToString());
        form.AddField(PlacementSheet.form_wavePlaced, data.wavePlaced);
        form.AddField(PlacementSheet.form_waveDestroyed, data.waveDestroyed);
        form.AddField(PlacementSheet.form_cost, data.cost.ToString());
        form.AddField(PlacementSheet.form_lifeTime, data.lifeTime.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();

            yield return null;
            print("Requeust sent");
        }
    }

    IEnumerator SendData(LevelSheet data)
    {
        print("Level Data starting...");

        CultureInfo ci = CultureInfo.GetCultureInfo("en-GB");
        Thread.CurrentThread.CurrentCulture = ci;

        string urlGoogleFormResponse = LevelSheet.url + "formResponse";

        WWWForm form = new WWWForm();

        form.AddField(LevelSheet.form_playerID, data.playerID.ToString());
        form.AddField(LevelSheet.form_gameSessionID, data.gameSessionID.ToString());
        form.AddField(LevelSheet.form_levelSessionID, data.levelSessionID.ToString());
        form.AddField(LevelSheet.form_levelID, data.levelID.ToString());
        form.AddField(LevelSheet.form_damage, data.damage.ToString());
        form.AddField(LevelSheet.form_failedAttemptsToPlace, data.failedAttemptsToPlace.ToString());
        form.AddField(LevelSheet.form_numberOfUndos, data.numberOfUndos.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();

            yield return null;
            print("Requeust sent");
        }
    }

    IEnumerator SendData(SessionSheet data)
    {
        print("Session Data starting...");

        CultureInfo ci = CultureInfo.GetCultureInfo("en-GB");
        Thread.CurrentThread.CurrentCulture = ci;

        string urlGoogleFormResponse = SessionSheet.url + "formResponse";

        WWWForm form = new WWWForm();

        form.AddField(SessionSheet.form_playerID, data.playerID.ToString());
        form.AddField(SessionSheet.form_gameSessionID, data.gameSessionID.ToString());
        form.AddField(SessionSheet.form_completed, data.completed.ToString());
        form.AddField(SessionSheet.form_lvl1Loss, data.lvl1Loss.ToString());
        form.AddField(SessionSheet.form_lvl2Loss, data.lvl2Loss.ToString());
        form.AddField(SessionSheet.form_lvl3Loss, data.lvl3Loss.ToString());
        form.AddField(SessionSheet.form_lvl1CompletionTime, data.lvl1CompletionTime.ToString());
        form.AddField(SessionSheet.form_lvl2CompletionTime, data.lvl2CompletionTime.ToString());
        form.AddField(SessionSheet.form_lvl3CompletionTime, data.lvl3CompletionTime.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();

            yield return null;
            print("Requeust sent");
        }
    }
}

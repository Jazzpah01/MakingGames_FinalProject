using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Globalization;
using System.Threading;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    public int playerId;
    public int GameSessionId;

    void Awake()
    {
        instance = this;
        playerId = Random.Range(1, 50000);
        GameSessionId = playerId;
    }

    private void Start()
    {
        GameEvents.instance.Subscribe(PlacementData, "BuildableObjectDestroy");
    }

    public void PlacementData(object data)
    {

    }

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

    //placement
    //https://docs.google.com/forms/d/e/1FAIpQLSdCiBthYndKGuRt82xpBR_nIHBV4n1CbiZO44WurpEagw4STw/viewform?usp=sf_link
    //level
    //https://docs.google.com/forms/d/e/1FAIpQLSdheL4fZf8IAcD3q6ZXI2MqhgnHkWFTK6cayFnjvKm8v6C0sA/viewform?usp=sf_link
    //session
    //https://docs.google.com/forms/d/e/1FAIpQLSd10m2X0FPLRKzaHPffMNzFeCSeeQdK_8tfIjMk--P7Ujurdg/viewform?usp=sf_link

    public void collectData()
    {

    }

    void SendData(object data) {

    }

    public void AddPlacementData()
    {

    }

    IEnumerator SendData(PlacementSheet data)
    {
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
        form.AddField(PlacementSheet.form_lifeTime, data.lifeTime);

        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();

            yield return null;
            print("Requeust sent");
        }
    }

    IEnumerator SendData(LevelSheet data)
    {
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

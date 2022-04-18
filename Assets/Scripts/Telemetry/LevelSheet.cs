using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSheet
{
    public int playerID;
    public int gameSessionID;
    public int levelSessionID;
    public int levelID;
    public float damage = 0;
    public int failedAttemptsToPlace = 0;
    public int numberOfUndos = 0;

    public const string url = "https://docs.google.com/forms/d/e/1FAIpQLSdheL4fZf8IAcD3q6ZXI2MqhgnHkWFTK6cayFnjvKm8v6C0sA/";
    public const string url_viewform = "https://docs.google.com/forms/d/e/1FAIpQLSdheL4fZf8IAcD3q6ZXI2MqhgnHkWFTK6cayFnjvKm8v6C0sA/viewform?usp=sf_link";
    public const string form_playerID = "entry.222496046";
    public const string form_gameSessionID= "entry.631585767";
    public const string form_levelSessionID= "entry.2103452268";
    public const string form_levelID= "entry.1712623364";
    public const string form_damage= "entry.1152675665";
    public const string form_failedAttemptsToPlace= "entry.1589070766";
    public const string form_numberOfUndos= "entry.894516615";
}

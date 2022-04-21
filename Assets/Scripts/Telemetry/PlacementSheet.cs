using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSheet
{
    public string gameVersion;
    public int playerID;
    public int gameSessionID;
    public int levelSessionID;
    public string type = "";
    public string data = "";
    public int levelID;
    public float damage = 0;
    public bool isDestroyed = false;
    public int wavePlaced = 0;
    public int waveDestroyed = 0;
    public float cost = 0;
    public float lifeTime = 0;
    public float xPosition;
    public float zPosition;

    public const string url = "https://docs.google.com/forms/d/e/1FAIpQLSdCiBthYndKGuRt82xpBR_nIHBV4n1CbiZO44WurpEagw4STw/";
    public const string url_viewform = "https://docs.google.com/forms/d/e/1FAIpQLSdCiBthYndKGuRt82xpBR_nIHBV4n1CbiZO44WurpEagw4STw/viewform?usp=sf_link";
    public const string form_gameVersion = "entry.656015506";
    public const string form_playerID = "entry.175486278";
    public const string form_gameSessionID = "entry.152759416";
    public const string form_levelSessionID = "entry.309323523";
    public const string form_type = "entry.284250750";
    public const string form_data = "entry.340410309";
    public const string form_levelID = "entry.900037523";
    public const string form_damage = "entry.399265859";
    public const string form_isDestroyed = "entry.693700941";
    public const string form_wavePlaced = "entry.329000091";
    public const string form_waveDestroyed = "entry.844842889";
    public const string form_cost = "entry.2062664417";
    public const string form_lifeTime = "entry.1860082768";
    public const string form_xPosition = "entry.340410309";
    public const string form_zPosition = "entry.1409320112";
}

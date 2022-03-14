using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SessionSheet
{
    public int playerID;
    public int gameSessionID;
    public bool completed;
    public int lvl1Loss;
    public int lvl2Loss;
    public int lvl3Loss;
    public float lvl1CompletionTime;
    public float lvl2CompletionTime;
    public float lvl3CompletionTime;

    public const string url = "https://docs.google.com/forms/d/e/1FAIpQLSd10m2X0FPLRKzaHPffMNzFeCSeeQdK_8tfIjMk--P7Ujurdg/";
    public const string url_viewform = "https://docs.google.com/forms/d/e/1FAIpQLSd10m2X0FPLRKzaHPffMNzFeCSeeQdK_8tfIjMk--P7Ujurdg/viewform?usp=sf_link";
    public const string form_playerID= "entry.1864009900";
    public const string form_gameSessionID= "entry.887693786";
    public const string form_completed= "entry.87416968";
    public const string form_lvl1Loss= "entry.1067054184";
    public const string form_lvl2Loss= "entry.1063579091";
    public const string form_lvl3Loss= "entry.435194433";
    public const string form_lvl1CompletionTime= "entry.247505372";
    public const string form_lvl2CompletionTime= "entry.787208791";
    public const string form_lvl3CompletionTime= "entry.1558460416";
}

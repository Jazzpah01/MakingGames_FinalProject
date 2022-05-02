using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemetryPrompt : MonoBehaviour
{
    public InteractableUI yesButton;
    public InteractableUI noButton;

    public static bool choiseMade = false;

    private void Start()
    {
        yesButton.OnClicked = delegate { YesButton(); };
        noButton.OnClicked = delegate { NoButton(); };
    }

    void YesButton()
    {
        choiseMade = true;
        GameManager.instance.hud.StartMenuSetup();
        GameObject smGO = new GameObject();

        smGO.AddComponent<SessionManager>();
    }

    void NoButton()
    {
        choiseMade = true;
        GameManager.instance.hud.StartMenuSetup();
    }
}
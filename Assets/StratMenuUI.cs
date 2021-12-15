using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratMenuUI : MonoBehaviour
{
    HUD hud;
    private void Start()
    {
        hud = GetComponentInParent<HUD>();
    }
    public void StartButton()
    {
        hud.StartMenuStartButton();
    }

    public void CreditsButton()
    {
        hud.StartMenuCreditsButton();
    }

    public void ControlsButton()
    {
        hud.StartMenuControlsButton();
    }
    public void QuitButton()
    {
        hud.QuitButton();
    }
}

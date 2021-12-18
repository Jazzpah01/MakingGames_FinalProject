using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    HUD hud;
    private void Start()
    {
        hud = GetComponentInParent<HUD>();
    }
    public void ResumeButton()
    {
        hud.IngameMenuResumeButton();
    }
    public void SettingsButton()
    {
        hud.IngameMenuSettingsButton();
    }
    public void RestartButton()
    {
        hud.IngameMenuRestartMenuButton();
    }
    public void QuitButton()
    {
        hud.IngameMenuQuitButton();
    }

}

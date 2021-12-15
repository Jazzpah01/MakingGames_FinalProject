using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    HUD hud;
    private void Start()
    {
        hud = GetComponentInParent<HUD>();
    }

    public void RestartButton()
    {
        hud.GameOverMenuRetry();
    }
    public void MainMenuButton()
    {
        hud.GameOverMenuMainMenu();
    }
}

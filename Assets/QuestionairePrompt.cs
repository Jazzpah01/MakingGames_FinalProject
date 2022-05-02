using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionairePrompt : MonoBehaviour
{
    public InteractableUI yesButton;
    public InteractableUI noButton; 

    private void Start()
    {
        SessionManager sessionManager = SessionManager.instance;

        if (sessionManager == null)
        {
            NoButton();
        }

        yesButton.OnClicked = delegate { YesButton(); };
        noButton.OnClicked = delegate { NoButton(); };
    }

    void YesButton()
    {
        GameEvents.GameClosed();
        Application.OpenURL(
            $"https://docs.google.com/forms/d/e/1FAIpQLSfV0Pjggpm2oLKwJRBTGYpNwiC614W3wKjGU-kLUDW_L8TN0g/viewform?usp=pp_url&entry.512054640={SessionManager.instance.playerId}&entry.1864559782={SessionManager.instance.gameVersion}");
    }

    void NoButton()
    {
        GameEvents.GameClosed();
    }
}
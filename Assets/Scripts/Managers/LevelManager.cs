using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<GameObject> levels;
    private int currentLevel = 0;
    private GameObject level;
    public bool cheatsActive;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        RestartAllLevels();
    }

    public void RestartAllLevels()
    {
        currentLevel = 0;
        loadLevel();
    }

    public void RestartLevel()
    {
        currentLevel--;
        loadLevel();
    }

    public int getCurrentLevel()
    {
        return currentLevel;
    }

    public void loadLevel()
    {
        if (currentLevel >= levels.Count)
        {
            // Game completed!
            Debug.Log("There are no more levels!");
            if (GameEvents.GameCompleted != null)
                GameEvents.GameCompleted();
            if (GameEvents.GameClosed != null)
                GameEvents.GameClosed();
            if (SessionManager.instance != null)
            {
                Application.OpenURL(
                    $"https://docs.google.com/forms/d/e/1FAIpQLSfV0Pjggpm2oLKwJRBTGYpNwiC614W3wKjGU-kLUDW_L8TN0g/viewform?usp=pp_url&entry.512054640={SessionManager.instance.playerId}&entry.1864559782={SessionManager.instance.gameVersion}");
            }
            LevelManager.instance.RestartAllLevels();

        }
        else
        {
            if(level != null)
                Destroy(level);
            level = Instantiate(levels[currentLevel]);
            currentLevel++;
            if (GameEvents.LevelChanged != null)
                GameEvents.LevelChanged(currentLevel);
        }
    }
}

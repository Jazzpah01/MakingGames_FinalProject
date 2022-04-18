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
            Debug.Log("There are no more levels!");
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

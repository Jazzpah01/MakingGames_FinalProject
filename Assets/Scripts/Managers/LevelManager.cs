using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<GameObject> levels;
    private int currentLevel = 0;
    private GameObject level;

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

    private void Start()
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
        }
    }
}

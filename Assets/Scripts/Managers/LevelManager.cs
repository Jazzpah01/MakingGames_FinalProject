using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<GameObject> levels;
    private int currentLevel;
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

    void Start()
    {
        currentLevel = 0;
        loadFirstLevel();
    }

    void loadFirstLevel()
    {
        GameObject lvl = Instantiate(levels[currentLevel]) as GameObject;
        level = lvl;
    }

    public void loadNextLevel()
    {
        if (currentLevel + 1 >= levels.Count)
        {
            Debug.Log("There are no more levels!");
        }
        else
        {
            Destroy(level);
            GameObject lvl = Instantiate(levels[currentLevel + 1]) as GameObject;
            level = lvl;

            currentLevel += 1;
        }
    }
}

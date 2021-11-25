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

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        loadFirstLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadFirstLevel()
    {
        GameObject lvl = Instantiate(levels[currentLevel]) as GameObject;
        level = lvl;
    }

    public void loadNextLevel()
    {
        Destroy(level);
        GameObject lvl = Instantiate(levels[currentLevel+1]) as GameObject;
        level = lvl;

        currentLevel += 1;
    }
}

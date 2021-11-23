using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public bool inBattle = false;

    public int resource;

    [Header("Controller References")]
    public GameController gameController;
    public SpawnController spawnController;
    public BuildingController buildingController;


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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MBStateMachine
{
    public enum GameState
    {
        None,
        Combat,
        Strategy
    }

    public delegate void OnChangeState();
    public OnChangeState OnChangeToCombat;
    public OnChangeState OnChangeToBuilding;

    public static GameController instance;

    public SpawnController spawnController;

    public bool keyboardControl = false;
    [System.NonSerialized] public GameState state = GameState.None;
    public GameObject enemyParent;
    public float buildTimer = 9001;
    public GameObject gameOverScreen;

    public bool InWave => spawnController.InWave;
    private bool oldInWave = false;
    private int nextWave = 0;
    [HideInInspector]
    public int currentWave = 0;

    private float buildTime;
    private BuildingController buildingController;
    private PlayerController playerController;
    private PlayerManager playerManager;
    private GameManager gameManager;
    private HUD hud;
    private GameObject player;

    void Awake()
    {
        instance = this;
        buildTime = buildTimer;
    }

    public void Start()
    {
        playerManager = PlayerManager.instance;
        gameManager = GameManager.instance;
        buildingController = gameManager.buildingController;
        player = playerManager.player;
        playerController = playerManager.playerController;
        hud = gameManager.hud;
        ChangeState(GameState.Strategy);
    }

    internal void GameOver()
    {
        Time.timeScale = 0;
        hud.GameOver();
    }

    public void ChangeState(GameState newState)
    {
        if (newState == state)
            return;

        switch (newState)
        {
            case GameState.Combat:
                base.ChangeState(playerController);
                if (OnChangeToCombat != null)
                    OnChangeToCombat();
                break;
            case GameState.Strategy:
                base.ChangeState(buildingController);
                if (OnChangeToBuilding != null)
                    OnChangeToBuilding();
                gameManager.waveBuildingList.Clear();
                break;
            default:
                throw new System.Exception("Cannot have None state!");
        }
        state = newState;
    }

    private void Update()
    {
        if (!InWave && oldInWave)
        {
            gameManager.currentResource += gameManager.roundResource;
        }
        if (state == GameState.Combat)
        {
            if (InWave == false)
            {
                if (GameEvents.WaveFinished != null)
                    GameEvents.WaveFinished(currentWave);
                ChangeState(GameState.Strategy);
                currentWave++;
                hud.UpdateHUD();
            }
        }
        else if (state == GameState.Strategy)
        {

        }

        oldInWave = InWave;
        base.Update();
    }
    public void GoToBattle()
    {
        if (oldInWave == false)
        {
            // Start wave
            gameManager.hud.mouseOver = false;
            spawnController.StartWave(nextWave);
            nextWave++;
            oldInWave = true;
        }
        ChangeState(GameState.Combat);
        hud.UpdateHUD();
    }

    public int getNextWave()
    {
        return nextWave;
    }
}
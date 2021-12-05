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

    [HideInInspector]
    public GameObject player;

    public bool keyboardControl = false;
    [System.NonSerialized] public GameState state = GameState.None;
    public GameObject enemyParent;
    public float buildTimer = 9001;
    public GameObject gameOverScreen;

    public HUD hUD;
    public bool InWave => spawnController.InWave;
    private bool oldInWave = false;
    private int nextWave = 0;

    private float buildTime;
    private BuildingController buildingController;
    private PlayerController playerController;
    private PlayerManager playerManager;
    private GameManager gameManager;

    public Base baseController;

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

        ChangeState(GameState.Strategy);
    }

    internal void GameOver()
    {
        //TODO: something else than just showing a game over screen
        gameOverScreen.SetActive(true);
        //pause the time;
        Time.timeScale = 0;
    }

    public void ChangeState(GameState newState)
    {
        if (newState == state)
            return;

        switch (newState)
        {
            case GameState.Combat:
                if (!playerManager.buildWhileCombat)
                    player.SetActive(true);
                base.ChangeState(playerController);
                if (OnChangeToCombat != null)
                    OnChangeToCombat();
                break;
            case GameState.Strategy:
                if (state == GameState.None)
                {
                    gameManager.currentResource = gameManager.roundResource;
                }
                if (!playerManager.buildWhileCombat)
                    player.SetActive(false);
                base.ChangeState(buildingController);
                if (OnChangeToBuilding != null)
                    OnChangeToBuilding();
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
                ChangeState(GameState.Strategy);
                hUD.UpdateHUD();
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
            spawnController.StartWave(nextWave);
            nextWave++;
            oldInWave = true;
        }
        ChangeState(GameState.Combat);
        hUD.UpdateHUD();
    }

    public int getNextWave()
    {
        return nextWave;
    }
}
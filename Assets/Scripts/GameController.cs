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

    public static GameController instance;

    public SpawnController spawnController;

    [HideInInspector]
    public GameObject player;

    public bool keyboardControl = false;
    public GameState state = GameState.Strategy;
    public GameObject enemyParent;
    public float buildTimer = 9001;
    public GameObject gameOverScreen;

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

    private void Start()
    {
        playerManager = PlayerManager.instance;
        gameManager = GameManager.instance;
        buildingController = gameManager.buildingController;
        player = playerManager.player;
        playerController = playerManager.playerController;

        ChangeState(state);
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
        switch (newState)
        {
            case GameState.Combat: 
                if (!playerManager.buildWhileCombat)
                    player.SetActive(true);
                base.ChangeState(playerController);
                break;
            case GameState.Strategy:
                if (!playerManager.buildWhileCombat)
                    player.SetActive(false);
                base.ChangeState(buildingController);
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
            gameManager.resource += buildingController.roundResource;
        }
        if (state == GameState.Combat)
        {
            if (InWave == false)
            {
                ChangeState(GameState.Strategy);
                gameManager.inBattle = false;
            } else if (Input.GetKeyDown(KeyCode.B) && playerManager.buildWhileCombat)
            {
                ChangeState(GameState.Strategy);
            }
        }
        else if (state == GameState.Strategy)
        {
            //if (Input.GetKeyDown(KeyCode.B))
            //{
            //    if (!InWave)
            //    {
            //        // Start wave
            //        GoToBattle();
            //    }
            //    ChangeState(GameState.Combat);
            //}
        }


        oldInWave = InWave;
        base.Update();
    }
    public void GoToBattle()
    {
        if (oldInWave == false)
        {
            // Start wave
            gameManager.inBattle = true;
            spawnController.SpawnEnemies(nextWave);
            nextWave++;
            oldInWave = true;
        }
        ChangeState(GameState.Combat);
    }
}
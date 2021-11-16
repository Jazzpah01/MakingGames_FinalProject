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

    [HideInInspector]
    public GameObject player;

    public bool keyboardControl = false;
    public GameState state = GameState.Strategy;
    private bool inWave = false;
    public StrategyController strategyController;
    public GameObject enemyParent;
    public float buildTimer = 9001;
    public GameObject gameOverScreen;

    private float buildTime;
    private PlayerController playerController;

    public SpawnController spawnController;

    public Base baseController;

    void Awake()
    {
        instance = this;
        buildTime = buildTimer;
    }

    private void Start()
    {
        player = PlayerManager.instance.player;
        playerController = player.GetComponent<PlayerController>();

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
                if (!PlayerManager.instance.buildWhileCombat)
                    player.SetActive(true);
                base.ChangeState(playerController);
                break;
            case GameState.Strategy:
                if (!PlayerManager.instance.buildWhileCombat)
                    player.SetActive(false);
                base.ChangeState(strategyController);
                break;
            default:
                throw new System.Exception("Cannot have None state!");
        }
        state = newState;
    }

    private void Update()
    {
        if (!(enemyParent.transform.childCount > 0) && inWave == true)
        {
            inWave = false;
            strategyController.resource += strategyController.roundResource;
        }
        if (state == GameState.Combat)
        {
            if (inWave == false)
            {
                ChangeState(GameState.Strategy);
            } else if (Input.GetKeyDown(KeyCode.B) && PlayerManager.instance.buildWhileCombat)
            {
                ChangeState(GameState.Strategy);
            }
        }
        else if (state == GameState.Strategy)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (inWave == false)
                {
                    // Start wave
                    spawnController.SpawnEnemies();
                    inWave = true;
                }
                ChangeState(GameState.Combat);
            }
        }

        

        base.Update();
    }
}
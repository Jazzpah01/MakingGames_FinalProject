using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MBStateMachine
{
    public enum GameState
    {
        Combat,
        Strategy
    }

    public static GameController instance;

    public bool keyboardControl = false;
    public GameState state = GameState.Combat;
    public StrategyController strategyController;
    public GameObject player;
    public GameObject enemyParent;
    public float buildTimer = 9001;

    private float buildTime;
    private PlayerController playerController;

    public SpawnController spawnController;

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
        buildTime = buildTimer;
        instance = this;

        ChangeState(state);
    }

    public void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Combat:
                player.SetActive(true);
                base.ChangeState(playerController);
                break;
            case GameState.Strategy:
                player.SetActive(false);
                base.ChangeState(strategyController);
                break;
        }
        state = newState;
    }

    private void Update()
    {
        if (state == GameState.Combat)
        {
            if (!(enemyParent.transform.childCount > 0))
            {
                buildTime = buildTimer;
                ChangeState(GameState.Strategy);
            }
        }
        else if (state == GameState.Strategy)
        {
            buildTime -= Time.deltaTime;
            if (buildTime <= 0)
            {
                spawnController.SpawnEnemies();
                ChangeState(GameState.Combat);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                spawnController.SpawnEnemies();
                ChangeState(GameState.Combat);
            }
        }
        base.Update();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MBStateMachine
{
    public enum GameState
    {
        Combat,
        Strategy
    }

    public static PlayerManager instance;

    public bool keyboardControl = false;
    public GameState state = GameState.Combat;

    public PlayerController playerController;
    public StrategyController strategyController;

    public GameObject enemyParent;
    public float buildTimer = 9001;
    private float buildTime;

    public SpawnController spawner;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            buildTime = buildTimer;
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        ChangeState(state);
    }

    public void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Combat:
                base.ChangeState(playerController);
                break;
            case GameState.Strategy:
                base.ChangeState(strategyController);
                break;
        }
    }

    private void Update()
    {
        if (state == GameState.Combat)
        {
            if (enemyParent.GetComponentsInChildren<EnemyController>().Length < 1)
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
                spawner.SpawnEnemies();
                ChangeState(GameState.Combat);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                spawner.SpawnEnemies();
                ChangeState(GameState.Combat);
            }
        }
        base.Update();
    }
}
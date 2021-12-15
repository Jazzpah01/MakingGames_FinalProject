using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Security.Cryptography;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    private EnemyList enemyList;
    public List<WaveData> waves;

    public Transform spawnPointParent;
    private List<Transform> spawnPoints;

    [Header("References")]
    public GameObject enemyParent;
    public GameObject baseGO;

    private Projector projectorController;
    private GameController gameController;
    private LevelManager levelManager;

    public int CurrentWave
    {
        get;
        protected set;
    }

    public float CurrentCombatValue
    {
        get
        {
            IEnemy[] actors = enemyParent.GetComponentsInChildren<IEnemy>();
            if (actors == null || actors.Length == 0)
            {
                return 0;
            } else
            {
                float retval = 0;
                foreach(IEnemy a in actors)
                {
                    retval += a.enemyType.value;
                }
                return retval;
            }
        }
    }

    public bool InWave
    {
        get
        {
            return (!(enemyParent.GetComponentsInChildren<IActor>() == null ||
                enemyParent.GetComponentsInChildren<IActor>().Length == 0));
        }
    }

    private List<EnemyType> toSpawn;
    private float combatValue;

    void Start()
    {
        levelManager = LevelManager.instance;
        gameController = GameController.instance;
        enemyList = GameManager.instance.enemyTypes;
        CurrentWave = 0;
        toSpawn = new List<EnemyType>();
        projectorController = GetComponentInChildren<Projector>();

        spawnPoints = new List<Transform>();

        int count = 0;
        foreach (Transform child in spawnPointParent)
        {
            // If statement added to make sure child is added is not a grandchild
            if (child.parent == spawnPointParent.transform)
            {
                spawnPoints.Add(child);
                count++;
            }
        }
        //print(count);
    }

    private void Update()
    {
        if (InWave == false)
            return;

        if(LevelManager.instance != null && gameController.getNextWave() > waves.Count)
        {
            levelManager.loadLevel();
        }
        Spawn();
    }

    private void Spawn()
    {
        while (toSpawn.Count > 0 && CurrentCombatValue < waves[CurrentWave].combatValue)
        {
            // Spawn newest
            GameObject go = Instantiate(toSpawn[0].prefab);

            IEnemy enemy = go.GetComponent<IEnemy>();

            enemy.enemyType = toSpawn[0];

            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)].gameObject;

            go.transform.position = spawnPoint.transform.position;
            go.transform.parent = enemyParent.transform;
            enemy.spawnPoint = spawnPoint;

            toSpawn.RemoveAt(0);
        }
    }

    public void StartWave(int waveIndex) {
        if (waves == null || waves.Count < 1)
            throw new System.Exception("Cannot spawn a wave in a level with no waves!");

        if (waveIndex >= waves.Count)
            waveIndex = waves.Count - 1;

        CurrentWave = waveIndex;

        WaveData waveData = waves[CurrentWave];

        float remainingValue = waveData.totalValue;
        combatValue = 0;
        toSpawn.Clear();

        for (int i = 0; i < waveData.enemyWeights.Count; i++)
        {
            float totalPrefabValue = waveData.GetNormalizedWeight(i) * waveData.totalValue;
            while (totalPrefabValue > 0)
            {
                EnemyType enemyType = GetEnemyType(CurrentWave, i);
                toSpawn.Add(enemyType);
                totalPrefabValue -= enemyType.value;
            }
        }

        Shuffle<EnemyType>(toSpawn);

        Spawn();

        GameManager.instance.currentResource += waveData.addRoundResource;
    }

    private EnemyType GetEnemyType(int waveIndex, int weightIndex)
    {
        return enemyList[waves[waveIndex].enemyWeights[weightIndex].prefab];
    }

    /// <summary>
    /// https://stackoverflow.com/questions/273313/randomize-a-listt
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    private static void Shuffle<T>(List<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

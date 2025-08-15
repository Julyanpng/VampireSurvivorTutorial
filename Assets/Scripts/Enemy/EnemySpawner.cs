using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // List of enemies to spawn in this wave
        public int waveQuota; //total number of enemies to spawn in this wave
        public float spawnInterval; //the interval at which to spawn enemies
        public int spawnCount; //The number of enemies already spawned in this wave
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //the number of enemies to spawn in this wave
        public int spawnCount; // the number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }

    public List<Wave> waves; //A list of all the waves in the game
    public int currentWaveCount; //index of the current wave

    [Header("Spawner Attributes")]
    float spawnTimer; //Timer use to determine when to spawn the next enemy
    public int enemiesAlive;
    public int maxEnemiesAllowed; //the maximum number of enemies allowed on the map at once
    public bool maxEnemiesReached = false; //A flap indicating if the max amount of enemies has been reached
    public float waveInterval; //the interval between each wave
    bool isWaveActive = false;

    [Header("Spawn Postions")]
    public List<Transform> relativeSpawnPoints; //list to store all relative spawn points of enemies

    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0 && !isWaveActive)
        {
            StartCoroutine(BeginNextWave());
        }


        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        isWaveActive = true; // prevent multiple calls

        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            isWaveActive = false;
            currentWaveCount++;
            CalculateWaveQuota();
        }

    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach(var enemyGroup in waves[currentWaveCount].enemyGroups) 
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }

    /// <summary>
    /// This method willl stop spawning enemies if the amount of enemies on the map is maxmimum.
    /// The method will only spawn enemies in a particular wave until it is time for the next wave's enemies to be spawned
    /// </summary>

    void SpawnEnemies()
    {
        //check if minimum number of enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //spawn each type of enemy in the wave until the quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //checks i8f the minimum number of enemies of this type have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    //spawn the enemy at random positions close to the player
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                }
            }
        }
    }

    public void OnEnemyKilled()
    {
        //decrement number of enemies alive
        enemiesAlive--;

        //reset the maxenemiesReached flag if the number of enemies alive has dropped below the maximum amount
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
}

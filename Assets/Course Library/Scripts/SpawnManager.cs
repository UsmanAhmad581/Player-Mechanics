using UnityEngine;
using TMPro;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    //public GameObject BossPrefab;
    private float bossSpawnTime;
    private bool miniEnemiesSpawned = false;
    public GameObject[] Enemies;
    private float spawnRange = 9f;
    public int enemyCount;
    public int waveNumber = 1;

    public GameObject bossPrefab;
    public GameObject[] miniEnemyPrefabs;
    public int bossRound;

    // =====================================================
    // CHANGED:
    // Changed powerupPrefab to powerupPrefabs (array).
    // This allows us to have multiple types of power-ups,
    // such as Pushback and Rockets.
    // =====================================================
    public GameObject[] powerupPrefabs;

    // public GameObject ProjectilePrefab;
    // private float ProjectileSpawnTime = 0.5f;

    public TextMeshProUGUI GameOverText;

    private PlayerController playerControllerScript;


    // =====================================================
    // START
    // =====================================================
    void Start()
    {
        // Get the PlayerController from the Player object
        playerControllerScript =
            GameObject.Find("Player").GetComponent<PlayerController>();


        // =================================================
        // NEW POWER-UP SPAWNING
        // =================================================

        // Select a random power-up from the powerupPrefabs array
        int randomPowerup =
            Random.Range(0, powerupPrefabs.Length);

        // Spawn the randomly selected power-up
        Instantiate(
            powerupPrefabs[randomPowerup],
            GenerateSpawnPosition(),
            powerupPrefabs[randomPowerup].transform.rotation
        );


        // Spawn the first enemy wave
        SpawnEnemyWave(waveNumber);
    }


    // =====================================================
    // GENERATE RANDOM SPAWN POSITION
    // =====================================================

    // Generates a random position within the spawn range
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX =
            Random.Range(-spawnRange, spawnRange);

        float spawnPosZ =
            Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos =
            new Vector3(spawnPosX, 0, spawnPosZ);

        return randomPos;
    }
    void SpawnBossWave(int currentRound)
    {
     int miniEnemysToSpawn;
     //We dont want to divide by 0!
     if (bossRound != 0)
     {
     miniEnemysToSpawn = currentRound / bossRound;
     }
     else
     {
     miniEnemysToSpawn = 1;
     }
     var boss = Instantiate(bossPrefab, GenerateSpawnPosition(),
     bossPrefab.transform.rotation);
     boss.GetComponent<EnemyFollow>().miniEnemySpawnCount = miniEnemysToSpawn;
     }
     public void SpawnMiniEnemy(int amount)
    {
    for (int i = 0; i < amount; i++)
    {
        int randomMini = Random.Range(0, miniEnemyPrefabs.Length);

        Instantiate(
            miniEnemyPrefabs[randomMini],
            GenerateSpawnPosition(),
            miniEnemyPrefabs[randomMini].transform.rotation
        );
    }
}

    // =====================================================
    // SPAWN ENEMY WAVE
    // =====================================================

    // Spawns a wave of enemies based on the wave number
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(
                Enemies[RandomEnemy()],
                GenerateSpawnPosition(),
                Enemies[RandomEnemy()].transform.rotation
            );
        }
    }

    // =====================================================
    // RANDOM ENEMY
    // =====================================================

    // Randomly selects an enemy from the array of enemies to spawn
    int RandomEnemy()
    {
        int RandomEnemyindex =
            Random.Range(0, Enemies.Length);

        return RandomEnemyindex;
    }


    // =====================================================
    // RANDOM POWER-UP
    // =====================================================

    // Randomly selects a powerup from the array of powerups
    // This method is kept from your original script.
    // The new Start() and Update() directly use Random.Range,
    // as requested in the new instructions.
    int RandomPowerup()
    {
        int RandomPowerupindex =
            Random.Range(0, powerupPrefabs.Length);

        return RandomPowerupindex;
    }


    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {

        // Count all enemies currently in the scene
        enemyCount =
            FindObjectsByType<EnemyFollow>(
                FindObjectsSortMode.None
            ).Length;


        // =================================================
        // NEW WAVE + RANDOM POWER-UP
        // =================================================

        if (enemyCount == 0)
        {
            // Increase the wave number
            waveNumber++;

                           // Spawn a boss every x number of waves
           if (waveNumber % bossRound == 0)
           {
             StartCoroutine(SpawnBossAndMiniEnemies()); 
            }
            else
            {
            SpawnEnemyWave(waveNumber);
            }


            // Select a random power-up
            int randomPowerup =
                Random.Range(0, powerupPrefabs.Length);


            // Spawn the randomly selected power-up
            Instantiate(
                powerupPrefabs[randomPowerup],
                GenerateSpawnPosition(),
                powerupPrefabs[randomPowerup].transform.rotation
            );
        }


        // =================================================
        // GAME OVER
        // =================================================

        if (playerControllerScript.CheckPlayerDestroyed == true)
        {
            GameOver();
        }
    }
    IEnumerator SpawnBossAndMiniEnemies()
    {
       SpawnBossWave(waveNumber);

       yield return new WaitForSeconds(5f);

       SpawnMiniEnemy(4);
    }


    // =====================================================
    // GAME OVER
    // =====================================================

    void GameOver()
    {
        GameOverText.gameObject.SetActive(true);
    }
}

using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{  
    //public GameObject BossPrefab;
    public GameObject[] Enemies;
    private float spawnRange = 9f;
    public int enemyCount;
    public int waveNumber = 1;
    public GameObject[] powerupPrefab;
    // public GameObject ProjectilePrefab;
    // private float ProjectileSpawnTime = 0.5f;

    public TextMeshProUGUI GameOverText;

    private PlayerController playerControllerScript;
    
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        spawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab[RandomPowerup()], GenerateSpawnPosition(), powerupPrefab[RandomPowerup()].transform.rotation);
    }
    //Generates a random position within the spawn range
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
    //Spawns a wave of enemies based on the wave number
    void spawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(Enemies[RandomEnemy()], GenerateSpawnPosition(), Enemies[RandomEnemy()].transform.rotation);
        }
    }
    // void spawnProjectile()
    // {
    //     Instantiate(ProjectilePrefab, ProjectilePrefab.transform.position, ProjectilePrefab.transform.rotation);
    // }


    //Randomly selects an enemy from the array of enemies to spawn
    int RandomEnemy()
    {
        int RandomEnemyindex = Random.Range(0, Enemies.Length);
        return RandomEnemyindex;
    }
    //Randomly selects a powerup from the array of powerups to spawn
    int RandomPowerup()
    {
        int RandomPowerupindex = Random.Range(0, powerupPrefab.Length);
        return RandomPowerupindex;
    }
    // Update is called once per frame
    void Update()
    {   
        // if (Time.time > ProjectileSpawnTime)
        // {
        //     spawnProjectile();
        //     ProjectileSpawnTime = Time.time + 0.5f;
        // }
       
        enemyCount = FindObjectsByType<EnemyFollow>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0 && playerControllerScript.CheckPlayerDestroyed == false)
        {   
            waveNumber++;
            spawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab[RandomPowerup()], GenerateSpawnPosition(), powerupPrefab[RandomPowerup()].transform.rotation);
        }
        if (playerControllerScript.CheckPlayerDestroyed == true)
        {
            GameOver();
        }
    }
    void GameOver()
    {
        GameOverText.gameObject.SetActive(true);
    }
}

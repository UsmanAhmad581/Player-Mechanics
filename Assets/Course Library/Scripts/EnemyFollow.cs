using UnityEngine;

public class EnemyFollow : MonoBehaviour
{   
    private GameObject player;
    public float speed = 5f;
    public Rigidbody enemyRb;
    private float bottomBound = -10f;

    public  bool isBoss = false;
    public float spawnInterval;
    private float nextspawn;

    public int miniEnemySpawnCount;

    private SpawnManager spawnManagerScript;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        if(isBoss)
        {
            spawnManagerScript = FindObjectOfType<SpawnManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {   
          Vector3 lookDirection = (player.transform.position - transform.position).normalized;
           enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
        
        if (isBoss)
        {
            if (Time.time > nextspawn)
            {
                nextspawn = Time.time + spawnInterval;
                spawnManagerScript.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }
        if (transform.position.y < bottomBound)
        {
            Destroy(gameObject);
        }
        
    }
    
}

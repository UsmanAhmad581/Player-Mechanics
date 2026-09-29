using UnityEngine;

public class EnemyFollow : MonoBehaviour
{   
    private GameObject player;
    public float speed = 5f;
    public Rigidbody enemyRb;
    private float bottomBound = -10f;
    private PlayerController playerControllerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {   
        if(playerControllerScript.CheckPlayerDestroyed == false)
        {
          Vector3 lookDirection = (player.transform.position - transform.position).normalized;
           enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
        }
        if (transform.position.y < bottomBound)
        {
            Destroy(gameObject);
        }
    }
}

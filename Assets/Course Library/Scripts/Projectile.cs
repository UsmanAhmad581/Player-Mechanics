using UnityEngine;

public class Projectile : MonoBehaviour
{   
    private GameObject EnemyPrefab;
    public Rigidbody projectileRb;
    public float speed = 10f;
    private EnemyFollow enemyFollowScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        enemyFollowScript = GameObject.Find("Enemy").GetComponent<EnemyFollow>();
        projectileRb = GetComponent<Rigidbody>();
        EnemyPrefab = GameObject.Find("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (EnemyPrefab.transform.position - transform.position).normalized;
        projectileRb.AddForce(lookDirection * speed * Time.deltaTime);
    }
   void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyFollowScript.transform.Translate(new Vector3(-200f, 0, 0) * Time.deltaTime, Space.World);
            Destroy(gameObject);
        }
    }

}

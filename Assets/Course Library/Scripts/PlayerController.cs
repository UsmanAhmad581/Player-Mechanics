using UnityEngine;
using System.Collections;
using TMPro;
public class PlayerController : MonoBehaviour
{  
    public float speed = 200f;
    private InputSystem_Actions controls;
    public Rigidbody rb;
    private GameObject focalPoint;

    private float BottomBound = -10f;

    public bool hasPowerup;
    // public bool hasProjectile;
    private float powerupStrength = 15f;

    public GameObject powerupIndicator;
    // public GameObject projectilePrefab;
    public bool CheckPlayerDestroyed = false;

    // private float ProjectileSpawnTime = 0.3f;
    // private float stopTime = 7f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");

    }

    void OnEnable()
    {
        controls.Player.Enable();
    }
    void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float Forward = moveInput.y;
        rb.AddForce(focalPoint.transform.forward * Forward * speed * Time.deltaTime);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        CheckPlayerDestroyed = DestroyPlayer();

        // if (Time.time > ProjectileSpawnTime && hasProjectile && CheckPlayerDestroyed == false)
        // {   
        //     Instantiate(projectilePrefab, transform.position + new Vector3(0, 0.3f, 0.5f), projectilePrefab.transform.rotation);
        //     ProjectileSpawnTime = Time.time + 0.3f;
        //     StartCoroutine(PowerupCountdownRoutine());

        
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
        if (other.CompareTag("Fire"))
        {
            // hasProjectile = true;
            Destroy(other.gameObject);
        }
    }
    IEnumerator PowerupCountdownRoutine()
    {   
        if (hasPowerup)
        {
            yield return new WaitForSeconds(7);
            hasPowerup = false;
            powerupIndicator.SetActive(false);
        }
        // if(hasProjectile)
        // {
        //     yield return new WaitForSeconds(7);
        //     hasProjectile = false;
        // }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
    bool DestroyPlayer()
    {   
        if (transform.position.y < BottomBound)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    
} 

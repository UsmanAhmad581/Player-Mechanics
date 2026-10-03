using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
 

public class PlayerController : MonoBehaviour
{
    // =========================
    // PLAYER VARIABLES
    // =========================
    public float speed = 200f;
    private InputSystem_Actions controls;
    public Rigidbody rb;
    private GameObject focalPoint;

    private float BottomBound = -10f;


    // =========================
    // POWER-UP VARIABLES
    // =========================

    // Old power-up boolean
    public bool hasPowerup;

    // Stores which type of power-up the player currently has.
    // Examples: None, Pushback, Rockets, Smash
    public PowerUpType currentPowerUp = PowerUpType.None;

    // Strength of the Pushback power-up
    private float powerupStrength = 15f;


    // =====================================================
    // 🔴 SMASH CHANGE 1: SMASH VARIABLES
    // =====================================================

    // How long the player stays in the air
    public float hangTime = 0.5f;

    // Speed used when going up/down
    public float smashSpeed = 15f;

    // How strongly enemies are pushed away
    public float explosionForce = 40f;

    // How far the Smash attack affects enemies
    public float explosionRadius = 15f;

    // Checks whether the player is currently performing Smash
    private bool smashing = false;

    // Stores the player's original Y position
    private float floorY;


    // Shows the power-up indicator underneath the player
    public GameObject powerupIndicator;

    // The homing rocket prefab that will be spawned
    public GameObject rocketPrefab;

    // Temporary variable used when spawning a rocket
    private GameObject tmpRocket;

    // Stores the power-up countdown coroutine
    private Coroutine powerupCountdown;


    // =========================
    // PLAYER DESTROYED
    // =========================
    public bool CheckPlayerDestroyed = false;
    public GameManager gameManager;

    // =========================
    // AWAKE
    // =========================
    void Awake()
    {
        // Create the Input System controls
        controls = new InputSystem_Actions();

        // Get the Rigidbody attached to the player
        rb = GetComponent<Rigidbody>();

        // Find the Focal Point in the scene
        focalPoint = GameObject.Find("Focal Point");

    }

    // =========================
    // ENABLE INPUT
    // =========================
     private void OnEnable()
    {
       if (controls != null)
    {
        controls.Player.Enable();
    }
   }

    private void OnDisable()
    {
     if (controls != null)
     {
        controls.Player.Disable();
     }
    }
   


    // =========================
    // UPDATE
    // =========================
    void Update()
    {   if (controls == null)
           return;

       if (!isActiveAndEnabled)
            return;

        // Read movement input from the new Input System
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();

        // Get forward/backward input
        float Forward = moveInput.y;

        // Move the player in the direction of the Focal Point
        rb.AddForce(
            focalPoint.transform.forward *
            Forward *
            speed *
            Time.deltaTime
        );

        // Keep the power-up indicator underneath the player
        powerupIndicator.transform.position =
            transform.position + new Vector3(0, -0.5f, 0);


        // Check whether the player has fallen off the platform
        CheckPlayerDestroyed = DestroyPlayer();


        // =========================
        // ROCKET POWER-UP
        // =========================

        // If the current power-up is Rockets
        // AND the player presses F,
        // launch rockets at all enemies.
        if (currentPowerUp == PowerUpType.Rockets &&
           controls.Player.Rocket.WasPressedThisFrame())
       {
             LaunchRockets();
        }


        // =====================================================
        // 🔴 SMASH CHANGE 2: SMASH INPUT
        // =====================================================

        // Check:
        // 1. Player currently has Smash power-up
        // 2. Space key is pressed
        // 3. Player is NOT already smashing
         if (currentPowerUp == PowerUpType.Smash &&
            controls.Player.Jump.WasPressedThisFrame() &&
            !smashing)
         {
            // Player is now performing Smash
            smashing = true;

            // Start the Smash coroutine
            StartCoroutine(Smash());
        }
    }


    // =========================
    // POWER-UP COLLISION
    // =========================
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collected a power-up
        if (other.CompareTag("Powerup"))
        {
            // Player now has a power-up
            hasPowerup = true;

            // Get the type of power-up
            currentPowerUp =
                other.gameObject.GetComponent<PowerUp>().powerUpType;


            // Show the indicator for Pushback and Smash
            if (other.gameObject.GetComponent<PowerUp>().powerUpType == PowerUpType.Pushback
                || other.gameObject.GetComponent<PowerUp>().powerUpType == PowerUpType.Smash)
            {
                powerupIndicator.gameObject.SetActive(true);
            }


            // Remove the collected power-up from the scene
            Destroy(other.gameObject);


            // =========================
            // RESET POWER-UP TIMER
            // =========================

            // If a previous power-up countdown is already running,
            // stop it before starting a new one.
            if (powerupCountdown != null)
            {
                StopCoroutine(powerupCountdown);
            }

            // Start a new 7-second countdown
            powerupCountdown = StartCoroutine(
                PowerupCountdownRoutine()
            );
        }
    }


    // =========================
    // POWER-UP COUNTDOWN
    // =========================
    IEnumerator PowerupCountdownRoutine()
    {
        // Power-up remains active for 7 seconds
        yield return new WaitForSeconds(7);

        // Remove the power-up
        hasPowerup = false;

        // Reset the power-up type back to None
        currentPowerUp = PowerUpType.None;

        // Hide the power-up indicator
        powerupIndicator.gameObject.SetActive(false);

        // Reset the coroutine reference
        powerupCountdown = null;
    }


    // =========================
    // ENEMY COLLISION
    // =========================
    private void OnCollisionEnter(Collision collision)
    {
        // Push the enemy only when the current power-up
        // is specifically the Pushback power-up.
        if (collision.gameObject.CompareTag("Enemy") &&
            currentPowerUp == PowerUpType.Pushback)
        {
            // Get the enemy's Rigidbody
            Rigidbody enemyRigidbody =
                collision.gameObject.GetComponent<Rigidbody>();

            // Calculate the direction away from the player
            Vector3 awayFromPlayer =
                collision.gameObject.transform.position -
                transform.position;

            // Push the enemy away from the player
            enemyRigidbody.AddForce(
                awayFromPlayer * powerupStrength,
                ForceMode.Impulse
            );
        }
    }


    // =========================
    // LAUNCH ROCKETS
    // =========================
    void LaunchRockets()
    {
        // Find every Enemy currently in the scene
        foreach (var enemy in FindObjectsOfType<EnemyFollow>())
        {
            // Create a rocket above the player
            // Vector3.up prevents the rocket from immediately
            // colliding with/pushing the player.
            tmpRocket = Instantiate(
                rocketPrefab,
                transform.position + Vector3.up,
                Quaternion.identity
            );

            // Tell the rocket which enemy it should follow.
            tmpRocket.GetComponent<RocketBehaviour>()
                .Fire(enemy.transform);
        }
    }


    // =====================================================
    // 🔴 SMASH CHANGE 3: NEW SMASH COROUTINE
    // =====================================================
    IEnumerator Smash()
    {
        // Find all enemies currently in the scene
        var enemies = FindObjectsOfType<EnemyFollow>();


        // -------------------------------------------------
        // Store the player's Y position before jumping
        // -------------------------------------------------
        floorY = transform.position.y;


        // -------------------------------------------------
        // Calculate how long the player should stay upward
        // -------------------------------------------------
        float jumpTime = Time.time + hangTime;


        // -------------------------------------------------
        // MOVE PLAYER UP
        // -------------------------------------------------
        while (Time.time < jumpTime)
        {
            // Keep the player's current X and Z movement,
            // but force the player upward.
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                smashSpeed,
                rb.linearVelocity.z
            );

            // Wait until the next frame
            yield return null;
        }


        // -------------------------------------------------
        // MOVE PLAYER DOWN
        // -------------------------------------------------
        while (transform.position.y > floorY)
        {
            // Keep X and Z velocity,
            // but move strongly downward.
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                -smashSpeed * 2,
                rb.linearVelocity.z
            );

            // Wait until the next frame
            yield return null;
        }


        // -------------------------------------------------
        // SMASH / EXPLOSION EFFECT
        // -------------------------------------------------
        for (int i = 0; i < enemies.Length; i++)
        {
            // Make sure the enemy still exists
            if (enemies[i] != null)
            {
                // Get enemy Rigidbody
                Rigidbody enemyRigidbody =
                    enemies[i].GetComponent<Rigidbody>();

                // Make sure the enemy has a Rigidbody
                if (enemyRigidbody != null)
                {
                    // Push the enemy away from the player
                    enemyRigidbody.AddExplosionForce(
                        explosionForce,
                        transform.position,
                        explosionRadius,
                        0.0f,
                        ForceMode.Impulse
                    );
                }
            }
        }


        // -------------------------------------------------
        // Smash is finished
        // -------------------------------------------------
        smashing = false;
    }


    // =========================
    // DESTROY PLAYER
    // =========================
    private bool DestroyPlayer()
    {   
        // Check if the player has fallen below the boundary
        if (transform.position.y < BottomBound)
        {   
            gameManager.GameOver();
              return true;
        }

        return false;
    }
}

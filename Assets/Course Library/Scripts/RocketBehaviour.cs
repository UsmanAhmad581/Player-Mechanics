using UnityEngine;

public class RocketBehaviour : MonoBehaviour
{
    // The object the rocket will follow
    private Transform target;

    // Rocket speed
    private float speed = 15.0f;

    // Is the rocket following a target?
    private bool homing;

    // How strongly the rocket pushes the target
    private float rocketStrength = 15.0f;

    // Rocket will disappear after 5 seconds
    private float aliveTimer = 5.0f;


    void Update()
    {
        // Only move if we have a target
        if (homing && target != null)
        {
            // Find the direction from the rocket to the target
            Vector3 moveDirection =
                (target.position - transform.position).normalized;

            // Move the rocket toward the target
            transform.position += moveDirection * speed * Time.deltaTime;

            // Make the rocket face the target
            transform.LookAt(target);
        }
    }


    // This method is called when the player fires the rocket
    public void Fire(Transform newTarget)
    {
        // Store the target
        target = newTarget;

        // Start following the target
        homing = true;

        // Destroy rocket after 5 seconds
        Destroy(gameObject, aliveTimer);
    }


    // Runs when the rocket hits something
    void OnCollisionEnter(Collision col)
    {
        // Make sure we have a target
        if (target != null)
        {
            // Check if the object we hit has the same tag as our target
            if (col.gameObject.CompareTag(target.tag))
            {
                // Get the Rigidbody of the object we hit
                Rigidbody targetRigidbody =
                    col.gameObject.GetComponent<Rigidbody>();

                // Find the direction to push the object
                Vector3 away = -col.contacts[0].normal;

                // Push the target away
                targetRigidbody.AddForce(
                    away * rocketStrength,
                    ForceMode.Impulse
                );

                // Destroy the rocket
                Destroy(gameObject);
                
            }
        }
    }
}
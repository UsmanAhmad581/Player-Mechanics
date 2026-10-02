using UnityEngine;
public enum PowerUpType { None, Pushback, Rockets,Smash }
public class PowerUp : MonoBehaviour
{
public PowerUpType powerUpType;
public float rotateSpeed = 50f;

private void Update() 
{
    transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
}
}
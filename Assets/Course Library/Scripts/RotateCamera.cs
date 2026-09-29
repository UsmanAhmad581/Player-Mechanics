using UnityEngine;

public class RotateCamera : MonoBehaviour
{   
    private InputSystem_Actions controls;
    public float rotationSpeed = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }
    void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float Horizontal = moveInput.x;
        transform.Rotate(Vector3.up, Horizontal * rotationSpeed * Time.deltaTime);
        

    }
}

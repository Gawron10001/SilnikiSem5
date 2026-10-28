using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMotor : MonoBehaviour
{
    public float Acceleration = 10.0f;
    public float JumpForce = 10.0f;
    private Vector2 direction;
    private Rigidbody2D rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigidbody2D.AddForce(new Vector2(direction.x,0) * Acceleration);
    }

    void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    void OnJump()
    {
        rigidbody2D.AddForce(Vector2.up * JumpForce,ForceMode2D.Impulse);
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMotor : MonoBehaviour
{
    public float acceleration = 10.0f;
    public float jumpForce = 10.0f;
    public float stoppingPoint = 0.1f;
    public float stoppingForce = 10;
    private Vector2 _direction;
    private Rigidbody2D _rigidbody2D;
    public int maxJump = 1;
    private int _currentJump;
    public float maxSpeedX = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        LimitMaxSpeed();
    }
    
    private void LimitMaxSpeed()
    {
        //Limit max speed
        if (_rigidbody2D.linearVelocityX >= maxSpeedX)
        {
            _rigidbody2D.linearVelocityX = maxSpeedX;
        }
        else if (_rigidbody2D.linearVelocityX <= -maxSpeedX)
        {
            _rigidbody2D.linearVelocityX = -maxSpeedX;
        }
    }

    private void MovePlayer()
    {
        //accelerate if pressing button
        if (_direction.x != 0)
        {
            _rigidbody2D.AddForce(new Vector2(_direction.x * acceleration, 0));
        }
        //if not accelerating start slowing down
        else if (_rigidbody2D.linearVelocityX != 0)
        {
            //if almost stopped, force stop
            if (_rigidbody2D.linearVelocityX < stoppingPoint && _rigidbody2D.linearVelocityX > -stoppingPoint)
            {
                _rigidbody2D.linearVelocity = new Vector2(0.0f, _rigidbody2D.linearVelocityY);
            }
            //add stopping force
            else
            {
                _rigidbody2D.AddForce(new Vector2(-_rigidbody2D.linearVelocityX * stoppingForce, 0));
            }
        }
    }

    void OnMove(InputValue value)
    {
        _direction = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (_currentJump < maxJump)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            ++_currentJump;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _currentJump = 0;
    }
}

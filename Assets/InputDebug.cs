using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebug : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnJump()
    {
        Debug.Log("Jump");
    }

    void OnMove(InputValue value)
    {
        Debug.Log(value.Get<Vector2>());
    }
}

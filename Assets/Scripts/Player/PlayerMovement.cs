using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Inputs
    private CustomInput input = null;
    private Vector2 inputVec = Vector2.zero;

    // Movement
    Rigidbody2D rb;
    float moveLimiter = 0.8f;
    public float runSpeed = 20.0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Hold these values in their own floats
        // because if we were to affect the inputVec
        // itself then input would just stop after a while.
        float horizontal = inputVec.x;
        float vertical = inputVec.y;

        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // Limit the movement based off of moveLimiter,
            // currently at 80% feels nicer than 70% but is
            // still a little bit faster.
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        rb.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    // Input Handling
    private void Awake()
    {
        input = new CustomInput();
    }

    private void OnEnable()
    {
        // Enable input and subscribe to their needed methods.
        input.Enable();
        input.Player.Movement.performed += OnInputPerformed;
        input.Player.Movement.canceled += OnInputCancelled;
    }

    private void OnDisable()
    {
        // Disable input and unsubscribe to their methods.
        input.Disable();
        input.Player.Movement.performed -= OnInputPerformed;
        input.Player.Movement.canceled -= OnInputCancelled;
    }

    private void OnInputPerformed(InputAction.CallbackContext cbContext)
    {
        // Assign our inputVec to the player's inputs.
        inputVec = cbContext.ReadValue<Vector2>();
    }

    private void OnInputCancelled(InputAction.CallbackContext cbContext)
    {
        // Reset our inputVec upon release of a key
        inputVec = Vector2.zero;
    }
}

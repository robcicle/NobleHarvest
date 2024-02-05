using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Inputs
    private Vector2 inputVec = Vector2.zero;

    // Movement
    Rigidbody2D rb;
    public float runSpeed = 20.0f;

    private void Start()
    {
        // Assign our member rigidbody to our player's one.
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Simply add to our velocity the input vector multiplied
        // by our running speed to create movement.
        rb.velocity = inputVec * runSpeed;
    }

    // Input Handling
    public void HandleInput(InputAction.CallbackContext cbContext)
    {
        // inputVec = WASD/-1,0
        inputVec = cbContext.ReadValue<Vector2>();
    }
}

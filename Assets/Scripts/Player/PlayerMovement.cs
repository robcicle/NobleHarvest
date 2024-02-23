using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Inputs
    private Vector2 inputVec = Vector2.zero;

    // Movement
    Rigidbody2D rb;
    public float moveSpeed = 20.0f;
    bool inputDisabled = false;

    private void Start()
    {
        // Assign our member rigidbody to our player's one.
        rb = GetComponent<Rigidbody2D>();
    }

    // Called per physics update
    private void FixedUpdate()
    {
        // Simply add to our velocity the input vector multiplied
        // by our running speed to create movement.
        //rb.velocity = inputVec * moveSpeed;

        Vector2 moveVec = inputVec * moveSpeed;

        rb.AddForce(moveVec, ForceMode2D.Force);
    }

    // Input Handling
    public void HandleInput(InputAction.CallbackContext cbContext)
    {
        if (inputDisabled)
            return;

        // inputVec = WASD/-1,0
        inputVec = cbContext.ReadValue<Vector2>();
    }

    public void Knockback(Vector2 force)
    {
        //Debug.Log("Original Position" + transform.position);
        inputDisabled = true;
        rb.AddForce(force * 150);
        // Debug.Log("Knocked back Position" + transform.position);
        StartCoroutine(KnockbackReset());
    }

    IEnumerator KnockbackReset()
    {
        yield return new WaitForSeconds(0.1f);
        inputDisabled = false;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Inputs
    private Vector2 inputVec = Vector2.zero;

    // Movement
    Rigidbody2D rb;
    public float runSpeed = 20.0f;

    //used for knockback
    bool inputDisabled;

    private void Start()
    {
        // Assign our member rigidbody to our player's one.
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inputDisabled == false)
        {
            // Simply add to our velocity the input vector multiplied
            // by our running speed to create movement.
            rb.velocity = inputVec * runSpeed;
        }
    }

    // Input Handling
    public void HandleInput(InputAction.CallbackContext cbContext)
    {

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

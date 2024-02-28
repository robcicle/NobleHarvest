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
    float horizontal;
    float vertical;
    bool isFacingRight;

    Animator _characterAnimator; //reference to the character animator
    [SerializeField] AudioSource _playerAudio;
    int audioPlayCount = 1;
    
    private void Start()
    {
        // Assign our member rigidbody to our player's one.
        rb = GetComponent<Rigidbody2D>();
        _characterAnimator = GetComponent<Animator>();

    }

    public void Update()
    {
        _characterAnimator.SetFloat("Speed", Mathf.Abs(horizontal));
        _characterAnimator.SetFloat("VSpeed", Mathf.Abs(vertical));
        //Debug.Log("horizontal" + horizontal);
        //Debug.Log("Vertical" + vertical);

        if(horizontal > 0.01 || vertical > 0.01 || horizontal < -0.01f || vertical < -0.01f)
        {
            if (audioPlayCount > 0)
            {
               //Debug.Log("Playing audio");
                
                StartCoroutine(AudioCount());
           
            }
            _characterAnimator.SetBool("isMoving", true);
        }
        else
        {
            audioPlayCount = 1;
            _characterAnimator.SetBool("isMoving", false);
        }
     


        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        // rb.velocity.x;

        FlipSprite(); // flips the sprite to the correct direction 
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

    public void FlipSprite() // flips the players sprite left and right
    {
        if (!isFacingRight && horizontal < 0f || isFacingRight && horizontal > 0f)
        {

            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    IEnumerator AudioCount()
    {

        _playerAudio.Play();
        yield return new WaitForSeconds(0.1f);
        audioPlayCount--;


    }
}

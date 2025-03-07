using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Variables for movement and jump
    public float moveSpeed = 5f; 
    public float jumpForce = 10f; 
    public float jetpackForce = 8f; 
    public float gravityScale = 2f; 

    private Rigidbody2D rb; 
    private bool isGrounded = false; 
    private float moveInput; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Jetpack lift
        if (Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, Mathf.Lerp(rb.linearVelocity.y, jetpackForce, Time.deltaTime * 10f));
        }
        else
        {
            if (isGrounded)
            {
                // Regular jump when grounded
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                }
            }
            else
            {
                // Simulate gravity pull when airborne
                rb.gravityScale = gravityScale;
                rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            isGrounded = false;
        }
    }
}


// Test
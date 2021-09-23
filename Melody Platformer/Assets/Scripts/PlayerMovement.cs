using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float movementSpeed;
    private float horizontalInput;
    private Rigidbody2D playerRigidBody;
    private float dashDistance;
    private Vector2 jumpForce;
    private Animator playerAnimator;
    private bool isGrounded;
    private Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        movementSpeed = 5f;
        playerRigidBody = GetComponent<Rigidbody2D>();
        dashDistance = 1000f;
        jumpForce = new Vector2(0f, 5f);
        playerAnimator = GetComponent<Animator>();
        isGrounded = true;
        playerCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        flipSprite();
        Dash();
        Jump();
        DetectGround();
    }

    void Run()
    {
        float movementForce = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(movementForce * movementSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;

        
    }

    private void DetectGround()
    {
        if(isGrounded)
        {
            playerAnimator.SetBool("Grounded", true);
        }
    }

    private void Dash()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            transform.Translate(Vector2.right * new Vector2(Mathf.Sign(playerRigidBody.velocity.x) * dashDistance * Time.deltaTime, 1f));
        }
    }

    public void flipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidBody.velocity.x), 1f);
            playerAnimator.SetInteger("Running", 1);
        }
        else
        {
            playerAnimator.SetInteger("Running", 0);
        }
    }

    private void Jump()
    {
        isGrounded = playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRigidBody.velocity += jumpForce;
        }
    }
}

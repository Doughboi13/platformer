using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float movementSpeed = 10f;

    private enum State
    {
        Normal,
        Dashing,
    }
    private Rigidbody2D playerRB;
    private float moveDirection;
    private Vector2 dashDirection;
    private float dashSpeed;
    private float moveX;
    private float moveY;
    private float jumpForce;
    private bool isGrounded = false;
    public Transform isGroundedChecker;
    private float checkGroundRadius = 0.25f;
    public LayerMask Ground;
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private float coyoteTime = 0.1f;
    private float timeSinceGrounded;
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        state = State.Normal;
        jumpForce = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:

                moveX = Input.GetAxis("Horizontal");
                moveY = Input.GetAxis("Vertical");
                ModifyJump();
                Jump();
                CheckIfGrounded();

                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    dashDirection = new Vector2(moveX, moveY).normalized;
                    dashSpeed = 30f;
                    state = State.Dashing;
                }
                break;
            case State.Dashing:
                float dashDecelerationMultiplier = 5f;
                dashSpeed -= dashSpeed * dashDecelerationMultiplier * Time.deltaTime;
                float dashSpeedMinimum = 10f;
                if(dashSpeed < dashSpeedMinimum)
                {
                    state = State.Normal;
                }
                break;
        }

    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - timeSinceGrounded <= coyoteTime))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
        }
    }

    void CheckIfGrounded()
    {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, Ground);
        if (collider != null)
        {
            isGrounded = true;
        }
        else
        {
            if (isGrounded)
            {
                timeSinceGrounded = Time.time;
            }
            isGrounded = false;
        }
    }

    private void Run(Vector2 direction)
    {
        playerRB.velocity = new Vector2(direction.x * movementSpeed, playerRB.velocity.y);
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.Normal:
                Move();
                break;
            case State.Dashing:
                playerRB.velocity = dashDirection * dashSpeed;
                break;
        }
    }

    private void Move()
    {
        moveDirection = moveX * movementSpeed;
        playerRB.velocity = new Vector2(moveDirection, playerRB.velocity.y);
    }

    void ModifyJump()
    {
        if (playerRB.velocity.y < 0)
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (playerRB.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            playerRB.velocity += Vector2.up * Physics2D.gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
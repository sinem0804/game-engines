using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float runSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Magic Leap")]
    public bool magicLeapUnlocked = false;
    public float magicLeapForce = 8f;
    public float magicLeapSpeed = 4.5f;
    public float magicLeapGravity = 1.5f;
    public float magicLeapLandingTime = 0.08f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool isRunning;
    private bool isGrounded;
    private bool jumpRequested;
    private bool magicLeapRequested;
    private bool isMagicLeaping;

    private bool hasLeftGround;
    private float landingTimer;
    private float leapDirection;
    private float normalGravity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        normalGravity = rb.gravityScale;
    }

    void Update()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow))
            horizontalInput = -1f;

        if (Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow))
            horizontalInput = 1f;

        isRunning =
            Input.GetKey(KeyCode.LeftShift) ||
            Input.GetKey(KeyCode.RightShift);

        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        ) != null;

        if (Input.GetKeyDown(KeyCode.Space) &&
            isGrounded &&
            !isMagicLeaping &&
            !magicLeapRequested)
        {
            if (isRunning && magicLeapUnlocked)
            {
                magicLeapRequested = true;

                leapDirection = horizontalInput != 0f
                    ? horizontalInput
                    : (spriteRenderer.flipX ? -1f : 1f);
            }
            else
            {
                jumpRequested = true;
            }
        }

        if (!isMagicLeaping)
        {
            if (horizontalInput > 0)
                spriteRenderer.flipX = false;
            else if (horizontalInput < 0)
                spriteRenderer.flipX = true;
        }

        animator.SetFloat(
            "Speed",
            Mathf.Abs(horizontalInput)
        );

        animator.SetBool(
            "IsRunning",
            isRunning && horizontalInput != 0f
        );

        animator.SetBool(
            "IsGrounded",
            isGrounded && !jumpRequested &&
            !magicLeapRequested
        );

        animator.SetBool(
            "IsMagicLeaping",
            isMagicLeaping || magicLeapRequested
        );
    }

    void FixedUpdate()
    {
        if (magicLeapRequested)
        {
            isMagicLeaping = true;
            magicLeapRequested = false;

            hasLeftGround = false;
            landingTimer = -1f;

            rb.gravityScale = magicLeapGravity;

            rb.linearVelocity = new Vector2(
                leapDirection * magicLeapSpeed,
                magicLeapForce
            );
        }
        else if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(
                horizontalInput * moveSpeed,
                jumpForce
            );

            jumpRequested = false;
        }
        else if (isMagicLeaping)
        {
            if (!isGrounded)
                hasLeftGround = true;

            if (hasLeftGround && isGrounded &&
                landingTimer < 0f)
            {
                landingTimer = magicLeapLandingTime;
            }

            if (landingTimer >= 0f)
            {
                // Stop horizontal movement after landing
                rb.linearVelocity = new Vector2(
                    0f,
                    rb.linearVelocity.y
                );

                landingTimer -= Time.fixedDeltaTime;

                if (landingTimer < 0f)
                {
                    isMagicLeaping = false;
                    rb.gravityScale = normalGravity;
                }
            }
            else
            {
                // Continue moving forward in the air
                rb.linearVelocity = new Vector2(
                    leapDirection * magicLeapSpeed,
                    rb.linearVelocity.y
                );
            }
        }
        else
        {
            float speed = isRunning
                ? runSpeed
                : moveSpeed;

            rb.linearVelocity = new Vector2(
                horizontalInput * speed,
                rb.linearVelocity.y
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}

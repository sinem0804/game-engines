using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 8f;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetBool("IsWalking", horizontal != 0);

        if (horizontal != 0)
        {
            GetComponent<SpriteRenderer>().flipX = horizontal < 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}

using UnityEngine;

public class WheelbarrowController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool witchInside = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!witchInside)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponentInParent<WitchMovement>() != null)
            witchInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponentInParent<WitchMovement>() != null)
            witchInside = false;
    }
}

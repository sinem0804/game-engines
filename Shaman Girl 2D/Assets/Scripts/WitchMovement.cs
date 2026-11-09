using UnityEngine;

public class WitchMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float movement = 0f;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            movement = -1f;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            movement = 1f;

        transform.position += Vector3.right * movement * moveSpeed * Time.deltaTime;

        animator.SetBool("IsWalking", movement != 0f);

        if (movement != 0f)
            spriteRenderer.flipX = movement < 0f;
    }
}

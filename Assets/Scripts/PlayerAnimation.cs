using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float currentSpeed = playerMovement.moveX;
        bool onGround = playerMovement.isGrounded;

        anim.SetInteger("AnimState", Mathf.RoundToInt(Mathf.Abs(currentSpeed)));

        anim.SetBool("Grounded", onGround);

        anim.SetFloat("AirSpeedY", rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && onGround)
        {
            anim.SetTrigger("Jump");
        }

        if (currentSpeed > 0.1f)
        {
            sr.flipX = false; // Facing right
        }
        else if (currentSpeed < -0.1f)
        {
            sr.flipX = true; // Facing left
        }
    }
}

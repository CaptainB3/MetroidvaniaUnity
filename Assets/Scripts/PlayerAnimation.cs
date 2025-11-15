using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private PlayerControls controls;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        controls = GetComponent<PlayerControls>();

        // Animations independent of total time scale for slowing time!
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        float currentSpeed = playerMovement.moveX;
        bool onGround = playerMovement.isGrounded;

        if (controls.rollPressed && playerMovement.rollCooldownTimer <= 0f && playerMovement.canMove)
        {
            anim.SetTrigger("Roll");
        }

        if (!playerMovement.isRolling)
        {
            anim.SetInteger("AnimState", Mathf.RoundToInt(Mathf.Abs(currentSpeed)));
        }

        anim.SetBool("Grounded", onGround);

        anim.SetFloat("AirSpeedY", rb.linearVelocity.y);

        if (controls.jumpPressed && playerMovement.isGrounded)
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

        float playerSpeedMultiplier;
        // If slowing, set animation time scale
        if (playerMovement.isSlowing)
        {
            playerSpeedMultiplier = playerMovement.slowFactorPlayer;
        }
        else
        {
            playerSpeedMultiplier = 1f;
        }

        anim.speed = playerSpeedMultiplier;
    }
}
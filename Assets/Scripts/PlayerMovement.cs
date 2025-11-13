using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement: MonoBehaviour {

    private Rigidbody2D myBody;
    private SpriteRenderer sr;

    [Header("Basic Movement")]
    //Serialize Field allows to directly edit variable in inspector tab while keeping them private from rest of program
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 11f;

    public float moveX;
    public bool isGrounded = true;
    public bool canMove = true;

    [Header("Double Jump")]
    public bool canDoubleJump = false;
    [SerializeField]
    private int extraJumps = 1;
    private int jumpsLeft;

    [Header("Time Slow")]
    public bool canTimeSlow = false;
    public float slowDuration = 2f;
    public float slowFactor = 0.5f;
    public float slowFactorPlayer = 1f;
    public bool isSlowing = false;
    private float slowTimer = 0f;
    private float normalGravity;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //empty
        jumpsLeft = extraJumps;
        normalGravity = myBody.gravityScale;
    }

    void Update()
    {
        if (canMove) // canMove variable in case we need to stop the player like if you die
        {
            PlayerMoveKeyboard();
        }
        PlayerJump();

        if (canTimeSlow && !isSlowing && Input.GetButtonDown("Fire3"))
        {
            StartTimeSlow();
        }

        if (isSlowing)
        {
            slowTimer -= Time.unscaledDeltaTime;
            if (slowTimer <= 0f)
                EndTimeSlow();
        }

        myBody.gravityScale = isSlowing ? normalGravity * slowFactorPlayer : normalGravity;
    }

    //FixedUpdate is useful when performing physics calculations, called every set interval rather than every individual frame
    //ALL GOOD GAME DEVELOPERS TIE THEIR PHYSICS TO THE FRAME RATE
    //private void FixedUpdate()
    //{
    //    PlayerJump();
    //}

    void PlayerMoveKeyboard()
    {
        //GetAxis acts as a sort of slide, acts as a mild accelerator
        moveX = Input.GetAxisRaw("Horizontal");

        float horizontalVelocity = moveForce * moveX;
        if (isSlowing)
            horizontalVelocity *= slowFactorPlayer;
        myBody.linearVelocity = new Vector2(horizontalVelocity, myBody.linearVelocity.y);
    }

    void PlayerJump()
    {
        //Will default to whatever platform binds 'jump' (PC is space, controller may be A/X)
        //GetButtonUp will return true when button is released
        //GetButton will return true when pressed, held, and released
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
                isGrounded = false;
            }
            else if (jumpsLeft >  0)
            {
                Jump();
                jumpsLeft--;
            }
        }
    }

    void Jump()
    {
        // Reset vertical velocity
        myBody.linearVelocity = new Vector2(myBody.linearVelocity.x, 0f);

        float currentJumpForce = isSlowing ? jumpForce * slowFactorPlayer : jumpForce;
        myBody.AddForce(Vector2.up * currentJumpForce, ForceMode2D.Impulse);
    }

    void StartTimeSlow()
    {
        isSlowing = true;
        slowTimer = slowDuration;

        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Physics consistency I think
    }
    void EndTimeSlow()
    {
        isSlowing = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        myBody.gravityScale = normalGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpsLeft = canDoubleJump ? extraJumps : 0; // Set to extraJumps if true, 0 if false
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
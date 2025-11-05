using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerMovement: MonoBehaviour {

    //Serialize Field allows to directly edit variable in inspector tab while keeping them private from rest of program
    [SerializeField]
    private float moveForce = 10f;
    [SerializeField]
    private float jumpForce = 11f;

    private float moveX;

    private Rigidbody2D myBody;

    private SpriteRenderer sr;

    private bool isGrounded = true;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        //empty
    }

    void Update()
    {
        PlayerMoveKeyboard();

        PlayerJump();
    }

    //FixedUpdate is useful when performing physics calculations, called every set interval rather than every individual frame
    //ALL GOOD GAME DEVELOPERS TIE THEIR PHYSICS TO THE FRAME RATE
    private void FixedUpdate()
    {
        PlayerJump();
    }

    void PlayerMoveKeyboard()
    {
        //GetAxis acts as a sort of slide, acts as a mild accelerator
        moveX = Input.GetAxisRaw("Horizontal");

        //Time.deltatime is time between each frame, prevents the player from moving way too fast
        //Smooths movement when using transform
        transform.position += new Vector3(moveX, 0f, 0f) * Time.deltaTime * moveForce;
    }

    void PlayerJump()
    {
        //Will default to whatever platform binds 'jump' (PC is space, controller may be A/X)
        //GetButtonUp will return true when button is released
        //GetButton will return true when pressed, held, and released
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            myBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

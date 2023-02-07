using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    private Player player;

    [SerializeField] private PlayerControls playerControls;
    private InputAction moveIA;
    private InputAction jumpIA;
    private InputAction sprintIA;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private Animator animator;

    private Vector3 moveDirection;

    private bool holdSprint;

    [Header("Movement Speed")]

    private float maxSpeed;
    private float acceleration;
    [SerializeField] private float deacceleration;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float walkAcceleration;

    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintAcceleration;


    //Jump
    [Header("Jump")]

    [SerializeField] private float jumpSpeed;
    private int airJumpCountMax;
    private float minAirTime;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float coyoteTime;



    private int jumpCount;
    private bool isGrounded;
    private bool onMinAirTime = false;
    private bool onJumpBuffer = false;
    private bool onCoyoteTime = false;

    void Awake()
    {
        //player = GetComponent<Player>();
        //Debug.Log(player.playerControls);
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        playerControls.PlayerMap.Enable();
        moveIA = playerControls.PlayerMap.HorizontalMovement;
        //moveIA.Enable();

        jumpIA = playerControls.PlayerMap.Jump;
        //jumpIA.Enable();
        jumpIA.performed += JumpInput;


        sprintIA = playerControls.PlayerMap.Sprint;
        //sprintIA.Enable();
    }
    void OnDisable()
    {
        //moveIA.Disable();
        //jumpIA.Disable();
        //sprintIA.Disable();

        playerControls.PlayerMap.Disable();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        //if (moveIA.ReadValue<float>() != 0) { Debug.Log("Pressed Walk"); }
        moveDirection = Vector2.right * moveIA.ReadValue<float>();

        if (onJumpBuffer && isGrounded)
        {
            Jump();
        }

        if (onMinAirTime && jumpIA.ReadValue<float>() <= 0)
        {
            //JumpCancel();
            onMinAirTime = false;
        }

        //if (moveDirection.x > 0) { sprite.flipX = false; }
        //else if (moveDirection.x < 0) { sprite.flipX = true; }

        //animator.SetBool("isWalking", moveDirection.x != 0);
        //animator.SetBool("isGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        holdSprint = sprintIA.IsPressed();

        if (holdSprint)
        {
            maxSpeed = sprintSpeed;
            acceleration = sprintAcceleration;
        }
        else
        {
            maxSpeed = walkSpeed;
            acceleration = walkAcceleration;
        }


        if (isGrounded)
        {
            if (moveDirection.x != 0)
            {
                rb.AddForce(moveDirection * acceleration, ForceMode2D.Force);
                rb.velocity = new Vector2(Mathf.Min(maxSpeed, Mathf.Abs(rb.velocity.x)) * moveIA.ReadValue<float>(), rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(1 / deacceleration * rb.velocity.x, rb.velocity.y);
            }
        }


    }

    


    void JumpInput(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Pressed");

        if (isGrounded || jumpCount < airJumpCountMax)
        {
            Jump();
        }
        else
        {
            StartCoroutine(JumpBuffer());
        }
    }

    IEnumerator JumpBuffer()
    {
        onJumpBuffer = true;
        yield return new WaitForSeconds(jumpBufferTime);
        onJumpBuffer = false;
    }

    IEnumerator CoyeteTime()
    {
        onCoyoteTime = true;
        yield return new WaitForSeconds(coyoteTime);
        onCoyoteTime = false;
        if (jumpCount == 0)
        {
            jumpCount++;

        }
    }

    IEnumerator CountMinAirTime()
    {
        onMinAirTime = false;
        yield return new WaitForSeconds(minAirTime);
        onMinAirTime = true;
    }

    void Jump()
    {
        onJumpBuffer = false;
        onCoyoteTime = false;
        StartCoroutine(CountMinAirTime());
        if (!isGrounded || !onCoyoteTime)
        {
            jumpCount++;
        }
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //animator.SetTrigger("Jump");
    }

    void JumpCancel()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(0, rb.velocity.y));
        //animator.SetTrigger("Fall");
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor") //HitFloor
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
            //if (jumpCount == 0)
            //{
            StartCoroutine(CoyeteTime());

            //}
        }
    }
}

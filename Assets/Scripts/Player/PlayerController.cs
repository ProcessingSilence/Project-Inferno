using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField]private LayerMask platformLayerMask;

// Jump
    public float jumpVel;

    // Gravity multiplier depending if input is held or not.
    public float inputGravityWeight = 2.5f;
    public float noInputGravityWeight = 2f;

    // Gets y position of player when they are no longer grounded.
        // If the player's y-position >= yPosLimit, they will start falling
    private float yPosLimit;

    // Flag that determines what happens midair when not grounded.
        // 0- Currently falling
        // Have jumped: 1-  Going up, did not touch height limit. 2- Touched height limit, switch to 0.
    private int iJumpedFlag;

    // When pressing jump input, waits [input time] before seeing if player lands on ground, if the player lands on
    // the ground within that time period, they will automatically jump.
    private float jumpPressedPeriodCurrent;
    private float jumpPressedPeriodTime = 0.1f;

    public bool alreadyJumped;

    public float groundedTime;
    public float rememberGroundedTime;

    private AudioClip jumpSound;

// Movement
    public float moveSpeed = 13;
    [HideInInspector]public float maxSpeed;
    private Vector3 surfaceNormal;

    private float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;

    public int horizontal, vertical;
    public float prevHorizontal, prevVertical;

    [SerializeField]private Vector3 currentVelocity;
    // Multiplies horizontal velocity to determine direction in FixedUpdate().
    //[HideInInspector]
    //public int inputDirection;
    public Vector3 movementVelocity;

//Pushed Force
    public Vector3 currentForceDirection;

// Components
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private AudioSource aS;
    public Transform cameraPos;
    private CharacterController characterController;
    private PlayerState playerStateScript;

    public GameObject model;

// Animator Stuff
    [SerializeField] private Animator animator;

    public bool isGroundedThing;
    public GameObject colliderThing;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        characterController = GetComponent<CharacterController>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        aS = GetComponent<AudioSource>();
        jumpSound = Resources.Load("Audio/jump") as AudioClip;
        playerStateScript = GetComponent<PlayerState>();
        maxSpeed = moveSpeed;
        moveSpeed = 0;
        //Physics.IgnoreLayerCollision(13,13, true);
    }


    private void FixedUpdate()
    {
        Vector3 angle = Vector3.zero;
        RaycastHit hit;
        if (Physics.Raycast(boxCollider.bounds.center, Vector3.down, out hit, (boxCollider.size.y/2) + 5, platformLayerMask))
        {
            angle = hit.normal;
            // Debug.Log("angle: " + angle);
            if (angle.y < 1 && alreadyJumped == false)
            {
                //transform.position = hit.point + (-Vector3.down * (capsuleCollider.size.y / 2));
                //rb.AddForce(angle * -10000);
            }

            //if (hit.transform.eulerAngles. > 0.1f)
        }

        Gravity();
        //SnapToFloor();
        // Debug.Log("ISGROUNDED: " + IsGrounded());


        //HorizontalMovement();
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        var tempSpeed = moveSpeed;
        Vector3 direction = Vector3.zero;

        if (horizontal != 0 ||vertical != 0)
        {
            tempSpeed += (maxSpeed*5f) * Time.deltaTime;
            prevHorizontal = horizontal;
            prevVertical = vertical;
            direction = new Vector3(horizontal, 0f, vertical).normalized;
        }

        else if (horizontal == 0 && vertical == 0 && IsGrounded())
        {
            tempSpeed -= (maxSpeed*5f) * Time.deltaTime;
            direction = new Vector3(prevHorizontal, 0f, prevVertical).normalized;

            movementVelocity -= new Vector3(movementVelocity.x/5, 0, movementVelocity.z/5);
        }


        // Regulate speed amount.
        if (tempSpeed > maxSpeed)
        {
            tempSpeed = maxSpeed;
        }
        if (tempSpeed < 0)
        {
            tempSpeed = 0;
        }

        moveSpeed = tempSpeed;



        Vector3 finalDirection = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraPos.eulerAngles.y;

            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            /*
            if (playerStateScript.currentState != PlayerState.PlayerStates.Shooting)
            {
                //model.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            */

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            finalDirection = new Vector3(moveDir.x, 0, moveDir.z);
            //finalDirection = Quaternion.FromToRotation(moveDir, Quaternion.LookRotation(moveDir, surfaceNormal) * Vector3.forward) * Vector3.forward;
        }

        //currentVelocity += finalDirection * moveSpeed * Time.deltaTime;

        //movementVelocity += (new Vector3(finalDirection.x, 0, finalDirection.z) * maxSpeed ) * Time.deltaTime;
        var multForce = 10;
        if (!IsGrounded())
        {
            multForce = 2;
        }

        movementVelocity += new Vector3(finalDirection.x * (moveSpeed * multForce) * Time.deltaTime, 0, finalDirection.z * (moveSpeed * multForce) * Time.deltaTime);
        movementVelocity = Vector3.ClampMagnitude(movementVelocity, maxSpeed);

        Vector3 finalMovementVelocity = Vector3.ProjectOnPlane(movementVelocity, surfaceNormal);

        rb.velocity = new Vector3(finalMovementVelocity.x, rb.velocity.y, finalMovementVelocity.z);


        //rb.velocity = new Vector3(finalDirection.x * moveSpeed, rb.velocity.y, finalDirection.z * moveSpeed);



        //SpriteRender();
        //Debug.Log("Is grounded: " + IsGrounded());

        ExtDebug.DrawBoxCastBox(boxCollider.bounds.center, boxCollider.bounds.size*0.5f -(new Vector3(0,0.1f,0)),  Quaternion.identity, Vector3.down,  .6f, Color.red, Color.yellow);
        //return Physics.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size*1f, Vector3.down, Quaternion.identity,  .5f, platformLayerMask);
    }

    private void SnapToFloor()
    {
        RaycastHit hit;

        if (Physics.Raycast(boxCollider.bounds.center, Vector3.down, out hit, boxCollider.bounds.size.y * 0.5f + 0.5f))
        {
            Debug.Log("HIT: " + hit.point + " OBJECT: " + hit.collider.gameObject.name, hit.collider.gameObject);
            Debug.DrawRay(hit.point, hit.normal);
            transform.position = hit.point + Vector3.up * boxCollider.size.y * 0.5f;
            surfaceNormal = hit.normal;
        }
    }

    void Update()
    {
        JumpingInput();
        CoyoteTime();
    }

    private void LateUpdate()
    {
        var movement = new Vector3(horizontal, 0f, vertical);
        float velocityZ = Vector3.Dot(movement.normalized, transform.forward);
        float velocityX = Vector3.Dot(movement.normalized, transform.right);

        animator.SetFloat("VelocityZ", vertical, 0.1f, Time.deltaTime);
        animator.SetFloat("VelocityX", horizontal, 0.1f, Time.deltaTime);

        animator.SetBool("jumpBool", !IsGrounded());

        // For debugging purposes.
        isGroundedThing = IsGrounded();
    }



    void Gravity()
    {
        if (!IsGrounded())
        {
            /*
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (inputGravityWeight - 1) * Time.deltaTime;
            }

            else if (rb.velocity.y > 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (noInputGravityWeight - 1 ) * Time.deltaTime;
            }*/
            if (rb.velocity.y > -20)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (noInputGravityWeight - 1 ) * Time.deltaTime;
            }
        }
    }

    void Jump()
    {
        //rb.velocity = new Vector3(rb.velocity.x, 0);
        rb.velocity = new Vector3(rb.velocity.x, jumpVel, rb.velocity.z);
        if (alreadyJumped == false && aS.isPlaying == false)
        {
            alreadyJumped = true;
            aS.clip = jumpSound;
            aS.Play();
        }
    }
    /*
    void HigherJump()
    {
        //rb.velocity = new Vector3(rb.velocity.x, 0);
        rb.velocity = Vector3.up * jumpVel * 1.75f;
    }
    */

    void JumpingInput()
    {
        jumpPressedPeriodCurrent -= Time.deltaTime;

        // Reset timer on jump.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressedPeriodCurrent = jumpPressedPeriodTime;
        }

        if (jumpPressedPeriodCurrent > 0)
        {
            if (rb.velocity.y <= 0.01f && IsGrounded())
            {
                Jump();
            }
            else if (rememberGroundedTime - 0.05f < groundedTime)
            {
                Jump();
            }
        }

        if (IsGrounded())
        {
            alreadyJumped = false;
        }

        /*
        if (!IsGrounded())
        {
            // Jump sprite
            if (chosenSprite != 3)
            {
                chosenSprite = 2;
            }
        }
        */
    }

    void CoyoteTime()
    {
        groundedTime -= Time.deltaTime;
        if (IsGrounded())
        {
            rememberGroundedTime = groundedTime;
        }
    }

    /*
    private void HorizontalMovement()
    {
        if (Input.GetKey(KeyCode.D))
        {
            inputDirection = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            inputDirection = -1;
        }
        else
        {
            inputDirection = 0;
        }
    }
    */

    /*
    private void SpriteRender()
    {
        // Left
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spriteRenderer.flipX = true;
        }
        // Right
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (chosenSprite != 3)
        {
            if (IsGrounded() )
            {
                // Idle
                if (Input.GetAxisRaw("Horizontal") < 0.1f && Input.GetAxisRaw("Horizontal") > -0.1f)
                {
                    chosenSprite = 1;
                }

                // Moving
                else
                {
                    chosenSprite = 0;
                }
            }
            // Jumping
            else if (!IsGrounded())
            {
                chosenSprite = 2;
            }
        }
        spriteRenderer.sprite = spriteStates[chosenSprite];
    }
    */

    public bool IsGrounded()
    {
        return Physics.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size*0.5f -(new Vector3(0,0.1f,0)), Vector3.down, Quaternion.identity,  .6f);
    }


    /*
    // Prevents player from auto-jumping after touching the one-way platforms while velocity is upwards.
    // Prevents player from auto-jumping after touching the one-way platforms while velocity is upwards.
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("OneWayPlatform") && rb.velocity.y > 0)
        {
            jumpPressedPeriodCurrent = 0;
        }
        //Debug.Log(other.gameObject);
    }
    */
}

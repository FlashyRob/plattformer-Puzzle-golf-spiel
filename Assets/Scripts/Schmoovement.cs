using UnityEngine;

public class Schmoovement : MonoBehaviour

{
    private Rigidbody2D rb2d;
    private Collider2D capsule2d;
    private Animator animator;
    public bool Grounded = false;
    private float GroundedTimer = 0;
    public bool secondJump = false;
    public bool Walled = false;
    public float horizontalPush = 0;
    private bool isFacingRight;
    float collidex = 0;
    float myx = 0;
    float controldamper = 1;
    private Vector2 velocityDebug;
    public float moveSpeed = 5;
    public float platformJump;
    bool CameFromAbove = false;
    private float wallSlideCooldown = 0f;
    private float WalljumpWhileSliding = 0;
    private bool StayingOnGround = false;
    private Push push;
    [HideInInspector]
    public Vector2 PushBoost;
    public bool gettingBoosted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFacingRight = true;
        rb2d = GetComponent<Rigidbody2D>();
        capsule2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        push = FindAnyObjectByType<Push>();
    }

    private float inputHorizontalAxis;
    private int inputKeyDownSpace;
    private bool inputKeySpace;
    private bool inputKeyD;
    private bool inputKeyA;
    private bool processInput = false;

    private Vector2 playerVel;
    void Update() // Update is called once per frame
    {
        inputHorizontalAxis = Input.GetAxis("Horizontal"); // key a pressed = -1 ; key d pressed = 1 ; no key pressed = 0
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputKeyDownSpace++;
        }
        inputKeySpace = Input.GetKey(KeyCode.Space);
        inputKeyD = Input.GetKey(KeyCode.D);
        inputKeyA = Input.GetKey(KeyCode.A);
        playerVel = rb2d.linearVelocity;

        processInput = true;

    }

    float verticalVelocity = 0;
    float horizontalVelocity;
    float jumpVelocity;
    void FixedUpdate()
    {
        // Fixed Update is called multiple times while update is called once.
        // we are perform the movmement in fixed update to allow accurate moving platform tracking
        wallSlideCooldown -= Time.fixedDeltaTime;

        if (processInput)
        {
            if (GroundedTimer > 0)
            {
                GroundedTimer = GroundedTimer - 1;
            }

            if (GroundedTimer > 0)
            {
                Grounded = true;
            }
            else
            {
                Grounded = false;
            }

            if (inputKeySpace)
            {
                jumpVelocity = 0.08f;

            }
            else
            {
                jumpVelocity = 0;
            }

            if (inputKeyDownSpace >= 1)
            {
                if (Grounded && !Walled)
                {
                    jumpVelocity = 9 + platformJump;
                    playerVel.y = 0;
                }

                if (Walled)
                {
                    jumpVelocity = 12 * WalljumpWhileSliding;
                    wallSlideCooldown = 0.05f; // blocks slide for 0.2 seconds
                    Walled = false;

                    if (collidex > myx)
                    {
                        // The wall is to the left
                        horizontalPush = -4.5f;
                    }
                    else
                    {
                        // The wall is to the right
                        horizontalPush = 4.5f;
                    }
                }
            }
            else
            {
                if (Walled && !Grounded && wallSlideCooldown <= 0)
                {
                    if (collidex > myx)    // The wall is to the left
                    {
                        horizontalPush = 0.8f;
                        jumpVelocity = -6;
                        WalljumpWhileSliding = 1.5f;

                        if (Input.GetKey(KeyCode.A))
                        {
                            horizontalPush = 0;
                            controldamper = 2;
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            Debug.Log("Execute");
                            controldamper = 0.4f;
                            jumpVelocity = -3;
                            WalljumpWhileSliding = 1.25f;
                        }
                    }
                    else    // The wall is to the right
                    {
                        horizontalPush = -0.8f;
                        jumpVelocity = -6;
                        WalljumpWhileSliding = 1.5f;

                        if (Input.GetKey(KeyCode.D))
                        {
                            horizontalPush = 0;
                            controldamper = 2;
                        }

                        if (Input.GetKey(KeyCode.A))
                        {
                            Debug.Log("Execute");
                            controldamper = 0.4f;
                            jumpVelocity = -3;
                            WalljumpWhileSliding = 1.25f;
                        }
                    }
                }
                else
                {
                    controldamper = 1;
                    WalljumpWhileSliding = 1;
                }
            }
        }
            
        verticalVelocity = jumpVelocity + playerVel.y;

        if (inputKeyDownSpace >= 1 && secondJump && !Walled && !Grounded && verticalVelocity > 6)
        {
            jumpVelocity = 5;
            secondJump = false;
            animator.SetBool("isDJ", true);
        }

        verticalVelocity = jumpVelocity + playerVel.y;

        if (inputKeyDownSpace >= 1 && secondJump && !Walled && !Grounded && verticalVelocity < 6)
          
        {
            verticalVelocity = 9;
            secondJump = false;
            animator.SetBool("isDJ", true);
        }

        horizontalPush = horizontalPush * 0.95f;

        if (horizontalPush < 0.5 && horizontalPush > -0.5)
        {
            horizontalPush = 0;
        }

        if (Walled && !Grounded && wallSlideCooldown <= 0)
        {
            if ((inputKeyA || inputKeyD) && verticalVelocity < -3)
            {
                verticalVelocity = -3;
            }
            else if (verticalVelocity < -3)
            {
                verticalVelocity = -3;
            }
        }

            verticalVelocity += PushBoost.y;
            horizontalVelocity = (inputHorizontalAxis * controldamper + horizontalPush) * moveSpeed;

            if(gettingBoosted)
            {
                horizontalVelocity = PushBoost.x;
            }

            rb2d.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
            PushBoost = Vector2.zero;
            gettingBoosted = false;

            if (inputKeyDownSpace >= 1)
            {
                inputKeyDownSpace--;
            }
            processInput = false;

        // set animation parameters
        animator.SetFloat("Speed", rb2d.linearVelocity.x);
        animator.SetBool("isWalled", Walled);
        animator.SetBool("isGrounded", Grounded);
        if (Grounded)
        {
            animator.SetFloat("JumpSpeed", 0);
        }
        else
        {
            animator.SetFloat("JumpSpeed", rb2d.linearVelocity.y);
        }

            if (Grounded || Walled)
            {
                animator.SetBool("isDJ", false);
            }

            if (rb2d.linearVelocity.y < 0)
            {
                animator.SetBool("isFalling", true);
            }
            else
            {
                animator.SetBool("isFalling", false);
            }

        if (!isFacingRight && horizontalVelocity > 0)
        {
            Flip();
        }
        else if (isFacingRight && horizontalVelocity < 0)
        {
            Flip();
        }
        

        // move with platforms
        if (currentPlatform && !inputKeySpace)
        {
            Vector2 platformVel = currentPlatform.GetVelocity();
            Debug.Log("Got velocity " + platformVel + " from platform " + currentPlatform.name);

            Vector2 playerAndPlatform = new Vector2();
            // Always add horizontal platform motion when we stand on the platform
            playerAndPlatform.x = horizontalVelocity + platformVel.x;

            // The vertical velocity builds up over mutliple frames because we dont overwrite it from scratch like the horizontalVelocity.
            // So we cannot simply add it like the platform x motion
            // instead, I snap the player velocity to the plaform if the players velcoity is already close to the platform
            // this setup allows you to jump though the platform without snapping to the platform and loosing your velocity.
            if (Mathf.Abs(playerVel.y - platformVel.y) <= Mathf.Abs(platformVel.y * 0.05f))
            {
                Debug.Log("transfer y force to player "+ platformVel.y);
                playerAndPlatform.y = platformVel.y;
            }
            Debug.Log("playerAndPlatform " + playerAndPlatform);
            rb2d.linearVelocity = playerAndPlatform;
            Debug.Log("rb2d.linearVelocity " + rb2d.linearVelocity);
        }

        velocityDebug = rb2d.linearVelocity; 
    }


    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            ContactPoint2D contact = coll.contacts[0];
            Vector2 normal = contact.normal; // has length of 1
                                             // we check the collision normal to see which direction the ground hit us from
            horizontalPush = 0;
        }
    }
    
    void OnCollisionStay2D(Collision2D coll)
    {
        Debug.Log("Collision");
        if (coll.collider.CompareTag("MovingPlatform"))
        {
            // we use collider bounds of platform and player to see if we are on top of the platform
            // normal vector was to lenient
            // Get bounds
            Collider2D playerCol = GetComponent<Collider2D>();
            Collider2D platformCol = coll.collider;

            float playerBottom = playerCol.bounds.min.y;
            float platformTop = platformCol.bounds.max.y;

            // Optional buffer to account for small penetration
            float tolerance = 0.05f;

            if (playerBottom >= platformTop - tolerance)
            {
                GroundedTimer = 8;
                Walled = false;
                secondJump = false;
                currentPlatform = coll.gameObject.GetComponent<ForceTransfer>();
            }

        }
        if (coll.gameObject.tag == "Ground")
        {
            ContactPoint2D contact = coll.contacts[0];
            Vector2 normal = contact.normal; // has length of 1
                                             // we check the collision normal to see which direction the ground hit us from
            if (normal.y > 0.5f)
            {
                // the normal vector mostly points up. The ground has hit us from below.
                GroundedTimer = 8;
                Walled = false;
                secondJump = false;
            }
            else if (Mathf.Abs(normal.x) > 0.5f)
            {
                // The vector mostly points in x or -x direction. So we've hit a wall
                Walled = true;
                collidex = contact.point.x; // remeber the contact point for walljump
                myx = transform.position.x;
            }
        }

        if (coll.gameObject.tag == "Box")
        {
            ContactPoint2D contact = coll.contacts[0];
            Vector2 normal = contact.normal; // has length of 1
            // we check the collision normal to see which direction the ground hit us from

            if (normal.y > 0.5f)
            {
                // the normal vector mostly points up. The ground has hit us from below.
                GroundedTimer = 8;
                Walled = false;
                secondJump = false;
            }
        }
    }


    private ForceTransfer currentPlatform;
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
            secondJump = true;
        }
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Box")
        {
            if (Grounded)
            {
                secondJump = true; 
            }

            if (Walled)
            {
                Walled = false;
                if(wallSlideCooldown == 0)
                {
                    horizontalPush = 0;
                }
            }
        }
    }
}

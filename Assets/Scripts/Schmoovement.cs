using UnityEngine;

public class Schmoovement : MonoBehaviour

{
    private Rigidbody2D rb2d;
    private Collider2D capsule2d;
    private Animator animator;
    public bool Grounded = false;
    public bool secondJump = false;
    public bool Walled = false;
    public float horizontalPush = 0;
    private bool isFacingRight;
    float collidex = 0;
    float myx = 0;
    float controldamper = 1;
    public Vector2 velocity;
    public float moveSpeed;
    public float platformJump;
    bool Slide = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isFacingRight = true;
        rb2d = GetComponent<Rigidbody2D>();
        capsule2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    private float inputHorizontalAxis;
    private bool inputKeyDownSpace;
    private bool inputKeySpace;
    private bool inputKeyD;
    private bool inputKeyA;

    private bool processInput = false;

    private Vector2 playerVel;
    void Update()
    {

        inputHorizontalAxis = Input.GetAxis("Horizontal"); // key a pressed = -1 ; key d pressed = 1 ; no key pressed = 0
        inputKeyDownSpace = Input.GetKeyDown(KeyCode.Space);
        inputKeySpace = Input.GetKey(KeyCode.Space);
        inputKeyD = Input.GetKey(KeyCode.D);
        inputKeyA = Input.GetKey(KeyCode.A);

        Debug.Log("At start of FixedUpdate player velocity is " + rb2d.linearVelocity);

        animator.SetBool("isWalled", Walled);
        animator.SetBool("isGrounded", Grounded);

        playerVel = rb2d.linearVelocity;

    }

    void FixedUpdate()
    {

        float jumpVelocity;
        float verticalVelocity = 0;
        float horizontalVelocity;


        if (inputKeySpace)
        {
            jumpVelocity = 0.012f;
        }
        else
        {
            jumpVelocity = 0;
        }


        if (inputKeyDownSpace)
        {
            if (Grounded && !Walled)
            {
                jumpVelocity = 9 + platformJump;
            }

            if (Walled)
            {
                jumpVelocity = 10;
                controldamper = 0.3f;
                Slide = false;
                if (collidex > myx)
                {
                    // The wall is to the left

                    horizontalPush = -3;
                }
                else
                {
                    // The wall is to the right

                    horizontalPush = 3;
                }
            }
        }
        else
        {
            if (Slide)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    jumpVelocity = -2;
                }
                else
                {
                    jumpVelocity = -0.5f;
                }

            }
            else
            {
                jumpVelocity = 0;
            }
        }

        verticalVelocity = jumpVelocity + playerVel.y;


        if (inputKeyDownSpace && secondJump && !Walled && verticalVelocity > 6)
        {
            jumpVelocity = 4;
            secondJump = false;
        }

        verticalVelocity = jumpVelocity + playerVel.y;

        if (inputKeyDownSpace && secondJump && !Walled && verticalVelocity < 6)
        {
            verticalVelocity = 8;
            secondJump = false;
        }

        horizontalPush = horizontalPush * 0.95f;

        if (horizontalPush < 0.5 && horizontalPush > -0.5)
        {
            horizontalPush = 0;
        }

        if (Slide)
        {
            if (inputKeyA || inputKeyD && verticalVelocity < -3)
            {
                verticalVelocity = -3;
            }
            else if (verticalVelocity < -1.5f)
            {
                verticalVelocity = -1.5f;
            }
        }

        horizontalVelocity = inputHorizontalAxis * controldamper + horizontalPush;

        rb2d.linearVelocity = new Vector2(horizontalVelocity * moveSpeed, verticalVelocity);


        // move with platforms
        if (currentPlatform && !inputKeySpace)
        {
            Vector2 platformVel = currentPlatform.velocity;

            Vector2 playerAndPlatform = rb2d.linearVelocity;
            // Always add horizontal platform motion when we stand on the platform
            playerAndPlatform.x += platformVel.x;



            // The vertical velocity builds up over mutliple frames because dont overrite it from scratch every frame.
            // So we cannot simply add it like the platform x motion
            // instead, I check if the platform is moving faster in the direction than the player and snap the player to the platform velocity in this case
            // additionally I snap the player velocity to the plaform if the players velcoity is already close to the platform
            // this setup allows you to jump though the platform without loosing your velocity.
            float playerY = playerVel.y;
            float platformY = platformVel.y;
            bool similarVerticalSpeed = Mathf.Abs(playerY - platformY) <= Mathf.Abs(platformY * 0.05f);
            bool platformIsFaster = false; // Mathf.Abs(platformY) > Mathf.Abs(playerY) && Mathf.Sign(platformY) == Mathf.Sign(playerY);

            if (similarVerticalSpeed || platformIsFaster)
            {
                playerAndPlatform.y = platformY;
            }

            rb2d.linearVelocity = playerAndPlatform;

            Debug.Log("Player velocity platform added: " + rb2d.linearVelocity);
        }

        velocity = rb2d.linearVelocity;

        animator.SetFloat("Speed", rb2d.linearVelocity.x);
        if (Grounded)
        {
            animator.SetFloat("JumpSpeed", 0);
        }
        else
        {
            animator.SetFloat("JumpSpeed", rb2d.linearVelocity.y);
        }


        Camera.main.transform.position = transform.position + new Vector3(0, 0, -100);


        if (!isFacingRight && horizontalVelocity > 0)
        {
            Flip();
        }
        else if (isFacingRight && horizontalVelocity < 0)
        {
            Flip();
        }

        playerVel = rb2d.linearVelocity;
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

            if (Mathf.Abs(normal.x) > 0.5f)
            {
                // The vector mostly points in x or -x direction. So we've hit a wall
                Slide = true;
            }
        }

        if (coll.gameObject.tag == "MovingPlatform")
        {

        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {

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
                Grounded = true;
                Walled = false;
                secondJump = false;
                Slide = false;
                currentPlatform = coll.gameObject.GetComponent<PlatformMovement>();
            }

        }
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Box")
        {
            ContactPoint2D contact = coll.contacts[0];
            Vector2 normal = contact.normal; // has length of 1
            // we check the collision normal to see which direction the ground hit us from

            controldamper = 1;

            if (normal.y > 0.5f)
            {
                // the normal vector mostly points up. The ground has hit us from below.
                Grounded = true;
                Walled = false;
                secondJump = false;
                Slide = false;
                Debug.Log("Vector" + !Slide);
            }
            else if (Mathf.Abs(normal.x) > 0.5f)
            {
                // The vector mostly points in x or -x direction. So we've hit a wall
                Walled = true;
                collidex = contact.point.x; // remeber the contact point for walljump
                myx = transform.position.x;
            }
        }
    }


    private PlatformMovement currentPlatform;
    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("MovingPlatform"))
        {
            //rb2d.gravityScale = 1.8f;
            currentPlatform = null;
            Grounded = false;
            secondJump = true;
            moveSpeed = 5;
            platformJump = 0;
        }
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Box")
        {
            if (Grounded)
            {
                secondJump = true;
                Grounded = false; ;
            }

            if (Walled)
            {
                Walled = false;
            }

            if (Slide)
            {
                Slide = false;
                Debug.Log("collisonExit" + !Slide);
            }
        }
    }
}

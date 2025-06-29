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
    public Vector2 velocityDebug;
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

        processInput = true;

        //Debug.Log("At start Update player velocity is " + rb2d.linearVelocity);



        playerVel = rb2d.linearVelocity;

    }

    float verticalVelocity = 0;
    float horizontalVelocity;
    void FixedUpdate()
    {
        // Fixed Update is called multiple times while update is called once.
        // we are perform the movmement in fixed update to allow accurate moving platform tracking
        float jumpVelocity;
        //Debug.Log("At start Fixed Update player velocity is " + rb2d.linearVelocity);

        if (processInput)
        {
            //Debug.Log("Processing Input");
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

            horizontalVelocity = (inputHorizontalAxis * controldamper + horizontalPush) * moveSpeed;
            rb2d.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
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

            if (!isFacingRight && horizontalVelocity > 0)
            {
                Flip();
            }
            else if (isFacingRight && horizontalVelocity < 0)
            {
                Flip();
            }

            //Debug.Log("Input gives us " + rb2d.linearVelocity);
        }

        // move with platforms
        if (currentPlatform && !inputKeySpace)
        {
            Vector2 platformVel = currentPlatform.velocity;

            Vector2 playerAndPlatform = new Vector2();
            // Always add horizontal platform motion when we stand on the platform
            playerAndPlatform.x = horizontalVelocity + platformVel.x;

            // The vertical velocity builds up over mutliple frames because we dont overrite it from scratch like the horizontalVelocity.
            // So we cannot simply add it like the platform x motion
            // instead, I snap the player velocity to the plaform if the players velcoity is already close to the platform
            // this setup allows you to jump though the platform without snapping to the platform and loosing your velocity.
            if (Mathf.Abs(playerVel.y - platformVel.y) <= Mathf.Abs(platformVel.y * 0.05f))
            {
                playerAndPlatform.y = platformVel.y;
            }
            rb2d.linearVelocity = playerAndPlatform;
            //Debug.Log("Player velocity platform added: " + rb2d.linearVelocity);
        }

        velocityDebug = rb2d.linearVelocity; // debug display velocity in inspector window
        //Debug.Log("At player velocity Fixed Update calcualted " + rb2d.linearVelocity);

        Camera.main.transform.position = transform.position + new Vector3(0, 0, -100);

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
                //Debug.Log("Vector" + !Slide);
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
                //Debug.Log("collisonExit" + !Slide);
            }
        }
    }
}

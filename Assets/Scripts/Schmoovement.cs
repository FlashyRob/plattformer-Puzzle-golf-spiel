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
    void Update()
    {


        float horizontal = Input.GetAxis("Horizontal"); // key a pressed = -1 ; key d pressed = 1 ; no key pressed = 0
        float jumpVelocity;
        float verticalVelocity = 0;
        float horizontalVelocity;
        
        animator.SetFloat("Speed", rb2d.linearVelocity.x);
        animator.SetFloat("JumpSpeed", rb2d.linearVelocity.y);
        animator.SetBool("isWalled", Walled);
        animator.SetBool("isGrounded", Grounded);

        if (Input.GetKey(KeyCode.Space))
        {
            jumpVelocity = 0.012f;
        }
        else
        {
            jumpVelocity = 0;
        }



        if (Input.GetKeyDown(KeyCode.Space))
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
                if (Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
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

        verticalVelocity = jumpVelocity + rb2d.linearVelocity.y;


        if (Input.GetKeyDown(KeyCode.Space) && secondJump && !Walled && verticalVelocity > 6)
        {
            jumpVelocity = 4;
            secondJump = false;
        }

        verticalVelocity = jumpVelocity + rb2d.linearVelocity.y;

        if (Input.GetKeyDown(KeyCode.Space) && secondJump && !Walled && verticalVelocity < 6)
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
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && verticalVelocity < -3)
            {
                verticalVelocity = -3;
            }
            else if (verticalVelocity < -1.5f)
            {
                verticalVelocity = -1.5f;
            }
        }

        horizontalVelocity = horizontal * controldamper + horizontalPush;

        rb2d.linearVelocity = new Vector2(horizontalVelocity * moveSpeed, verticalVelocity);
        velocity = rb2d.linearVelocity;
        //Debug.Log(rb2d.linearVelocity.y);
        Camera.main.transform.position = transform.position + new Vector3(0, 0, -100);

        if(!isFacingRight && horizontalVelocity > 0)
        {
            Flip();
        }
        else if(isFacingRight && horizontalVelocity < 0)
        {
            Flip();
        }
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
        if(coll.collider.CompareTag("MovingPlatform"))
        {
            Grounded = true;
            Walled = false;
            secondJump = false;
            Slide = false;
            Debug.Log("Moving Platform" + !Slide);
            //moveSpeed = 12.5f;
            //platformJump = 20;
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

    void OnCollisionExit2D(Collision2D coll)
    {
        if(coll.collider.CompareTag("MovingPlatform"))
        {
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
                Grounded = false;;
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
  
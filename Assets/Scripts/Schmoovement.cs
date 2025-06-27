using UnityEngine;

public class Schmoovement : MonoBehaviour

{

    private Rigidbody2D rb2d;
    private Collider2D capsule2d;
    private Animator animator;
    public bool Grounded = false;
    public bool secondJump = false;
    public bool Walled = false;
    public bool Slide = false;
    private bool isFacingRight;
    float collidex = 0;
    float myx = 0;
    private float horizontaly = 0;
    float controldamper = 1;
    public Vector2 velocity;
    public float moveSpeed;
    public float platformJump;
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
        float verticalVelocity;
        float horizontalPush = 0;
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
                if (collidex > myx)
                {
                    // The wall is to the left
                    jumpVelocity = 13;
                    horizontalPush = -3;
                    Slide = false;
                    controldamper = 0.4f;
                }

                if (collidex < myx)
                {
                    // The wall is to the right
                    jumpVelocity = 13;
                    horizontalPush = 3;
                    Slide = false;
                    controldamper = 0.4f;
                }
            }
        }



        verticalVelocity = jumpVelocity + (rb2d.linearVelocity.y);


        if (Input.GetKeyDown(KeyCode.Space) && secondJump && !Walled && verticalVelocity > 6)
        {
            jumpVelocity = 4;
            secondJump = false;
        }

        verticalVelocity = jumpVelocity + (rb2d.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && secondJump && !Walled && verticalVelocity < 6)
        {
            verticalVelocity = 8;
            secondJump = false;
        }

        controldamper = 1;



        if (Slide)
        {
            verticalVelocity = -4;
        }

        horizontaly = (horizontaly + horizontalPush) * 0.99f;
        horizontalVelocity = horizontaly + horizontal * controldamper;

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

    void OnCollisionStay2D(Collision2D coll)
    {
        if(coll.collider.CompareTag("MovingPlatform"))
        {
            Grounded = true;
            Walled = false;
            secondJump = false;
            Slide = false;
            moveSpeed = 12.5f;
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
            }
            else if (Mathf.Abs(normal.x) > 0.5f)
            {
                // The vector mostly points in x or -x direction. So we've hit a wall

                if (Walled!)
                {
                    Slide = true;
                }
                else
                {
                    Slide = false;
                }
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
                Grounded = false;
                secondJump = true;
            }

            if (Walled)
            {
                Walled = false;
                Slide = false;
            }
        }
    }
}
  
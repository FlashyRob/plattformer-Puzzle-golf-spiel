using UnityEngine;

public class Schmoovement : MonoBehaviour

{

    private Rigidbody2D rb2d;
    private Collider2D capsule2d;
    private Animator animator;
    public bool Grounded = false;
    public bool secondJump = false;
    public bool Walled = false;

    private bool isFacingRight;
    float collidex = 0;
    float myx = 0;
    private float horizontaly = 0;
    float controldamper = 1;

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
        bool slideVel;

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
            slideVel = false;

            if (Grounded && !Walled)
            {
                jumpVelocity = 9;

            }

            if (Walled)
            {
                jumpVelocity = 13;
                controldamper = 0.4f;

                if (collidex > myx)
                {
                    // The wall is to the left

                    horizontalPush = -3;
                }

                if (collidex < myx)
                {
                    // The wall is to the right

                    horizontalPush = 3;
                }
            }
        }
        else
        {
            slideVel = true;
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

        if (slideVel && Walled)
        {
            verticalVelocity = -4;
        }


        horizontalVelocity = rb2d.linearVelocity.x + horizontalPush + horizontal * 5;


        /*
        horizontaly = (horizontaly + horizontalPush) * 0.99f;
        horizontalVelocity = horizontaly + horizontal * controldamper;
        */
        Debug.Log("verticalVelocity "+verticalVelocity);
        Debug.Log("jumpVelocity " +jumpVelocity);
        rb2d.linearVelocity = new Vector2(horizontalVelocity * 5, verticalVelocity);

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
        if (coll.gameObject.tag == "Ground")
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
        if (coll.gameObject.tag == "Ground")
        {
            if (Grounded)
            {
                Grounded = false;
                secondJump = true;
            }

            if (Walled)
            {
                Walled = false;
            }
        }
    }
}
  
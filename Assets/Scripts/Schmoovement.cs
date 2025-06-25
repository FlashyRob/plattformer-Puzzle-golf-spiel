using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour

{   

    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsule2d;
    public LayerMask groundLayer;
    public bool Grounded = false;
    public bool secondJump = false;
    public bool Walled = false;
    float collidex = 0;
    float myx = 0;
    private float horizontaly = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        rb2d = GetComponent<Rigidbody2D>();
        capsule2d = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
      
       
        float horizontal = Input.GetAxis("Horizontal"); // key a pressed = -1 ; key d pressed = 1 ; no key pressed = 0
        float verticalVelocity; 
        float jumpVelocity;
        float horizontalPush = 0;
        float horizontalVelocity;



        if (Input.GetKey(KeyCode.Space))
        {
            jumpVelocity = 0.01f;
        }
        else
        {
            jumpVelocity = 0;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Grounded)
            {
                jumpVelocity = 9;
            }

            if (secondJump && !Walled)
            {
                jumpVelocity = 7;
                secondJump = false;
            }

            if (Walled)
            {
                if (collidex > myx && !Grounded)
                {
                    jumpVelocity = 8;
                    horizontalPush = 3;
                    Walled = false;
                }

                if (collidex < myx && !Grounded)
                {
                    jumpVelocity = 8;
                    horizontalPush = -3;
                    Walled = false;
                }
            }
        }
     
        
        verticalVelocity = jumpVelocity + (rb2d.linearVelocity.y); 

        horizontaly = (horizontaly + horizontalPush) * 0.99f;
        horizontalVelocity = horizontaly + horizontal;


        rb2d.linearVelocity = new Vector2(horizontalVelocity * 5, verticalVelocity);
        Debug.Log(rb2d.linearVelocity);
        Camera.main.transform.position = transform.position + new Vector3(0, 0, -100);
    }



    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground") 
        {
            if (this.transform.position.y > coll.collider.transform.position.y)
            {
                Grounded = true;
                secondJump = false;
                collidex = coll.collider.transform.position.x;
                myx = this.transform.position.x;
            }

            else
            {
                Walled = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ground")
        {
            if(Grounded)
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


    //    private void OnCollisionEnter2D(Collision2D other)
    //    {
    //        if (other.gameObject.CompareTag("Ground"))
    //        {
    //            Grounded = true;
    //            secondJump = false;
}
        
//        if (other.gameObject.CompareTag("Wall") && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
//       {
//            Walled = true;
//            otherx = other.transform.position.x;
//           myx = transform.position.x;
//        }   
//    }

//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            Grounded = false;
//            secondJump = true;
//        }
        
//        if (collision.gameObject.CompareTag("Wall"))
//        {
//            Walled = false;
//        }
//    }
   
  
  
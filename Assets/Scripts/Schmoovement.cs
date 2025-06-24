using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour

{
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsule2d;
    public new GameObject camera;
    public LayerMask groundLayer;
    public bool Grounded = false;

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
        float verticalVelocity = rb2d.linearVelocity.y;
        float jumpVelocity = 0;
        float verticalMultiplier;

        if (Input.GetKey(KeyCode.Space))
        {
            verticalMultiplier = 12;
        }
        else
        {
            verticalMultiplier = 10;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            jumpVelocity = 14;
        }

        verticalVelocity = (jumpVelocity * verticalMultiplier) / 30 + rb2d.linearVelocity.y;

        rb2d.linearVelocity = new Vector2(horizontal, verticalVelocity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("CollisionEnter");
        if (other.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("CollisionExit");
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }

}

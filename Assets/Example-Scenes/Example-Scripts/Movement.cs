using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    public float Move;

    public float speed;
    public float jump;
    public bool grounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", rb.linearVelocity.x);
        animator.SetFloat("JumpSpeed", rb.linearVelocity.y);

        Move = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(Move * speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jump * 10));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Grounded!", collision.gameObject);
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Nicht Grounded!", collision.gameObject);
            grounded = false;
        }
    }
}

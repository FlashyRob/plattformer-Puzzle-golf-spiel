using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour

{
    private Rigidbody2D rb2d;
    private CapsuleCollider2D capsule2d;
    public new GameObject camera;
    public LayerMask groundLayer;

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
        float jumpVelocity = 1;
        float verticalMultiplier = 1;
        verticalVelocity = jumpVelocity * verticalMultiplier;
      
        if (Input.GetKey(KeyCode.Space))
        {
            verticalMultiplier = 12;
        }
        else
        {
            verticalMultiplier = 10;
        }
        





    }
}

using UnityEngine;

public class Box : MonoBehaviour
{
    private PlatformMovement currentPlatform;
    private Rigidbody2D rb2d;

    private void Start()
    {
        var generateLevel = FindAnyObjectByType<GenerateLevel>();
        rb2d = GetComponent<Rigidbody2D>();
        if (!generateLevel.playMode)
        {
            Destroy(rb2d);
            GetComponent<SpriteRenderer>().color -= new Color(0, 0 , 0, 0.5f);
            enabled = false;
        }
    }


    private void FixedUpdate()
    {
        if (currentPlatform)
        {
            rb2d.linearVelocity = currentPlatform.velocity;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("MovingPlatform"))
            return;

        // Get bounds
        Bounds platformBounds = collision.collider.bounds;
        Bounds boxBounds = GetComponent<Collider2D>().bounds;

        float boxCenterX = boxBounds.center.x;

        // Check if box center is still horizontally above the platform
        if (boxCenterX >= platformBounds.min.x && boxCenterX <= platformBounds.max.x)
        {
            currentPlatform = collision.gameObject.GetComponent<PlatformMovement>();
        }
        else
        {
            // Center is hanging over the edge — stop sticking
            currentPlatform = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
        }
    }
}

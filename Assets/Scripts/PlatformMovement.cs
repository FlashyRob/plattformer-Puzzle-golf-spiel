using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public LayerMask boxLayer;
    public Vector2 movement;

    private int i = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal; // has length of 1

        if (normal.y < -0.5f)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
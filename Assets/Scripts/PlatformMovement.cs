using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public Vector2 velocity;
    private Rigidbody2D rb;

    private Vector2 lastPosition;

    private int i = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = points[startingPoint].position;
        rb.MovePosition(points[startingPoint].position);
        lastPosition = rb.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        //rb.MovePosition(Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime));
        Vector2 newPostion = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        velocity = (newPostion - lastPosition) / Time.deltaTime;
        if(velocity.magnitude > speed * 2) // if you pause the game the movement grows huge
        {
            velocity = velocity.normalized * speed;
        }
        rb.MovePosition(newPostion);
        //movement = (points[i].position - transform.position).normalized * speed;
        //movement = ((Vector2)transform.position - lastPosition);
        lastPosition = newPostion;
    }

}
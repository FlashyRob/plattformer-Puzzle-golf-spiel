using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public Vector2 velocity;
    private Vector2 lastPosition;
    public LayerMask boxLayer;

    private int i;
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

          private void FixedUpdate()
          {
              //velocity = (points[i].position - transform.position).normalized * speed;
              velocity = ((Vector2)transform.position - lastPosition) / Time.fixedDeltaTime;
              lastPosition = transform.position;
          }

    /*     private void OnCollisionStay2D(Collision2D collision)
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
         } */
}
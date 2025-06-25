using UnityEngine;

public class JumpThroughPlatform : MonoBehaviour
{
    private BoxCollider2D box;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.position.y <= (collision.transform.position.y - 1))
        {
            box.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        box.enabled = false;
    }
}

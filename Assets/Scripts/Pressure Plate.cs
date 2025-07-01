using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected general;
    public int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        update = FindAnyObjectByType<Updates>();
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box"))
        {
            update.SetPressurePlate(update.GetBlock(index).index, update.GetBlock(index), 0);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Box"))
        {
            update.SetPressurePlate(update.GetBlock(index).index, update.GetBlock(index), 1);
        }
    }

    
}

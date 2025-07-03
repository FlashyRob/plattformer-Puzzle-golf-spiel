using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected general;
    public int index;
    private SpriteRenderer spriteRenderer;
    private Sprite spriteOff;
    public Sprite spriteOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        update = FindAnyObjectByType<Updates>();
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();

        index = general.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOff = spriteRenderer.sprite;
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            update.SetPressurePlate(update.GetBlock(index).index, update.GetBlock(index), 0);

            spriteRenderer.sprite = spriteOff;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Box"))
        {
            update.SetPressurePlate(update.GetBlock(index).index, update.GetBlock(index), 1);

            spriteRenderer.sprite = spriteOn;
        }
    }

    
}

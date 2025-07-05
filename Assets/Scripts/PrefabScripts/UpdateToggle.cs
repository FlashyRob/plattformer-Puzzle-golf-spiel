using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class UpdateToggle : MonoBehaviour
{
    private int index;
    private int oldState;
    private Updates update;
    SpriteRenderer spriteRenderer;
    private Sprite spriteOff;
    private blockData block;

    public Sprite spriteOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        index = position.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);
        spriteOff = spriteRenderer.sprite;
        block = update.GetBlock(index);

    }

    // Update is called once per frame
    void Update()
    {
        if (block.state != oldState)
        {
            if (block.state == 0)
            {
                spriteRenderer.sprite = spriteOff;
            }
            else
            {
                spriteRenderer.sprite = spriteOn;
            }
        }
        oldState = block.state;



        switch (update.GetBlock(index).visualActive)
        {
            case 1:
                spriteRenderer.color = Color.white;
                break;
            case 0:
                spriteRenderer.color = Color.gray;
                break;
        }
    }
}

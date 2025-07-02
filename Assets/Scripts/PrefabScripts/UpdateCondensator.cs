using UnityEngine;

public class UpdateCondensator : MonoBehaviour

{
    private int index;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected position;
    private SpriteRenderer spriteRenderer;
    private Sprite spriteOff;

    public Sprite[] sprites = new Sprite[14];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        index = position.GetIndexFromXY((int) transform.position.x, (int) transform.position.y);
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOff = spriteRenderer.sprite;
    }

    void Update()
    {
        if (transform.position == GenerateLevel.mousePos && Input.GetKeyDown(KeyCode.Mouse1))
        {
            blockData block = update.GetBlock(index);
            update.ToggleSwitch(index, block);
            if (block.state == 0)
            {
                spriteRenderer.sprite = spriteOff;
            }
            else
            {
                spriteRenderer.sprite = sprites[block.state + 1];
            }
        }
    }
}

using UnityEngine;

public class UpdateButton : MonoBehaviour

{
    private int index;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected position;
    private SpriteRenderer spriteRenderer;
    private Sprite spriteOff;

    public Sprite spriteOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        index = position.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOff = spriteRenderer.sprite;
    }

    void Update()
    {
        if (transform.position == GenerateLevel.mousePos && Input.GetKey(KeyCode.Mouse1))
        {
            blockData block = update.GetBlock(index);
            update.SetButton(index, block, 1);

            spriteRenderer.sprite = spriteOn;
        } else
        {
            blockData block= update.GetBlock(index);
            update.SetButton(index, block, 0);

            spriteRenderer.sprite = spriteOff;
        }
    }
}

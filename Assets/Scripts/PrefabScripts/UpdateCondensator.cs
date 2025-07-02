using UnityEngine;

public class UpdateCondensator : MonoBehaviour
{
    private int index;
    private int oldState;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected position;
    private SpriteRenderer spriteRenderer;
    private Sprite spriteOff;
    private blockData block;

    public Sprite[] sprites = new Sprite[14];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        index = position.GetIndexFromXY((int) transform.position.x, (int) transform.position.y);
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOff = spriteRenderer.sprite;
        block = update.GetBlock(index);
    }

    void Update()
    {
        block = update.GetBlock(index);

        if (block.state != oldState)
        {
            int stage = Mathf.CeilToInt((float)block.state / 300.0f * 14);
            Debug.Log(stage);
            if (stage == 0)
            {
                spriteRenderer.sprite = spriteOff;
            }
            else
            {
                spriteRenderer.sprite = sprites[stage - 1];
            }
        }
        oldState = block.state;
    }
}

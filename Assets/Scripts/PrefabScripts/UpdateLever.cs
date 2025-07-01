using UnityEngine;

public class UpdateLever : MonoBehaviour

{
    public int index;
    private Updates update;
    SpriteRenderer spriteRenderer;

    private Sprite spriteOff;
    public Sprite spriteOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteOff = spriteRenderer.sprite;
    }

    void Update()
    {
        if (transform.position == GenerateLevel.mousePos && Input.GetKeyDown(KeyCode.Mouse1))
        {
            blockData block = update.GetBlock(index);
            update.ToggleLever(index, block);
            if (block.state == 0)
            {
                spriteRenderer.sprite = spriteOff;
            }
            else
            {
                spriteRenderer.sprite = spriteOn;
            }
        }
    }
}

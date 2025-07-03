using UnityEngine;

public class UpdateCable : MonoBehaviour
{
    public int index;
    private Updates update;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        index = position.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
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

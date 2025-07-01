using UnityEngine;

public class UpdateCable : MonoBehaviour
{
    public int index;
    private Updates update;
    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        update = FindAnyObjectByType<Updates>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

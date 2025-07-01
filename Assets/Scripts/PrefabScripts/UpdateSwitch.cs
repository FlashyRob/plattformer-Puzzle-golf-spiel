using UnityEngine;

public class UpdateSwitch : MonoBehaviour
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
        
    }
}

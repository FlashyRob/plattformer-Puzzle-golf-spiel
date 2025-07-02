using UnityEngine;

public class Door : MonoBehaviour
{
    public int index;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected general;
    private BoxCollider2D door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = GetComponent<BoxCollider2D>();
        update = FindAnyObjectByType<Updates>();
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
    }

    // Update is called once per frame
    void Update()
    {
        if (update.GetBlock(index).state == 1)
        {
            door.enabled = false;
        }
        else
        {
            door.enabled = true;
        }
    }
}
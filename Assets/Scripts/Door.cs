using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class Door : MonoBehaviour
{
    public int index;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected general;
    int blockstate = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        update = FindAnyObjectByType<Updates>();
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        index = general.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        blockData block = update.GetBlock(index);
        if (blockstate != block.visualActive)
        {
            if (block.visualActive == 1)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
            }
            blockstate = block.visualActive;
        }
    }
}
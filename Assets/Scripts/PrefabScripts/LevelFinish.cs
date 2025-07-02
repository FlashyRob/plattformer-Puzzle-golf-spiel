using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private int index;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected general;
    int blockstate = 0;

    private BoxCollider2D FinishTriggerCollider;

    private void Start()
    {
        FinishTriggerCollider = GetComponent<BoxCollider2D>();
        update = FindAnyObjectByType<Updates>();
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        index = general.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Schmoovement>())
        {
            Debug.Log("Level Finished. Sending back to Level Creator...");
            SceneManager.LoadScene("LevelCreator");
        }
    }

    void Update()
    {
        blockData block = update.GetBlock(index);
        if (blockstate != block.visualActive)
        {
            if (block.visualActive == 1)
            {
                FinishTriggerCollider.enabled = true;
            }
            else
            {
                FinishTriggerCollider.enabled = false;
            }
            blockstate = block.visualActive;
        }
    }
}

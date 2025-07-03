using UnityEngine;

public class Push : MonoBehaviour
{
    private Animator animator;
    private bool isActive = false;
    private bool isPressed = false;
    private int index;
    private Updates update;
    private CheckWheatherTwoBlocksAreConnected general;
    public Vector2 velocity = new Vector2(0,14);


    int blockstate = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        update = FindAnyObjectByType<Updates>();
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        index = general.GetIndexFromXY((int)transform.position.x, (int)transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("shouldPush", isActive);
        animator.SetBool("touchPlayer", isPressed);

        blockData block = update.GetBlock(index);
        if (blockstate != block.visualActive)
        {
            if (block.visualActive == 1)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
            blockstate = block.visualActive;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
            if (collision.CompareTag("Player") || collision.CompareTag("Box"))
            {
                isPressed = true;
                Debug.Log(transform.rotation * velocity);
                if (isActive && collision.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<Schmoovement>().PushBoost = transform.rotation * velocity;
                    collision.gameObject.GetComponent<Schmoovement>().gettingBoosted = true;
                }
                else if(isActive && collision.CompareTag("Box"))
                {
                    collision.gameObject.GetComponent<Box>().PushBoost = transform.rotation * velocity;
                }
            }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        isPressed = false;
    }
}

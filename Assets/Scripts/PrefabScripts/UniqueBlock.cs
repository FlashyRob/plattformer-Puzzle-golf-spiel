using System.Collections.Generic;
using UnityEngine;
public class UniqueBlock : MonoBehaviour
{
    public string blocktype = "unique";

    private void Start()
    {
        Invoke("CheckUnique", 0.05f); // we need this delay
        // When the level is saved and re-loaded we get a new block of the same time from the level load
        // but the old one still exists for a short time (or maybe just that frame). During our Start the old block is still around and we remove it from reader.blocks.
        // However the save and re-load scripts also removes it from reader.blocks.
        // We end up removing the block from the blocklist after the load, removing it from the level entirely.
        // To avoid this we wait a frame or two to make sure we don't catch blocks from before the re-load that haven't gotten cleaned up yet.
    }
    void CheckUnique()
    {
        var blocks = FindObjectsByType<UniqueBlock>(FindObjectsSortMode.InstanceID);

        List<UniqueBlock> blocksOfSameType = new List<UniqueBlock>();

        foreach (var block in blocks)
        {
            if (block.blocktype == blocktype)
            {
                blocksOfSameType.Add(block);
            }
        }

        blocksOfSameType.RemoveAt(0);

        foreach (var block in blocksOfSameType)
        {
            block.CleanupAndDestroy();
        }

    }

    void CleanupAndDestroy()
    {
        var reader = FindAnyObjectByType<JSONReader>();
        var pos = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        if (reader && pos)
            reader.RemoveBlock(pos.GetIndexFromXY(transform.position));

        Destroy(gameObject);
    }
}
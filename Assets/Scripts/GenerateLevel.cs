using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    private CheckWheatherTwoBlocksAreConnected position;
    private JSONReader reader;

    private GameObject[] block;
    List<string> blockNames = new List<string>();

    private GameObject createdBlocks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        reader = FindAnyObjectByType<JSONReader>();

        block = Resources.LoadAll<GameObject>("Blocks");
        for (int i = 0; i < block.Length; i++)
        {
            blockNames.Add(block[i].name);
        }

        createdBlocks = new GameObject("CreatedBlocks");

        reader.BlockSafeFile = reader.load();

        for (int i = 0; i < reader.BlockSafeFile.Count; i++)
        {
            blockData thisBlock = reader.BlockSafeFile[i];
            xy blockPos = position.GetXY(thisBlock.index);
            string blockType = thisBlock.type;
            if (blockType == null)
            {
                blockType = thisBlock.typetype;
            }
            int blockDirection = thisBlock.direction * 90;
            string blockName = "block:" + blockPos.x + "," + blockPos.y;
            GameObject blockPrefab;
        
            try
            {
                blockPrefab = block[blockNames.IndexOf(blockType)];

                GameObject newBlock = Instantiate(
                    blockPrefab,
                    new Vector3((float) blockPos.x, (float) blockPos.y, 0),
                    Quaternion.Euler(0, 0, blockDirection),
                    createdBlocks.transform
                );
                newBlock.name = blockName;
                newBlock.AddComponent<RemoveBlock>();
            } catch { }
        }

        Editor editor;
        editor = FindAnyObjectByType<Editor>();
        if (editor != null) {
            editor.StartEditor();
        }
    }
}

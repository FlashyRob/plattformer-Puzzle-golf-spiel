using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns level blocks to re-create the saved level loaded by JSONReader. If an LevelEditor exists it's initialized.
/// </summary>
public class GenerateLevel : MonoBehaviour
{
    private CheckWheatherTwoBlocksAreConnected position;
    private JSONReader reader;

    private GameObject[] block;
    List<string> blockNames = new List<string>();

    private GameObject createdBlocks;

    public static bool creative = false;
    public static Vector3 mousePos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        if (!position)
        {
            position = gameObject.AddComponent<CheckWheatherTwoBlocksAreConnected>();
        }
        reader = FindAnyObjectByType<JSONReader>();
        if (!reader)
        {
            reader = gameObject.AddComponent<JSONReader>();
        }

        block = Resources.LoadAll<GameObject>("Blocks");
        for (int i = 0; i < block.Length; i++)
        {
            blockNames.Add(block[i].name);
        }

        createdBlocks = new GameObject("CreatedBlocks");
        reader.LoadLevel();
        Load();

        LevelEditor editor;
        editor = FindAnyObjectByType<LevelEditor>();
        if (editor != null)
        {
            editor.StartEditor();
        }
    }   
    public void Load()
    {
        // remove all blocks to clear (if we are loading a different level file)
        int childCount = createdBlocks.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(createdBlocks.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < reader.blockSafeFile.Count; i++)
        {
            blockData thisBlock = reader.blockSafeFile[i];
            xy blockPos = position.GetXY(thisBlock.index);
            string blockType = thisBlock.type;
            int blockDirection = thisBlock.direction * -90;
            string blockName = "block:" + blockPos.x + "," + blockPos.y;
            GameObject blockPrefab;

            try
            {
                blockPrefab = block[blockNames.IndexOf(blockType)];

                GameObject newBlock = Instantiate(
                    blockPrefab,
                    new Vector3((float)blockPos.x, (float)blockPos.y, 0),
                    Quaternion.Euler(0, 0, blockDirection),
                    createdBlocks.transform
                );
                newBlock.name = blockName;
                newBlock.AddComponent<RemoveBlock>();
                UpdateCable updateCable = newBlock.GetComponent<UpdateCable>();
                if (updateCable != null) updateCable.index = thisBlock.index;
            }
            catch { }
        }

    }
    void GetMousePos()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y),
            0
        );
    }

    private void Update()
    {
        GetMousePos();              
    }
}

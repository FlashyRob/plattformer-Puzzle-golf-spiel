using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    public blockData myBlock = new blockData();

    public List<blockData> BlockSafeFile;

    [System.Serializable]
    public class BlockList
    {
        public List<blockData> blocks;
    }

    public BlockList myBlockList = new BlockList();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        myBlockList = JsonUtility.FromJson<BlockList>(textJSON.text);

        //Debug.Log(myBlockList.blocks); //Wenn nicht mehr funktioniert diese Zeile wieder einbauen. 

        BlockSafeFile = load();

        RemoveBlock(1);
    }

    private void Update()
    {
        SafeSafeFile();
    }

    public List<blockData> load()
    {
        return new List<blockData>(JsonUtility.FromJson<BlockList>(textJSON.text).blocks);
    }

    public void RemoveBlock(int index)
    {
        for (int i = 0; i < BlockSafeFile.Count; i++)
        {
            if (BlockSafeFile[i].index == index)
            {
                BlockSafeFile.RemoveAt(i);
                SafeSafeFile();
            }
        }

    }

    public int[] getInputDirectionsOfIndex(int index)
    {
        int[] directions = new int[] { 0, 0, 0, 0 };
        for (int i = 0; i < BlockSafeFile.Count; i++)
        {
            if (BlockSafeFile[i].index == index)
            {
                directions = BlockSafeFile[i].inputDirections;
            }
        }
        return (directions);
    }

    public int[] getOutputDirectionsOfIndex(int index)
    {
        int[] directions = new int[] { 0, 0, 0, 0 };
        for (int i = 0; i < BlockSafeFile.Count; i++)
        {
            if (BlockSafeFile[i].index == index)
            {
                directions = BlockSafeFile[i].outputDirections;
            }
        }
        return (directions);
    }

    public void AddBlock(blockData block)
    {
        BlockSafeFile.Add(block);

        SafeSafeFile();
    }

    public void EditBlock(blockData block)
    {
        RemoveBlock(block.index);
        AddBlock(block);

        SafeSafeFile();
    }

    public bool BlockExists(int index)
    {
        foreach (blockData block in BlockSafeFile)
        {
            if (block.index == index)
                return true;
        }
        return false;
    }

    public void EditBlockDirection(blockData block, int newVal)
    {
        RemoveBlock(block.index);
        block.direction = newVal;
        AddBlock(block);

        SafeSafeFile();
    }

    public void EditBlockType(blockData block, string newVal)
    {
        RemoveBlock(block.index);
        block.type = newVal;
        AddBlock(block);

        SafeSafeFile();
    }
    public void EditBlockTypeType(blockData block, string newVal)
    {
        RemoveBlock(block.index);
        block.typetype = newVal;
        AddBlock(block);

        SafeSafeFile();
    }
    public void EditBlockState(blockData block, int newVal)
    {
        RemoveBlock(block.index);
        block.state = newVal;
        AddBlock(block);

        SafeSafeFile();
    }

    public void EditBlockMeta(blockData block, string newVal)
    {
        RemoveBlock(block.index);
        block.meta = newVal;
        AddBlock(block);

        SafeSafeFile();
    }
    public void EditBlockVisualActive(blockData block, int newVal)
    {
        RemoveBlock(block.index);
        block.visualActive = newVal;
        AddBlock(block);

        SafeSafeFile();
    }

    public void SafeSafeFile()
    {
        BlockList b = new BlockList();
        b.blocks = BlockSafeFile;
        var j = JsonUtility.ToJson(b);
        File.WriteAllText("Assets/Resources/JSONLevelFiles" + "/JSONLevelFiles.txt", j);
    }
}

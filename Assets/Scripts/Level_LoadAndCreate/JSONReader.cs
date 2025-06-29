using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class JSONReader : MonoBehaviour
{
    public string DefaultSaveFile = "Level_0";
    public List<blockData> BlockSafeFile;
    
    private string SavePath(string level_name)
    {
        return Application.persistentDataPath+ "\\" +level_name +".json";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BlockSafeFile = load();
        RemoveBlock(1);
    }

    public List<blockData> load()
    {
        string path = SavePath(DefaultSaveFile);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BlockList loaded = JsonUtility.FromJson<BlockList>(json);
            Debug.Log("Loaded " + loaded.blocks.Count + " blocks from save file " + path);
            return new List<blockData>(loaded.blocks);
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + path);
            return new List<blockData>();
        }
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
        // Debug.Log("Saved " + BlockSafeFile.Count + " blocks to save file " + SavePath(DefaultSaveFile));

        BlockList b = new BlockList();
        b.blocks = BlockSafeFile;
        var j = JsonUtility.ToJson(b);

        File.WriteAllText(SavePath(DefaultSaveFile), j);
    }

    [System.Serializable]
    public class BlockList
    {
        public List<blockData> blocks;
    }
}

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class JSONReader : MonoBehaviour
{
    public string saveName = "Level_0";
    public List<blockData> blockSafeFile;

    private ScannerinoCrocodilo scanner;
    private Updates update;

    private string SavePath(string level_name)
    {
        return Application.persistentDataPath+ "\\" +level_name +".json";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blockSafeFile = LoadLevel();
        RemoveBlock(1);

        scanner = FindAnyObjectByType<ScannerinoCrocodilo>();
        update = FindAnyObjectByType<Updates>();
    }

    public List<blockData> LoadLevel()
    {
        string path = SavePath(saveName);
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
        for (int i = 0; i < blockSafeFile.Count; i++)
        {
            if (blockSafeFile[i].index == index)
            {
                blockSafeFile.RemoveAt(i);
                SafeSafeFile();
            }
        }

    }

    public int[] GetInputDirectionsOfIndex(int index)
    {
        int[] directions = new int[] { 0, 0, 0, 0 };
        for (int i = 0; i < blockSafeFile.Count; i++)
        {
            if (blockSafeFile[i].index == index)
            {
                directions = blockSafeFile[i].inputDirections;
            }
        }
        return (directions);
    }

    public int[] GetOutputDirectionsOfIndex(int index)
    {
        int[] directions = new int[] { 0, 0, 0, 0 };
        for (int i = 0; i < blockSafeFile.Count; i++)
        {
            if (blockSafeFile[i].index == index)
            {
                directions = blockSafeFile[i].outputDirections;
            }
        }
        return (directions);
    }

    public void AddBlock(blockData block)
    {
        blockSafeFile.Add(block);

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
        foreach (blockData block in blockSafeFile)
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

        BlockList b = new BlockList
        {
            blocks = blockSafeFile
        };
        var j = JsonUtility.ToJson(b);

        File.WriteAllText(SavePath(saveName), j);

        update.updateLoop = false;

       
    }

    [System.Serializable]
    public class BlockList
    {
        public List<blockData> blocks;
    }
}

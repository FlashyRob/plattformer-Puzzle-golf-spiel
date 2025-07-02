using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class JSONReader : MonoBehaviour
{
    public string saveName = "tmp";
    public List<blockData> blockSafeFile;

    private ScannerinoCrocodilo scanner;
    private Updates update;

    private string SavePath(string level_name)
    {
        // in the build, players can create their own levels. These will be saved in AppData/LocalLow 
        return Application.persistentDataPath + "\\" + level_name + ".json";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
        RemoveBlock(1);

        scanner = FindAnyObjectByType<ScannerinoCrocodilo>();
        update = FindAnyObjectByType<Updates>();

        InvokeRepeating("SaveSaveFile", 5, 30);
    }

    public void ClearLevel()
    {
        blockSafeFile = new List<blockData>();
    }

    public void LoadLevel()
    {
        string json = GetLevelJson(saveName);
        if (json != null)
        {
            BlockList loaded = JsonUtility.FromJson<BlockList>(json);
            blockSafeFile = new List<blockData>(loaded.blocks);
        }
        else
        {
            ClearLevel();
        }
    }

    private string GetLevelJson(string levelName)
    {
        // Try persistent data path first
        string persistentPath = Path.Combine(Application.persistentDataPath, levelName + ".json");
        if (File.Exists(persistentPath))
        {
            Debug.Log("Level loaded from " + persistentPath);
            return File.ReadAllText(persistentPath);
        }

        // If not found, try loading from Resources
        TextAsset textAsset = Resources.Load<TextAsset>("JSONLevelFiles/" + levelName);
        if (textAsset != null)
        {
            Debug.Log("Level loaded from " + resourcesFolderPath);
            return textAsset.text;
        }

        Debug.LogWarning("Level not found in persistent data or Resources: " + levelName);
        return null;
    }


    public void RemoveBlock(int index)
    {
        for (int i = 0; i < blockSafeFile.Count; i++)
        {
            if (blockSafeFile[i].index == index)
            {
                blockSafeFile.RemoveAt(i);
                BlockWasEdited();
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

        BlockWasEdited();
    }

    public void EditBlock(blockData block)
    {
        RemoveBlock(block.index);
        AddBlock(block);

        BlockWasEdited();
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

        BlockWasEdited();
    }

    public void EditBlockType(blockData block, string newVal)
    {
        RemoveBlock(block.index);
        block.type = newVal;
        AddBlock(block);

        BlockWasEdited();
    }
    public void EditBlockState(blockData block, int newVal)
    {
        RemoveBlock(block.index);
        block.state = newVal;
        AddBlock(block);

        BlockWasEdited();
    }

    public void EditBlockMeta(blockData block, string newVal)
    {
        RemoveBlock(block.index);
        block.meta = newVal;
        AddBlock(block);

        BlockWasEdited();
    }
    public void EditBlockVisualActive(blockData block, int newVal)
    {
        RemoveBlock(block.index);
        block.visualActive = newVal;
        AddBlock(block);

        BlockWasEdited();
    }

    public void SaveSaveFile()
    {

        if (update.updateLoop == true)
        {
            return;
        }
        // Debug.Log("Saved " + BlockSafeFile.Count + " blocks to save file " + SavePath(DefaultSaveFile));

        BlockList b = new BlockList
        {
            blocks = blockSafeFile
        };
        var j = JsonUtility.ToJson(b);

        SaveJson(saveName, j);

        if (!update)
        {
            update = FindAnyObjectByType<Updates>();
        }
        if (update)
        {
            update.updateLoop = false;
        }
        else
        {
            Debug.LogWarning("JSON READER has no Update reference");
        }

    }

    public void BlockWasEdited()
    {
        update.updateLoop = false;
    }


    private string resourcesFolderPath = "Assets/Resources/JSONLevelFiles/";
    private void SaveJson(string levelName, string json)
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            // Save to Resources folder in editor mode (write to Assets folder)
            string path = Path.Combine(resourcesFolderPath, levelName + ".json");
            File.WriteAllText(path, json);
            Debug.Log("Saved level to Resources folder: " + path);

            // IMPORTANT: In Editor, refresh AssetDatabase so Unity notices the new file
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.ImportAsset(resourcesFolderPath + levelName + ".json");
            UnityEditor.AssetDatabase.Refresh();
#endif
            return;
        }
#endif

        // Save to persistentDataPath in builds
        string savePath = Path.Combine(Application.persistentDataPath, levelName + ".json");
        File.WriteAllText(savePath, json);
        Debug.Log("Saved level to persistent data path: " + savePath);
    }

    public List<string> GetAllLevels()
    {
        var levels = new List<string>();

        // 1. Get player saves from persistent data
        string persistentPath = Application.persistentDataPath;
        if (Directory.Exists(persistentPath))
        {
            foreach (var file in Directory.GetFiles(persistentPath, "*.json"))
            {
                levels.Add(Path.GetFileNameWithoutExtension(file));
            }
        }

        // 2. Load all level TextAssets from Resources/JSONLevelFiles
        TextAsset[] resourcesLevels = Resources.LoadAll<TextAsset>("JSONLevelFiles");
        foreach (var ta in resourcesLevels)
        {
            if (!levels.Contains(ta.name))
                levels.Add(ta.name);
        }

        return levels;
    }


    [System.Serializable]
    public class BlockList
    {
        public List<blockData> blocks;
    }
}

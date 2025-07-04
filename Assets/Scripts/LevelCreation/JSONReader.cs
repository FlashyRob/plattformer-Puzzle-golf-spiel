using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class JSONReader : MonoBehaviour
{
    public string saveName = "tmp";
    public List<blockData> blockSafeFile;

    private ScannerinoCrocodilo scanner;
    private Updates update;
    private LevelCreatorUI levelCreatorUI;

    private string SavePath(string level_name)
    {
        // in the build, players can create their own levels. These will be saved in AppData/LocalLow 
        return Application.persistentDataPath + "\\" + level_name + ".json";
    }


    private void Awake()
    {
        SelectedLevelPersistent selectedLevel = FindAnyObjectByType<SelectedLevelPersistent>();
        if (selectedLevel)
        {
            saveName = selectedLevel.level;
        }
        else
        {
            if (!Application.isEditor)// I dont want the tmp.json to show up in the build game
            {
                saveName = "";
                var availableLevels = GetAllLevels();
                foreach(var level in availableLevels)
                {
                    if(level != "tmp")
                    {
                        saveName = level;
                        break;
                    }
                }
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel();
        RemoveBlock(1);

        scanner = FindAnyObjectByType<ScannerinoCrocodilo>();
        update = FindAnyObjectByType<Updates>();
        levelCreatorUI = FindAnyObjectByType<LevelCreatorUI>();

        // Only auto save in creation mode
        if (!FindAnyObjectByType<GenerateLevel>().playMode)
        {
            InvokeRepeating("SaveSaveFile", 5, 30);
        }
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
        SaveSaveFile(false);
    }

    public void SaveSaveFile(bool pauseUpdateLoop = false)
    {
        if(IsDeveloperLevel(saveName) && !Application.isEditor && levelCreatorUI.levelSaved)
        {
            // in build mode having a developer level open that has not been changed should not lead to a (copy) copy via auto save.
            return; 
        }
        if (IsDeveloperLevel(saveName) && !Application.isEditor && saveName != "tmp")
        {
            saveName = saveName + "(copy)";
        }
        if (pauseUpdateLoop) // allows us to force a save if the user presses the button
        {
            update.updateLoop = false;
        }
        if (update.updateLoop == true)
        {
            Debug.Log("Rejected Saving the file. Update loop is running");
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
        if (levelCreatorUI)
        {
            levelCreatorUI.LevelSaved();
        }
        Debug.Log("File Saved");

    }



    public void BlockWasEdited()
    {
        update.updateLoop = false;
        if (levelCreatorUI)
        {
            levelCreatorUI.LevelUnsaved();
        }

    }


    private string resourcesFolderPath = "Assets/Resources/JSONLevelFiles/";
    private void SaveJson(string levelName, string json)
    {
        if (Application.isEditor)
        {
            if(!IsDeveloperLevel(saveName) && IsUserLevel(saveName))
            {
                SaveToPersistent(levelName, json); // if the file aready exists and is in users persistent, we overwrite there.
            }
            else
            {
                // runing in unity editor saves to Resources as those are "developer" levels.
                SaveToResources(levelName, json);
            }
        }
        else // in Build always save to persistent (appdata/locallow/unitygames/ThisGameName)
        {
            SaveToPersistent(levelName, json);
        }
    }

    private void SaveToResources(string levelName, string json)
    {
#if UNITY_EDITOR
        if (Application.isEditor)
        {
            // Save to Resources folder in editor mode (write to Assets folder)
            string path = Path.Combine(resourcesFolderPath, levelName + ".json");
            File.WriteAllText(path, json);
            Debug.Log("Saved level to Resources folder: " + path);

            // IMPORTANT: In Editor, refresh AssetDatabase so Unity notices the new file

            UnityEditor.AssetDatabase.ImportAsset(resourcesFolderPath + levelName + ".json");
            UnityEditor.AssetDatabase.Refresh();

            return;
        }
#endif
    }
    private void SaveToPersistent(string levelName, string json)
    {
        // Save to persistentDataPath in builds
        string savePath = Path.Combine(Application.persistentDataPath, levelName + ".json");
        File.WriteAllText(savePath, json);
        Debug.Log("Saved level to persistent data path: " + savePath);
    }



    public void DeleteJson(string levelName)
    {

        if (IsDeveloperLevel(levelName)){
            if (Application.isEditor)
            {
                DeleteFromPersistent(levelName); // also deletes the level by the same name from users persistent. This shouldnt really happen though
                DeleteFromResources(levelName);
            }
            else
            {
                Debug.Log("Build cannot delete developer levels");
            }
        }
        else if(IsUserLevel(levelName))
        {
            DeleteFromPersistent(levelName);
        }
    }

    private void DeleteFromResources(string levelName)
    {
#if UNITY_EDITOR
        // Delete from Resources folder in Editor mode
        string path = Path.Combine(resourcesFolderPath, levelName + ".json");
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Deleted level from Resources folder: " + path);

            // Refresh AssetDatabase so Unity notices the file is gone
            UnityEditor.AssetDatabase.DeleteAsset(resourcesFolderPath + levelName + ".json");
            UnityEditor.AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("Level not found in Resources folder: " + path);
        }
#endif
    }

    private void DeleteFromPersistent(string levelName)
    {
        // Delete from persistentDataPath in builds
        string savePath = Path.Combine(Application.persistentDataPath, levelName + ".json");
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Deleted level from persistent data path: " + savePath);
        }
        else
        {
            Debug.LogWarning("Level not found in persistent data path: " + savePath);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SaveSaveFile();
            scanner.Scanner();
            update.updateLoop = true;
        }
    }

    public bool IsDeveloperLevel(string levelname)
    {
        return GetDeveloperLevels().Contains(levelname);
    }

    public bool IsUserLevel(string levelname)
    {
        return GetUserLevels().Contains(levelname);
    }

    private List<string> GetUserLevels()
    {
        var levels = new List<string>();
        // Get player saves from persistent data
        string persistentPath = Application.persistentDataPath;
        if (Directory.Exists(persistentPath))
        {
            foreach (var file in Directory.GetFiles(persistentPath, "*.json"))
            {
                levels.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        return levels;
    }

    private List<string> GetDeveloperLevels()
    {
        var levels = new List<string>();
        // Load all level TextAssets from Resources/JSONLevelFiles
        TextAsset[] resourcesLevels = Resources.LoadAll<TextAsset>("JSONLevelFiles");
        foreach (var ta in resourcesLevels)
        {
            if (!levels.Contains(ta.name))
                levels.Add(ta.name);
        }

        return levels;
    }

    public List<string> GetAllLevels()
    {
        var developerLevels = GetDeveloperLevels();
        var userLevels = GetUserLevels();

        // Merge the two lists while avoiding duplicates
        return developerLevels
            .Concat(userLevels)
            .Distinct()
            .ToList();
    }


    [System.Serializable]
    public class BlockList
    {
        public List<blockData> blocks;
    }
}

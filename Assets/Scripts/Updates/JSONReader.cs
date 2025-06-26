using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    //[System.Serializable]
    /*public class Block
    {
        public int index;
        public string typetype;
        public int direction;
        public int state;
        public int[] inputDirections;
        public int[] outputDirections;
        public string meta;
        public int visualActive;
    }
    */
    public blockData myBlock = new blockData();
    
    public void WriteJSON()
    {
        string strOutput = JsonUtility.ToJson(myBlock);
        File.WriteAllText("C:/Users/sp25-2/Documents/GitHub/plattformer-Puzzle-golf-spiel/Assets/Resources/JSONLevelFiles" + "/JSONLevelFileTest1.txt", strOutput);
    }

    public List<blockData> BlockSafeFile;

    [System.Serializable]
    public class BlockList
    {
        public List <blockData> blocks;
    }

    public BlockList myBlockList = new BlockList();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WriteJSON();
        
        myBlockList = JsonUtility.FromJson<BlockList>(textJSON.text);

        //Debug.Log(myBlockList.blocks); //Wenn nicht mehr funktioniert diese Zeile wieder einbauen. 

        BlockSafeFile = load();

        RemoveBlock(1);
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
            }
        }
        
    }

    public void AddBlock(blockData block)
    {

    }

    public bool BlockExists (int index)
    {
        foreach (blockData block in BlockSafeFile)
        {
            if (block.index == index)
                return true;
        }
        return false; 
    }
}

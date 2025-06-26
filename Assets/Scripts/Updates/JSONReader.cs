using UnityEngine;
using System.IO;

public class JSONReader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]
    public class Block
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

    public Block myBlock = new Block();

    public void OutputJSON()
    {
        string strOutput = JsonUtility.ToJson(myBlock);
        File.WriteAllText("C:/Users/sp25-2/Documents/GitHub/plattformer-Puzzle-golf-spiel/Assets/Resources/JSONLevelFiles" + "/JSONLevelFileTest1.txt", strOutput);
    }


    [System.Serializable]
    public class BlockList
    {
        public Block[] blocks;
    }

    public BlockList myBlockList = new BlockList();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OutputJSON();

        Debug.Log(Application.dataPath + "/JSONLevelFileTest1.txt");
        
        myBlockList = JsonUtility.FromJson<BlockList>(textJSON.text);

        //Debug.Log(myBlockList.blocks); //Wenn nicht mehr funktioniert diese Zeile wieder einbauen. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

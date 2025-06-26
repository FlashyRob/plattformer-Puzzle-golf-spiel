using UnityEngine;

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

    [System.Serializable]
    public class BlockList
    {
        public Block[] blocks;
    }

    public BlockList myBlockList = new BlockList();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myBlockList = JsonUtility.FromJson<BlockList>(textJSON.text); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

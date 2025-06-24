using System.Collections.Generic;
using UnityEngine;

public class Updates : MonoBehaviour
{

    public connectionData[] blocks = new connectionData[20];
    public int[,] activeSides = new int[20,4];
    public blockData[] blockData = new blockData[20];


    

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < blocks.Length; i++)
        {
            var c = new connectionData();
            c.sides = new int[] { 0, 1, 2, 3 };
            c.data = new Dictionary<int, List<connections>> { { 0, new List<connections>() }, { 1, new List<connections>()}, { 2, new List<connections>()}, { 3, new List<connections>()} };

            blocks[i] = c;
        }

        AddConnection(0, 0, new connections { outputIndex = 6, outputSide = 1 });
        AddConnection(0, 0, new connections { outputIndex = 7, outputSide = 3 });


        var l = GetConnections(0,0);
        for(int i= 0; i < l.Count; i++)
        {
            Debug.Log(l[i].outputIndex+ " " +l[i].outputSide);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < blockData.Length; i++)
        {
            blockData block = blockData[i];

            switch  (block.type)
            {
                case "wire":
                    handleWire(i, block);
                    break;

            }
        }
    }

    public void AddConnection(int blockIndex, int side, connections connection)
    {
        connectionData currentConnection = blocks[blockIndex];
        currentConnection.data[side].Add(connection);
        

    }

    public List<connections> GetConnections(int blockIndex, int side)
    {
        var l = blocks[blockIndex].data[side];
        return (l);
    }

    public bool isActive(int index, int side)
    {
        return (activeSides[index, side] == 1);
    }

    public bool checkActive(List<connections> sources)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            connections source = sources[i];
            if (isActive(source.outputIndex, source.outputSide))
                return true;

        }
        return false;
    }

    private void handleWire(int i, blockData block)
    {
        switch (block.typetype)
        {
            case "wire_straight":
                int[] activeConnections = checkConnectionSides(block.inputDirections, GetConnections(i, 0));
                if (activeConnections == 1)
                    {
                        
                    }
                break;
        } 
    }


    private int[] checkConnectionSides(int[] connectionSides, List<connections> sources)
    {
        int[] activeConnectionSides = new int[4] {0,0,0,0};
        for (int side = 0; side < 4; side++)
        {
            if (connectionSides[side] == 1)
            {
                if (checkActive(sources))
                {
                    activeConnectionSides[side] = 1;
                }
            }
        }
        return (activeConnectionSides);
    }
}

public struct connectionData
{
    public int[] sides;
    public Dictionary<int, List<connections>> data;
}

public struct connections
{
    public int outputIndex;
    public int outputSide;
}

public struct blockData
{
    public string type;
    public string typetype;
    public int direction;
    public int[] outputDirections;
    public int[] inputDirections;
    public int state;
    public string meta;
    public int visualActive;
}
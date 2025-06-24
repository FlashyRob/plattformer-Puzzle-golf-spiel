using System.Collections.Generic;
using UnityEngine;

public class Updates : MonoBehaviour
{

    public connectionData[] blocks = new connectionData[20];
    public int[,] activeSides = new int[20,4];
    public int[]


    

    


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

    public int isActive(int index, int side)
    {
        return (activeSides[index, side]);
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
    public string meta;
    public byte directions;
    public int height;
}

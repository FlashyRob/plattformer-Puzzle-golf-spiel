using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Updates : MonoBehaviour
{

    public connectionData[] blocks = new connectionData[20];
    public int[,] activeSides = new int[20, 4];
    public blockData[] blockData = new blockData[20];







    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            var c = new connectionData();
            c.sides = new int[] { 0, 1, 2, 3 };
            c.data = new Dictionary<int, List<connections>> { { 0, new List<connections>() }, { 1, new List<connections>() }, { 2, new List<connections>() }, { 3, new List<connections>() } };

            blocks[i] = c;
        }

        AddConnection(0, 0, new connections { outputIndex = 6, outputSide = 1 });
        AddConnection(0, 0, new connections { outputIndex = 7, outputSide = 3 });


        var l = GetConnections(0, 0);
        for (int i = 0; i < l.Count; i++)
        {
            Debug.Log(l[i].outputIndex + " " + l[i].outputSide);
        }


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < blockData.Length; i++)
        {
            blockData block = blockData[i];

            switch (block.type)
            {
                case "wire":
                    HandleWire(i, block);
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

    public bool IsActive(int index, int side)
    {
        return (activeSides[index, side] == 1);
    }

    public bool CheckActive(List<connections> sources)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            connections source = sources[i];
            if (IsActive(source.outputIndex, source.outputSide))
                return true;

        }
        return false;
    }

    private void HandleWire(int i, blockData block)
    {
        int[] activeConnections;

        switch (block.typetype)
        {
            case "wire_straight":
                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 1));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }


                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 3));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }
                EditVisualActive(i, 0);
                break;

            case "wire_curve":
                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 0));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }


                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 1));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }
                break;

            case "wire_t":
                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 0));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }

                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 1));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }

                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 3));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }
                break;

            case "wire_cross":
                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 0));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }

                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 1));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }

                activeConnections = CheckConnectionSides(block.inputDirections, GetConnections(i, 3));
                if (activeConnections.Contains(1))
                {
                    EditVisualActive(i, 1);
                    break;
                }
                break;

        }
    }

    public bool IsAnyConnectionActive(int[] directions, int blockIndex, int side)
    {
        int[] activeConnections;
        activeConnections = CheckConnectionSides(directions, GetConnections(blockIndex, side));
        if (activeConnections.Contains(1))
        {
            return true;
        }
        return false;
    }

    public void EditBlockActiveSide(int index, int side, int edit)
    {
        blockData block = GetBlock(index);
        block.outputDirections[side] = edit;
    }

    public void EditVisualActive(int index, int edit)
    {
        blockData block = GetBlock(index);
        block.visualActive = edit;
    }
        
    public blockData GetBlock(int index)
    {
        blockData block = blockData[index];
        return block;
    }


    private int[] CheckConnectionSides(int[] connectionSides, List<connections> sources)
    {
        int[] activeConnectionSides = new int[4] {0,0,0,0};
        for (int side = 0; side < 4; side++)
        {
            if (connectionSides[side] == 1)
            {
                if (CheckActive(sources))
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
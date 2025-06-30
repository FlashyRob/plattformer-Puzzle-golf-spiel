using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Updates : MonoBehaviour
{
    private CheckWheatherTwoBlocksAreConnected position;
    private JSONReader reader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = FindAnyObjectByType <CheckWheatherTwoBlocksAreConnected>();
        if (!position)
        {
            position = gameObject.AddComponent<CheckWheatherTwoBlocksAreConnected>();
        }
        reader = FindAnyObjectByType<JSONReader>();
        if (!reader)
        {
            reader = gameObject.AddComponent<JSONReader>();
        }

        AddConnection(0, 0, new connections { outputIndex = 6, outputSide = 1 });
        AddConnection(0, 0, new connections { outputIndex = 7, outputSide = 3 });
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < reader.blockSafeFile.Count; i++)
        {
            blockData block = reader.blockSafeFile[i];

            switch  (block.type)
            {
                case "wire_straight":
                    HandleWireStraight(block.index, block);
                    break;
                case "wire_curve":
                    HandleWireCorner(block.index, block);
                    break;
                case "wire_T":
                    HandleWireT(block.index, block);
                    break;
                case "wire_corner":
                    HandleWireCorner(block.index, block);
                    break;
                case "and_gate":
                    HandleAndGate(block.index, block);
                    break;
                case "or_gate":
                    HandleOrGate(block.index, block);
                    break;
                case "xor_gate":
                    HandleXorGate(block.index, block);
                    break;
                case "not_gate":
                    HandleNotGate(block.index, block);
                    break;
                case "lamp":
                    HandleLamp(block.index, block);
                    break;
                case "door":
                    HandleDoor(block.index, block);
                    break;
                case "flip_flop":
                    HandleFlipFlop(block.index, block);
                    break;
                case "toggle":
                    HandleToggle(block.index, block);
                    break;
                case "pulse":
                    HandlePulse(block.index, block);
                    break;
                case "condensator":
                    HandleCondensator(block.index, block);
                    break;
                case "battery":
                    HandleBattery(block.index);
                    break;
            }
        }
    }

    public void AddConnection(int blockIndex, int side, connections connection)
    {
        blockData thisBlock = GetBlock(blockIndex);
        thisBlock.connectionData[side].Add(connection);
    }

    public List<connections> GetConnections(int blockIndex, int side)
    {
        blockData thisBlock = GetBlock(blockIndex);
        var l = thisBlock.connectionData[side];
        return (l);
    }

    public bool isActive(int index, int side)
    {
        blockData thisBlock = GetBlock(index);
        var l = thisBlock.activeSides[side];
        return (l);
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

    private void HandleWireStraight(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 3))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 1))
        {
            EditVisualActive(i, 1);
        }
        EditVisualActive(i, 0);
    }

    private void HandleWireCorner(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 1))
        {
            EditVisualActive(i, 1);
        }
        EditVisualActive(i, 0);
    }

    private void HandleWireT(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 1))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 3))
        {
            EditVisualActive(i, 1);
        }
        EditVisualActive(i, 0);
    }

    private void HandelWireCross(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 1))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 3))
        {
            EditVisualActive(i, 1);
        }
        EditVisualActive(i, 0);
    }

    private void HandleLamp(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 1))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 3))
        {
            EditVisualActive(i, 1);
        }
        EditVisualActive(i, 0);
    }

    private void HandleBattery(int i)
    {
        EditVisualActive(i, 1);

        EditBlockActiveSide(i, 0, 1);
        EditBlockActiveSide(i, 1, 1);
        EditBlockActiveSide(i, 2, 1);
        EditBlockActiveSide(i, 3, 1);
    }

    private void ToggleLever(int i, blockData block)
    {
        block.visualActive = (block.visualActive + 1) % 2;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 2, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else
        {
            EditBlockActiveSide(i, 0, 0);
            EditBlockActiveSide(i, 1, 0);
            EditBlockActiveSide(i, 2, 0);
            EditBlockActiveSide(i, 3, 0);
        }
    }

    private void SetLever(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 2, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else
        {
            EditBlockActiveSide(i, 0, 0);
            EditBlockActiveSide(i, 1, 0);
            EditBlockActiveSide(i, 2, 0);
            EditBlockActiveSide(i, 3, 0);
        }
    }

    private void SetButton(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 2, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else
        {
            EditBlockActiveSide(i, 0, 0);
            EditBlockActiveSide(i, 1, 0);
            EditBlockActiveSide(i, 2, 0);
            EditBlockActiveSide(i, 3, 0);
        }
    }

    private void SetPreassureplate(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 2, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else
        {
            EditBlockActiveSide(i, 0, 0);
            EditBlockActiveSide(i, 1, 0);
            EditBlockActiveSide(i, 2, 0);
            EditBlockActiveSide(i, 3, 0);
        }
    }
     
    private void SetVisualAvive(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;
    }

    private void HandleDoor(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 1))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 3))
        {
            EditVisualActive(i, 1);
        }
        EditVisualActive(i, 0);
    }

    private void HandleAndGate(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0) && IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 3, 1);
        } else EditVisualActive(i, 0);
    }

    private void HandleOrGate(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0) || IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else EditVisualActive(i, 0);
    }

    private void HandleXorGate(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0) ^ IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else EditVisualActive(i, 0);
    }
    private void HandleCondensator(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0) ^ IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 3, 1);
            EditBlockState(i, 300);
        }
        else
        {
            if (block.state > 0)
            {
                EditBlockState(i, block.state - 1 );
            }
            else
            {
                EditVisualActive(i, 0);
                EditBlockState(i, 0);
            }
                
        }
    }

    private void HandlePulse(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0) ^ IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditBlockState(i, block.state + 1);

            if (block.state < 10)
            {
                EditVisualActive(i, 1);

                EditBlockActiveSide(i, 1, 1);
                EditBlockActiveSide(i, 3, 1);
            }
            else
            {
                EditVisualActive(i, 0);
                EditBlockActiveSide(i, 1, 0);
                EditBlockActiveSide(i, 3, 0);
            }
        }
        else
        {
            EditVisualActive(i, 0);
            EditBlockState(i, 0);
            EditBlockActiveSide(i, 1, 0);
            EditBlockActiveSide(i, 3, 0);
        }
    }

    private void HandleToggle(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0) || IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            if(block.meta == "0")
            {
                block.meta = "1";
                block.state = (block.state + 1) % 2;

                if(block.state == 1)
                {
                    EditBlockActiveSide(i, 1, 1);
                    EditBlockActiveSide(i, 3, 1);
                }
                else
                {
                    EditBlockActiveSide(i, 1, 0);
                    EditBlockActiveSide(i, 3, 0);
                }
            }
        }
        else
        {
            block.meta = "0";
        }
    }

    private void HandleFlipFlop(int i, blockData block)
    {
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            block.state = 0;

            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 3, 0);
        }
        if (IsAnyConnectionActive(block.inputDirections, i, 0))
        {
            block.state = 1;

            EditBlockActiveSide(i, 1, 0);
            EditBlockActiveSide(i, 3, 1);
        }
    }

    private void HandleNotGate(int i, blockData block)
    {
        if (!IsAnyConnectionActive(block.inputDirections, i, 0) && !IsAnyConnectionActive(block.inputDirections, i, 2))
        {
            EditVisualActive(i, 1);
            EditBlockActiveSide(i, 1, 1);
            EditBlockActiveSide(i, 3, 1);
        }
        else EditVisualActive(i, 0);
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

    public void EditBlockState (int index, int edit)
    {
        blockData block = GetBlock(index);
        block.state = edit;
    }
    public void EditBlockMeta(int index, string edit)
    {
        blockData block = GetBlock(index);
        block.meta = edit;
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
        blockData thisBlock = new blockData();
        for (int i = 0; i < reader.blockSafeFile.Count; i++)
        {
            if (reader.blockSafeFile[i].index == index)
            {
                thisBlock = reader.blockSafeFile[i];
                return thisBlock;
            }
        }
        return thisBlock;
        
    }

    private int[] CheckConnectionSides(int[] connectionSides, List<connections> sources)
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


[System.Serializable]
public struct connections
{
    public int outputIndex;
    public int outputSide;
}

[System.Serializable]
public struct blockData
{
    public int index;
    public string type;
    public int direction;
    public int[] outputDirections;
    public int[] inputDirections;
    public int state;
    public string meta;
    public int visualActive;
    public List<List<connections>> connectionData;
    public bool[] activeSides;
    public bool editable;
    public bool clickkable;
}
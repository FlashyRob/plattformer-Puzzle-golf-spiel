using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Updates : MonoBehaviour
{
    private CheckWheatherTwoBlocksAreConnected position;
    private JSONReader reader;

    public bool updateLoop = false;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (updateLoop)
        {


            for (int i = 0; i < reader.blockSafeFile.Count; i++)
            {
                
                blockData block = reader.blockSafeFile[i];

                

                switch (block.type)
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

        
    }
    
    public void ResetConnections(int index, int side)
    {
        blockData thisBlock = new blockData();
        thisBlock = GetBlock(index);
        thisBlock.connectios_top.Clear();
        thisBlock.connectios_bottom.Clear();
        thisBlock.connectios_right.Clear();
        thisBlock.connectios_left.Clear();

        thisBlock.activeSides = new bool[4];
    }

    public void AddConnection(int blockIndex, int side, connections connection)
    {
        blockData thisBlock = new blockData();
        thisBlock = GetBlock(blockIndex);
        switch (side)
        {
            case 0:
                if (!thisBlock.connectios_top.Contains(connection))
                {
                    thisBlock.connectios_top.Add(connection);
                }
                break;
            case 1:
                if (!thisBlock.connectios_right.Contains(connection))
                {
                    thisBlock.connectios_right.Add(connection);
                }
                break;
            case 2:
                if (!thisBlock.connectios_bottom.Contains(connection))
                {
                    thisBlock.connectios_bottom.Add(connection);
                }
                break;
            case 3:
                if (!thisBlock.connectios_left.Contains(connection))
                {
                    thisBlock.connectios_left.Add(connection);
                }
                break;
        }
            
    }

    public List<connections> GetConnections(int blockIndex, int side)
    {
        blockData thisBlock = GetBlock(blockIndex);
        var l = new List<connections>();

        switch (side)
        {
            case 0:
                l = thisBlock.connectios_top;
                break;
            case 1:
                l = thisBlock.connectios_right;
                break;
            case 2:
                l = thisBlock.connectios_bottom;
                break;
            case 3:
                l = thisBlock.connectios_left;
                break;
        }
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
        if (IsAnyConnectionActive(block, 3))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 1))
        {
            SetVisualActive(i, 1);
            return;
        }
        SetVisualActive(i, 0);
    }

    private void HandleWireCorner(int i, blockData block)
    {

        if (IsAnyConnectionActive(block, 0))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 1))
        {
            SetVisualActive(i, 1);
            return;
        }
        SetVisualActive(i, 0);
    }

    private void HandleWireT(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 1))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 3))
        {
            SetVisualActive(i, 1);
            return;
        }
        SetVisualActive(i, 0);
    }

    private void HandelWireCross(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 1))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 3))
        {
            SetVisualActive(i, 1);
            return;
        }
        SetVisualActive(i, 0);
    }

    private void HandleLamp(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 1))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 3))
        {
            SetVisualActive(i, 1);
            return;
        }
        SetVisualActive(i, 0);
    }

    private void HandleBattery(int i)
    {
        SetVisualActive(i, 1);

        EditBlockActiveSide(i, 0, true);
        EditBlockActiveSide(i, 1, true);
        EditBlockActiveSide(i, 2, true);
        EditBlockActiveSide(i, 3, true);
    }

    private void ToggleLever(int i, blockData block)
    {
        block.visualActive = (block.visualActive + 1) % 2;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, true);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 2, true);
            EditBlockActiveSide(i, 3, true);
        }
        else
        {
            EditBlockActiveSide(i, 0, false);
            EditBlockActiveSide(i, 1, false);
            EditBlockActiveSide(i, 2, false);
            EditBlockActiveSide(i, 3, false);
        }
    }

    private void SetLever(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, true);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 2, true);
            EditBlockActiveSide(i, 3, true);
        }
        else
        {
            EditBlockActiveSide(i, 0, false);
            EditBlockActiveSide(i, 1, false);
            EditBlockActiveSide(i, 2, false);
            EditBlockActiveSide(i, 3, false);
        }
    }

    private void SetButton(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, true);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 2, true);
            EditBlockActiveSide(i, 3, true);
        }
        else
        {
            EditBlockActiveSide(i, 0, false);
            EditBlockActiveSide(i, 1, false);
            EditBlockActiveSide(i, 2, false);
            EditBlockActiveSide(i, 3, false);
        }
    }

    private void SetPreassureplate(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;

        if (block.visualActive == 1)
        {
            EditBlockActiveSide(i, 0, true);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 2, true);
            EditBlockActiveSide(i, 3, true);
        }
        else
        {
            EditBlockActiveSide(i, 0, false);
            EditBlockActiveSide(i, 1, false);
            EditBlockActiveSide(i, 2, false);
            EditBlockActiveSide(i, 3, false);
        }
    }
     
    private void SetVisualAvive(int i, blockData block, int newVal)
    {
        block.visualActive = newVal;
    }

    private void HandleDoor(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 1))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            return;
        }
        if (IsAnyConnectionActive(block, 3))
        {
            SetVisualActive(i, 1);
            return;
        }
        SetVisualActive(i, 0);
    }

    private void HandleAndGate(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0) && IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 3, true);
        } else SetVisualActive(i, 0);
    }

    private void HandleOrGate(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0) || IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 3, true);
        }
        else SetVisualActive(i, 0);
    }

    private void HandleXorGate(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0) ^ IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 3, true);
        }
        else SetVisualActive(i, 0);
    }
    private void HandleCondensator(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0) ^ IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 3, true);
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
                SetVisualActive(i, 0);
                EditBlockState(i, 0);
            }
                
        }
    }

    private void HandlePulse(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0) ^ IsAnyConnectionActive(block, 2))
        {
            EditBlockState(i, block.state + 1);

            if (block.state < 10)
            {
                SetVisualActive(i, 1);

                EditBlockActiveSide(i, 1, true);
                EditBlockActiveSide(i, 3, true);
            }
            else
            {
                SetVisualActive(i, 0);
                EditBlockActiveSide(i, 1, false);
                EditBlockActiveSide(i, 3, false);
            }
        }
        else
        {
            SetVisualActive(i, 0);
            EditBlockState(i, 0);
            EditBlockActiveSide(i, 1, false);
            EditBlockActiveSide(i, 3, false);
        }
    }

    private void HandleToggle(int i, blockData block)
    {
        if (IsAnyConnectionActive(block, 0) || IsAnyConnectionActive(block, 2))
        {
            if(block.meta == "0")
            {
                block.meta = "1";
                block.state = (block.state + 1) % 2;

                if(block.state == 1)
                {
                    EditBlockActiveSide(i, 1, true);
                    EditBlockActiveSide(i, 3, true);
                }
                else
                {
                    EditBlockActiveSide(i, 1, false);
                    EditBlockActiveSide(i, 3, false);
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
        if (IsAnyConnectionActive(block, 0))
        {
            block.state = 0;

            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 3, false);
        }
        if (IsAnyConnectionActive(block, 0))
        {
            block.state = 1;

            EditBlockActiveSide(i, 1, false);
            EditBlockActiveSide(i, 3, true);
        }
    }

    private void HandleNotGate(int i, blockData block)
    {
        if (!IsAnyConnectionActive(block, 0) && !IsAnyConnectionActive(block, 2))
        {
            SetVisualActive(i, 1);
            EditBlockActiveSide(i, 1, true);
            EditBlockActiveSide(i, 3, true);
        }
        else SetVisualActive(i, 0);
    }

    public bool IsAnyConnectionActive(blockData block, int side)
    {
        List <connections> inputConnections = new List<connections>();

        switch (side)
        {
            case 0:
                inputConnections = block.connectios_top;
                break;
            case 1:
                inputConnections = block.connectios_right;
                break;
            case 2:
                inputConnections = block.connectios_bottom;
                break;
            case 3:
                inputConnections = block.connectios_left;
                break;
        }

        foreach (connections connection in inputConnections)
        {
            if (isActive(connection.outputIndex, connection.outputSide))
            {
                return true;
            }
        }
        return false;

        /*
        activeConnections = CheckConnectionSides(inputConnections, GetConnections(blockIndex, side));
        if (activeConnections.Contains(1))
        {
            return true;
        }
        return false;
        */
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

    public void EditBlockActiveSide(int index, int side, bool edit)
    {
        blockData block = GetBlock(index);
        block.activeSides[side] = edit;
    }

    public void EditVisualActive(int index, int edit)
    {
        blockData block = GetBlock(index);
        block.visualActive = edit;

        //Debug.Log("black visual set to " + edit+ " "+ block.type);
        //Debug.Log("result: "+ GetBlock(index).visualActive);
    }
    
    public void SetVisualActive(int index, int edit)
    {
        for (int i = 0; i < reader.blockSafeFile.Count; i++)
        {
            if (reader.blockSafeFile[i].index == index)
            {
                var b = reader.blockSafeFile[i];
                b.visualActive = edit;
                reader.blockSafeFile[i] = b;

                //Debug.Log("block visual set to " + edit + " " + reader.blockSafeFile[i].type);
                //Debug.Log("result: " + reader.blockSafeFile[i].visualActive);
                return;
            }
        }
    }

    public blockData GetBlock(int index)
    {
        blockData thisBlock;

        for (int i = 0; i < reader.blockSafeFile.Count; i++)
        {
            if (reader.blockSafeFile[i].index == index)
            {
                thisBlock = reader.blockSafeFile[i];
                return thisBlock;
            }
        }
        return new blockData();
        
    }
     /*
    private int[] CheckConnectionSides(List <connections> inputConnections, List<connections> sources)
    {
        int[] activeConnectionSides = new int[4] {0,0,0,0};

        for (int side = 0; side < 4; side++)
        {
            if (inputConnections)
            {
                if (checkActive(sources))
                {
                    activeConnectionSides[side] = 1;
                }
            }
        }
        return (activeConnectionSides);
    }
     */
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
    public List<connections> connectios_top;
    public List<connections> connectios_bottom;
    public List<connections> connectios_left;
    public List<connections> connectios_right;
    public bool[] activeSides;
    public bool editable;
    public bool clickkable;
}
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScannerinoCrocodilo : MonoBehaviour
{
    public List<int> scannedBlockIndexes = new List<int>();
    public List<int> nextScanns = new List<int>();
    public List<findBlock> foundPowerssources = new List<findBlock>();
    public List<int> visitedBlocks = new List<int>();
    public List<findBlock> foundInputBlocks = new List<findBlock>();

    public string[] powerSources = new string[] { "and_gate", "lever", "button", "preassure_plate", "or_gate", "xor_gate", "flip_flop", "toggle" };

    private CheckWheatherTwoBlocksAreConnected general;
    private Updates update;
    private JSONReader reader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        update = FindAnyObjectByType<Updates>();
        reader = FindAnyObjectByType<JSONReader>();


        //for (int i = 0; i < general.worldX * general.worldY; i++)
        //{
        //    scannedBlockIndexes.Add(i);
        //}
        Invoke("Scanner", 2f);
        
    }



    public void ScannFromBlock(int index, int side)
    {

        nextScanns.Clear();
        foundPowerssources.Clear();
        visitedBlocks.Clear();


        switch (side)
        {
            case 0:
                nextScanns.Add(ScannNeighbours(index).top);
                break;
            case 1:
                nextScanns.Add(ScannNeighbours(index).right);
                break;
            case 2:
                nextScanns.Add(ScannNeighbours(index).bottom);
                break;
            case 3:
                nextScanns.Add(ScannNeighbours(index).left);
                break;
        }

        List<int> thisScann = new List<int>();

        int safetyBreak = 0;

        while (true)
        {
            thisScann = nextScanns;
            nextScanns.Clear();

            safetyBreak += 1;

            if (thisScann.Count == 0 || safetyBreak > 1000)
            {
                break;
            }

            for (int i = 0; i < thisScann.Count; i++)
            {
                int tileIndex = thisScann[i];
                tiles neightbours = ScannNeighbours(tileIndex);
                var neighbourDir = new int[] { neightbours.top, neightbours.left, neightbours.right, neightbours.bottom };

                visitedBlocks.Add(tileIndex);
                int scannDir = 0;
                foreach (var nIndex in neighbourDir)
                {
                    if (general.CheckIfTwoBlocksAreConnected(tileIndex, nIndex) && !(nIndex == index) && !visitedBlocks.Contains(nIndex))
                    {
                        if (StringContains(update.GetBlock(tileIndex).type, powerSources))
                        {
                            findBlock powerSource;

                            powerSource.index = nIndex;
                            powerSource.Side = scannDir;
                            powerSource.fromSide = side;
                            powerSource.fromIndex = index;

                            foundPowerssources.Add(powerSource);
                        }
                        else
                        {
                            if (update.GetBlock(tileIndex).type == "wire")
                            {
                                nextScanns.Add(nIndex);
                            }
                        }
                        scannDir++;
                    }
                }
            }
        }
    }


    public void FindInputBlocks()
    {
        foundInputBlocks.Clear();

        foreach (blockData block in reader.blockSafeFile)
        {
            if (block.inputDirections.Contains(1))
            {
                for (int side = 0; side < 4; side++)
                {
                    if (block.inputDirections[side] == 1)
                    {
                        findBlock foundBlock = new findBlock();
                        foundBlock.index = block.index;
                        foundBlock.Side = side;

                        foundInputBlocks.Add(foundBlock);
                    }

                }

            }
        }
    }


    public void Scanner()
    {
        FindInputBlocks();
        foreach (findBlock foundBlock in foundInputBlocks)
        {
            ScannFromBlock(foundBlock.index, foundBlock.Side);
            ApplyConnectionData(foundPowerssources);

        }
    }

    public void ApplyConnectionData(List<findBlock> foundConnections)
    {
        foreach (findBlock foundBlock in foundConnections)
        {
            Debug.Log(foundBlock.index + " " + foundBlock.Side + " " + update.GetBlock(foundBlock.index).type);
            update.AddConnection(foundBlock.index, foundBlock.Side, new connections { outputIndex = foundBlock.fromIndex, outputSide = foundBlock.fromSide});
        }
    }

    /*
    scannedBlockIndexes.Add(neightbours.myself);
                            if (StringContains(update.blockData[nDir].type, powerSources))
                            {

                            }
                            else
                            {
                                nextScanns.Add(nDir);
                            }
    */

public bool StringContains(string item, string[] array)
    {
        foreach (string arrayItem in array)
        {
            if (arrayItem == item)
            {
                return true;
            }
        }
        return false;
    }

    public tiles ScannNeighbours(int index)
    {
        tiles tile = new tiles();
        tile.myself = index;
        int x = (int)general.GetXY(index).x;
        int y = (int)general.GetXY(index).y;

        tile.right = (int)general.GetIndexFromXY(x + 1, y);
        tile.left = (int)general.GetIndexFromXY(x - 1, y);
        tile.top = (int)general.GetIndexFromXY(x, y + 1);
        tile.bottom = (int)general.GetIndexFromXY(x, y - 1);

        return (tile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public struct tiles
    {
        public int myself;
        public int top;
        public int right;
        public int bottom;
        public int left;
    }

    public struct findBlock
    {
        public int index;
        public int Side;
        public int fromIndex;
        public int fromSide;
    }
    
    
}

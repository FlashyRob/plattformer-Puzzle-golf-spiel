using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScannerinoCrocodilo : MonoBehaviour
{
    public List<int> scannedBlockIndexes;
    public List<int> nextScanns;
    public List<int> foundPowerssources;
    public List<int> visitedBlocks;

    public string[] powerSources = new string[] { "and_gate", "lever", "button", "preassure_plate", "or_gate", "xor_gate", "flip_flop", "toggle"};

    private CheckWheatherTwoBlocksAreConnected general;
    private Updates update;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        general = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        update = FindAnyObjectByType<Updates>();

        //for (int i = 0; i < general.worldX * general.worldY; i++)
        //{
        //    scannedBlockIndexes.Add(i);
        //}
    }

    public void ScannFromBlock(int index, int side)
    {
        string[] sides = new string[4] {"top", "right", "bootom", "left"};

        nextScanns.Clear();
        foundPowerssources.Clear();
        visitedBlocks.Clear();

        string sideName = sides[side];
        var n = ScannNeighbours(index);

        var sidePropertyField = typeof(tiles).GetField(sideName);
        int sidePropertyValue = (int)sidePropertyField.GetValue(n);
        nextScanns.Add(sidePropertyValue);

        List<int> thisScann = new List<int>();

        int safetyBreak = 0;

        while (true)
        {
            thisScann = nextScanns;
            nextScanns.Clear();

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

                 foreach(var nIndex in neighbourDir)
                 {
                    if (general.CheckIfTwoBlocksAreConnected(tileIndex, update.GetBlock(tileIndex).inputDirections, nIndex, update.GetBlock(nIndex).outputDirections) && !(nIndex == index) && !visitedBlocks.Contains(nIndex))
                    {
                        if (StringContains(update.GetBlock(tileIndex).type, powerSources))
                        {
                            foundPowerssources.Add(nIndex);
                        }
                        else
                        {
                            nextScanns.Add(nIndex);
                        }
                    }
                 }
             }
        }
    }

    public void Scanner()
    {

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

    
    
}

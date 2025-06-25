using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScannerinoCrocodilo : MonoBehaviour
{
    public List<int> scannedBlockIndexes;
    public List<int> nextScanns;

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

    public void Scanner(int index)
    {
        nextScanns.Clear();
        nextScanns.Add(index);

        for (int i = 0; i < nextScanns.Count; i++)
        {
            int tileIndex = nextScanns[i];
            tiles neightbours = ScannNeighbours(index);
            scannedBlockIndexes.Add(neightbours.myself);
            if (powerSources.Contains(neightbours.top))
            {
                nextScanns.Add(neightbours.top);
            }
            
            nextScanns.Add(neightbours.bottom);
            nextScanns.Add(neightbours.left);
            nextScanns.Add(neightbours.right);
        }


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

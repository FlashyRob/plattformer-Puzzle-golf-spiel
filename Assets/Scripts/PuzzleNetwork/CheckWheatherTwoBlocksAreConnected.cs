using System;
using UnityEngine;

public class CheckWheatherTwoBlocksAreConnected : MonoBehaviour
{
    private JSONReader reader;

    public int worldX = 100;
    public int worldY = 100;
    int Index = 789;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int testX = (int)GetXY(Index).x;
        int testY = (int)GetXY(Index).y;

        reader = FindAnyObjectByType<JSONReader>();
    }


    public xy GetXY(int index)
    {
        xy xy;
        xy.x = (index % worldX);
        decimal dX = (index/ worldY);
        xy.y = Math.Floor(dX);

        int x = (int)Math.Round(xy.x);
        int y = (int)Math.Round(xy.y);

        xy.x = (int)x;
        xy.y = (int)y;

        return (xy);
    }
    public int GetIndexFromXY(Vector3 position)
    {
        return GetIndexFromXY((int)position.x, (int)position.y);
    }
    public int GetIndexFromXY(int x, int y)
    {
        int index = x + y * worldX;
        return (index);
    }

    public bool CheckIfTwoBlocksAreNextToEachother(int index1, int index2)
    {
        bool NextToEachOther = false;

        if (index1 - 1 == index2 || index2 - 1 == index1 || index1 - worldX == index2 || index2 - worldX == index1)
        {
            NextToEachOther = true;
        }

        return (NextToEachOther);
    }

    public bool CheckIfTwoBlocksAreConnected(int Index1, int Index2) //Index1 = input; Index2 = output
    {
        int[] InputDirections = reader.GetInputDirectionsOfIndex(Index1);
        int[] OutputDirections = reader.GetOutputDirectionsOfIndex(Index2);

        bool connected;

        if (Index1 - 1 == Index2 && InputDirections[3] == 1 && OutputDirections[1] == 1) // Index1 genau ein Tile rechts von Index2
        {
            connected = true;
        }
        else if (Index1 + 1 == Index2 && InputDirections[1] == 1 && OutputDirections[3] == 1) // Index1 genau ein Tile links von Index2
        {
            connected = true;
        }
        else if (Index1 - worldX == Index2 && InputDirections[2] == 1 && OutputDirections[0] == 1) // Index1 genau ein tile �ber Index2
        {
            connected = true;
        }
        else if (Index2 - worldX == Index1 && InputDirections[0] == 1 && OutputDirections[2] == 1) // Index1 genau ein tile unter Index2
        {
            connected = true;
        }
        else
        {
            connected = false;
        }

        return (connected);

    }


    public int getDirectionFromXY(int x, int y) // Die m�sste man mal bauen
    {
        int index = GetIndexFromXY(x, y);

        int direction = 0;

        for (int i = 0; i < reader.blockSafeFile.Count; i++)
        {
            if (reader.blockSafeFile[i].index == index)
            {
                direction = reader.blockSafeFile[i].direction;
            }
        }
        return (direction);
            
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct xy
{
    public decimal x;
    public decimal y;
}

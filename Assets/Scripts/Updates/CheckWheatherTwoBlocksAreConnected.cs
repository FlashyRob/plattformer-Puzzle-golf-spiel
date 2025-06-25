using System;
using UnityEngine;

public class CheckWheatherTwoBlocksAreConnected : MonoBehaviour
{

    public int worldX = 100;
    public int worldY = 100;
    int Index = 789;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int testX = (int)GetXY(Index).x;
        int testY = (int)GetXY(Index).y;

        

        Debug.Log(testX);
        Debug.Log(testY);

        Debug.Log(GetIndexFromXY(testX, testY));
        
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

    public bool CheckIfTwoBlocksAreConnected(int Index1, int[] directions1, int Index2, int[] directions2) // directions In/Out-Put angeben je nach dem was man halt überprüfen will
    {
        bool connected;

        if (Index1 - 1 == Index2 && directions1[3] == 1 && directions2[1] == 1) // Index1 genau ein Tile rechts von Index2
        {
            connected = true;
        }
        else if (Index1 + 1 == Index2 && directions1[1] == 1 && directions2[3] == 1) // Index1 genau ein Tile links von Index2
        {
            connected = true;
        }
        else if (Index1 - worldX == Index2 && directions1[2] == 1 && directions2[0] == 1) // Index1 genau ein tile über Index2
        {
            connected = true;
        }
        else if (Index2 - worldX == Index1 && directions1[0] == 1 && directions2[2] == 1) // Index1 genau ein tile unter Index2
        {
            connected = true;
        }
        else
        {
            connected = false;
        }

        return (connected);
    }


    public int getDirectionFromXY(int x, int y) // Die müsste man mal bauen
    {
        int direction = 0;
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

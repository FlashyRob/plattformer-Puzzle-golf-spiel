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

    private string[] powerSources = new string[] { "and_gate", "lever", "button", "pressureplate", "or_gate", "xor_gate", "flip_flop", "battery", "toggle", "switch", "or_gate", "not_gate", "xor_gate", "cross", "condensator"};

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

        /*
        var levelGenator = FindAnyObjectByType<GenerateLevel>();
        if (!levelGenator.playMode)
        {
            Invoke("Scanner", 2f);
        }
        */
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

        Debug.Log("started a new Scan at index: "+ index);
        Debug.Log("since the side was " + side + ", we started at index: " + nextScanns[0]);

        List<int> thisScann = new List<int>();

        int safetyBreak = 0;

        Debug.Log("I will try to start the scan from here: " + index + " whilest actually starting here: " + nextScanns[0]);

        if (!general.CheckIfTwoBlocksAreConnected(index, nextScanns[0]))
        {
            Debug.Log("No connections, since the block is not even connected at the side you told me to scann :(");
            return;
        }


        if (IsPowerssource(update.GetBlock(nextScanns[0]).type))
        {
            findBlock powerSource;

            powerSource.index = nextScanns[0];
            powerSource.side = (side + 2) % 4;
            powerSource.fromSide = side;
            powerSource.fromIndex = index;

            Debug.Log("foud a powersource at index: " + powerSource.index + "at side: " + powerSource.side + "from: " + powerSource.fromIndex + "side:" + powerSource.fromSide + "I have ended the script for you =)");
            foundPowerssources.Add(powerSource);
            return;

        }

        while (true)
        {
            Debug.Log("today I will scann: " + nextScanns.Count + " blocks");

            thisScann.Clear();
            
            foreach (int i in nextScanns)
            {
                thisScann.Add(i);
            }

            nextScanns.Clear();

            

            safetyBreak += 1;

            Debug.Log("started a new Scan of the this blocks List (len = " + thisScann.Count + ")");

            if (thisScann.Count == 0 || safetyBreak > 1000)
            {
                Debug.Log("I have to break, since I have nothing left to scann :/");
                break;
            }

            for (int i = 0; i < thisScann.Count; i++)
            {
                int tileIndex = thisScann[i];
                tiles neightbours = ScannNeighbours(tileIndex);
                var neighbourDir = new int[] { neightbours.top, neightbours.right, neightbours.bottom, neightbours.left};

                visitedBlocks.Add(tileIndex);

                int scannDir = -1;

                foreach (var nIndex in neighbourDir)
                {
                    scannDir++; 

                    Debug.Log("now we are scanning the tile at " + nIndex + " from " + tileIndex);

                    if(nIndex > -1 && nIndex < general.worldX * general.worldY)
                    {
                        
                        bool connected = general.CheckIfTwoBlocksAreConnected(tileIndex, nIndex);
                        bool isnotUs = !(nIndex == index);
                        bool notYetVisited = !visitedBlocks.Contains(nIndex);
                        if (connected && isnotUs  && notYetVisited)
                        {
                            if (IsPowerssource(update.GetBlock(nIndex).type))
                            {
                                findBlock powerSource;

                                powerSource.index = nIndex;
                                powerSource.side = (scannDir + 2) % 4;
                                powerSource.fromSide = side;
                                powerSource.fromIndex = index;

                                Debug.Log("foud a powersource at index: " + powerSource.index + " at side: " + powerSource.side + " from: " + powerSource.fromIndex +" side: "+ powerSource.fromSide);
                                foundPowerssources.Add(powerSource);

                            }
                            else
                            {
                                if (update.GetBlock(nIndex).type.Contains("wire"))
                                {
                                    nextScanns.Add(nIndex);

                                    Debug.Log("Sadly I have not found a powerssource at index " + nIndex + " but it is a wire." + " The tile we found is named: " + update.GetBlock(nIndex).type);
                                }
                                else
                                {
                                    Debug.Log(nIndex + " does not contain a powerssource, nor a wire" + " The tile we found is named: " + update.GetBlock(nIndex).type);
                                }
                            }
                            


                        }else
                        {
                            Debug.Log("There is no connection between the blocks " + nIndex + " (nIndex) and " + tileIndex + " (this tile) or we have already tried this tile before");
                        }
                        
                    }else
                    {
                        Debug.Log("The index I tried to scann: " + nIndex + " was out of the map");
                    }
                    
                   
                    
                }
            }

            Debug.Log("scan round ended");
        }
        Debug.Log("scan is finished =)");
        Debug.Log("have fun with the powerssources I found:");
        for (int i = 0; i < foundPowerssources.Count; i++)
        {
            Debug.Log("powerssource at: index:" + foundPowerssources[i].index + "side: " + foundPowerssources[i].side);
        }
        Debug.Log("that's all I have for you :(");
    }

    public bool IsPowerssource(string type)
    {
        Debug.Log("Now we are checking if " + type + " is a powerssouce:");

        foreach (string checkType in powerSources)
        {
            if (checkType == type) 
            {
                Debug.Log("YES =)");
                return true;
            }
        }

        Debug.Log("NO! GRRRR");

        return false;
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
                        foundBlock.side = side;

                        foundInputBlocks.Add(foundBlock);
                    }

                }

            }
        }
    }


    public void Scanner()
    {
        FindInputBlocks();

        Debug.Log("#sides with input: " + foundInputBlocks.Count());

        foreach (findBlock foundBlock in foundInputBlocks)
        {
            update.ResetConnections(foundBlock.index, foundBlock.side);
        }

        foreach (findBlock foundBlock in foundInputBlocks)
        {
            Debug.Log("found an input at idx: " + foundBlock.index +" at side: " + foundBlock.side);

            ScannFromBlock(foundBlock.index, foundBlock.side);

            

            ApplyConnectionData(foundPowerssources);

        }

        update.updateLoop = true;
    }

    public void ApplyConnectionData(List<findBlock> foundConnections)
    {
        foreach (findBlock foundBlock in foundConnections)
        {
            Debug.Log("appied connection to " + foundBlock.index + " " + foundBlock.side + " " + update.GetBlock(foundBlock.index).type);
            update.AddConnection(foundBlock.fromIndex, foundBlock.fromSide, new connections { outputIndex = foundBlock.index, outputSide = foundBlock.side});
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
        public int side;
        public int fromIndex;
        public int fromSide;
    }
    
    
}

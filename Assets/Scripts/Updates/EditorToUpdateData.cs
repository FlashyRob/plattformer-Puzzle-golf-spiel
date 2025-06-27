using System.Collections.Generic;
using UnityEngine;

public class EditorToUpdateData : MonoBehaviour
{
    private static EditorToUpdateData instance;
    private CheckWheatherTwoBlocksAreConnected position;
    private Updates update;
    public static EditorToUpdateData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<EditorToUpdateData>();
            }
            return instance;
        }


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        update = FindAnyObjectByType<Updates>();

        int[] inputDirections = new int[4] { 0, 0, 1, 1 }; // directions from editor e.g. {1,0,1,0} for "straight wire"
        // int direction = 2; // direction from editor e.g. 1 for rotated 90 degrees to the right

        // List<string> wireNames = new List<string>() { "wire_straight" , "wire_curve" , "wire_t" , "wire_cross" , "lever" };

        editorData loadedData = new editorData();
        loadedData.blockNames = new string[] { "And", "Or" };
        loadedData.directions = new int[] { 0, 1 };
        loadedData.state = new int[] { 0, 1 };


        blockData[] blocks = new blockData[2];
        for (int i = 0; i < blocks.Length; i++)
        {

            blockData b = new blockData();
            b.typetype = loadedData.blockNames[i];
            b.direction = loadedData.directions[i];
            b.state = loadedData.state[i];
            connectors inAndOutPutDirectionsAtI = BlockNamesToDirections(loadedData.blockNames[i]);
            b.inputDirections = directions1AndDirectionToDirection2(inAndOutPutDirectionsAtI.inputDirections, loadedData.directions[i]);
            b.outputDirections = directions1AndDirectionToDirection2(inAndOutPutDirectionsAtI.outputDirections, loadedData.directions[i]);
            b.meta = "0"; // evtl sp?ter relevant
            b.visualActive = 0;


            blocks[i] = b;
        }
    }
    public connectors BlockNamesToDirections(string blockName)
    {

        int[] directionsOutput = new int[4] { 0, 0, 0, 0 };
        int[] directionsInput = new int[4] { 0, 0, 0, 0 };

        if (blockName == "wire_straight")
        {
            directionsInput = new int[4] { 0, 1, 0, 1 };
            directionsOutput =  new int[4] { 0, 1, 0, 1 };
        }
        else if (blockName == "wire_curve")
        {
            directionsInput = new int[4] { 1, 1, 0, 0 };
            directionsOutput = new int[4] { 1, 1, 0, 0 };
        }
        else if (blockName == "wire_t")
        {
            directionsInput = new int[4] { 1, 1, 0, 1 };
            directionsOutput = new int[4] { 1, 1, 0, 1 };
        }
        else if (blockName == "wire_cross")
        {
            directionsInput = new int[4] { 1, 1, 1, 1 };
            directionsOutput = new int[4] { 1, 1, 1, 1 };
        }
        else if (blockName == "lever")
        {
            directionsOutput = new int[4] { 1, 1, 1, 1 };
        }
        else if (blockName == "and_gate")
        {
            directionsInput = new int[4] { 1, 0, 1, 0 };
            directionsOutput = new int[4] { 0, 1, 0, 1 };
        }
        else
        {
            directionsOutput = new int[4] { 0, 0, 0, 0 };
        }

        connectors inOut;
        inOut.outputDirections = directionsOutput;
        inOut.inputDirections = directionsInput;

        return (inOut);
    }

    public int[] directions1AndDirectionToDirection2(int[] directions1, int direction)
    {
        int[] outputDirections = new int[directions1.Length]; // directions for scan e.g. {0,1,0,1} for "straight wire rotated 90 degrees"

        for (int i = 0; i < directions1.Length; i++)
        {
            outputDirections[i] = directions1[(i + direction) % 4];
        }

        return (outputDirections);


    }

    public void addDataToBlockData(int x, int y, string blockName, int direction = 0, int state = 0)
    {
        blockData b = new blockData();
        
        int index = position.GetIndexFromXY(x, y);

        b.typetype = blockName;
        b.direction = direction;
        b.state = state;
        connectors inAndOutPutDirectionsAtI = BlockNamesToDirections(blockName);
        b.inputDirections = directions1AndDirectionToDirection2(inAndOutPutDirectionsAtI.inputDirections, direction);
        b.outputDirections = directions1AndDirectionToDirection2(inAndOutPutDirectionsAtI.outputDirections, direction);
        b.meta = "0"; // evtl sp?ter relevant
        b.visualActive = 0;
        Debug.Log(index + " " + x + " " + y);
        //update.blockData[index] = b;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public struct connectors
{
    public int[] inputDirections;
    public int[] outputDirections;
}

public struct editorData
{
    public string[] blockNames;
    public int[] directions;
    public int[] state;
}


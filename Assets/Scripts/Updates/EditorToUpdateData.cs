using System.Collections.Generic;
using UnityEngine;

public class EditorToUpdateData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        int[] inputDirections = new int[4] {0,0,1,1}; // directions from editor e.g. {1,0,1,0} for "straight wire"
        int direction = 2; // direction from editor e.g. 1 for rotated 90 degrees to the right

        /*
        int[] Test = directions1AndDirectionToDirection2(inputDirections, direction);
        for (int i = 0; i < Test.Length; i++)
        {
            Debug.Log(Test[i]);
        }
        */


        // List<string> wireNames = new List<string>() { "wire_straight" , "wire_curve" , "wire_t" , "wire_cross" , "lever" };

        connectors Test2 = BlockNamesToDirections("wire_straight");
        for (int i = 0; i < Test2.outputDirections.Length; i++)
        {
            Debug.Log(Test2.outputDirections[i]);
        }
        for (int i = 0; i < Test2.inputDirections.Length; i++)
        {
            Debug.Log(Test2.inputDirections[i]);
        }
    }
    public connectors BlockNamesToDirections(string blockName)
    { 
        
        int[] directionsOutput = new int[4] { 0,0,0,0};
        int[] directionsInput = new int[4] {0,0,0,0};

        if (blockName == "wire_straight")
        {
            directionsInput = new int[4] { 0, 1, 0, 1 };
        }
        else if (blockName == "wire_curve")
        {
            directionsInput = new int[4] { 1, 1, 0, 0 };
        }
        else if (blockName == "wire_t")
        {
            directionsInput = new int[4] { 1, 1, 0, 1 };
        }
        else if (blockName == "wire_cross")
        {
            directionsInput = new int[4] { 1, 1, 1, 1 };
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
        int[] outputDirections = new int[directions1.Length] ; // directions for scan e.g. {0,1,0,1} for "straight wire rotated 90 degrees"

        for (int i = 0; i < directions1.Length; i++)
        {
            outputDirections[i] = directions1[(i + direction) % 4];
        }

        return (outputDirections);
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }



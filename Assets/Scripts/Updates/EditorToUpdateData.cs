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
        
    }
    int[] directions1AndDirectionToDirection2(int[] directions1, int direction)
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
}

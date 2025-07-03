using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScannerinoCrocodilo))]
public class ScannerinoEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ScannerinoCrocodilo scanner = (ScannerinoCrocodilo)target;

        if (GUILayout.Button("Scan"))
        {
            scanner.Scanner();
        }
    }
}

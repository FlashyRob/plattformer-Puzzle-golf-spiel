using UnityEngine;

public class AddScripts : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<GenerateLevel>();
        gameObject.AddComponent<JSONReader>();
        gameObject.AddComponent<LevelEditor>();
        gameObject.AddComponent<CheckWheatherTwoBlocksAreConnected>();
        gameObject.AddComponent<EditorToUpdateData>();
        gameObject.AddComponent<ScannerinoCrocodilo>();
        gameObject.AddComponent<Updates>();
    }
}

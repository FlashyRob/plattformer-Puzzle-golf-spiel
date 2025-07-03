using UnityEngine;

public class RemoveInPlayMode : MonoBehaviour
{
    void Start()
    {
        var generateLevel = FindAnyObjectByType<GenerateLevel>();
        if (generateLevel.playMode)
        {
            Destroy(gameObject);
        }
    }
}

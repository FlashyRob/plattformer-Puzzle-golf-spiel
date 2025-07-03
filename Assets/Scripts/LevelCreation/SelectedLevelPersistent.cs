using UnityEngine;

public class SelectedLevelPersistent : MonoBehaviour
{
    private static SelectedLevelPersistent instance;
    public static SelectedLevelPersistent Instance
    {
        get
        {
            if (instance == null)
            {
                // ensure there is only one instance
                instance = FindAnyObjectByType<SelectedLevelPersistent>();
                if (instance == null)
                {
                    instance = new GameObject("SelectedLevel").AddComponent<SelectedLevelPersistent>();
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }

    public string level="";
}

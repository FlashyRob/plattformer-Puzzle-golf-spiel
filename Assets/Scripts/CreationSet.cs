using UnityEngine;

public class CreationSet : MonoBehaviour
{
    public string[] boxNames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<Editor>().SetMaterial(boxNames);
    }
}

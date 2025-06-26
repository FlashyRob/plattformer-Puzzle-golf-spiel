using UnityEngine;

public class CreationSet : MonoBehaviour
{
    public string[] boxNames = new string[] {"wire_straight", "Grass"};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FindAnyObjectByType<Editor>().SetMaterial(boxNames);
    }
}

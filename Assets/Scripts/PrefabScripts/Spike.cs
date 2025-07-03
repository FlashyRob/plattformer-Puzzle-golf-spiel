using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    private GenerateLevel generate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generate = FindAnyObjectByType<GenerateLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (generate.playMode == true)
        {
            SceneManager.LoadScene("PlayMode");
        }
    }
}

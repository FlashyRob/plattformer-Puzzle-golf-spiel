using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var generateLevel = FindAnyObjectByType<GenerateLevel>();
        if (generateLevel.playMode)
        {
            var player = FindAnyObjectByType<Schmoovement>();
            player.gameObject.SetActive(false);
            player.transform.position = transform.position + new Vector3(0,0.5f, 0);
            player.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

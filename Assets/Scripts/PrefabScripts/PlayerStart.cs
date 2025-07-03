using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    private static PlayerStart instance;

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
            Destroy(gameObject);
        }
        else
        {
            // make sure we are the only player start
            if(instance == null)
            {
                // we are the only instance :)
                instance = this;
            }
            else
            {
                // we are not the only instace :(
                Debug.Log("Destroy previous instance");
                // we have some killing to do :)
                Destroy(instance.gameObject);
                var reader = FindAnyObjectByType<JSONReader>();
                var position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
                if (reader)
                {
                    reader.RemoveBlock(position.GetIndexFromXY(instance.transform.position));
                }
                instance = this;
            }
        }
    }
}

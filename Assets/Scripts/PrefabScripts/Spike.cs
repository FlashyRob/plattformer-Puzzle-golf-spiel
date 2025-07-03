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
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Animator>().SetBool("isDead", true);
            Invoke("Die", 0.08f);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (generate.playMode == true)
        {
            collision.gameObject.GetComponent<Animator>().SetBool("isDead", false);
            
        }
    }

    public void Die()
    {
        SceneManager.LoadScene("PlayMode");
    }
}

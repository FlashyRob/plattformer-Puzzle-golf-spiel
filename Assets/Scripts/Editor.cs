using UnityEngine;

public class Editor : MonoBehaviour
{
    public GameObject[] grass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(grass[0], new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(grass[0], new Vector3(1, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}

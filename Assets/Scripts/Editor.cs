using UnityEngine;

public class Editor : MonoBehaviour
{
    public GameObject[] grass;

    public Vector3 mousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(grass[0], new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(
                Mathf.Round(mousePos.x),
                Mathf.Round(mousePos.y),
                0
            );
            Debug.Log(mousePos);
            Instantiate(grass[0], mousePos, Quaternion.identity);
        }
    }
}

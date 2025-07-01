using UnityEngine;
using TMPro;

public class ChangeBlockCount : MonoBehaviour
{
    void count(int count)
    {
        GetComponent<TextMeshProUGUI>().text = count.ToString();
    }
}

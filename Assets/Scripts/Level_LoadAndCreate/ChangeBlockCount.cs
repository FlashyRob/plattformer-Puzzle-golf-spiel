using UnityEngine;
using TMPro;

public class ChangeBlockCount : MonoBehaviour
{
    public void update(int count)
    {
        GetComponent<TextMeshProUGUI>().text = count.ToString();
    }
}

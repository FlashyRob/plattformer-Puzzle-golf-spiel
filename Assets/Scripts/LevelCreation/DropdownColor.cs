using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropdownColor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var rect = GetComponent<RectTransform>();
        TMP_Text mytext = rect.GetComponentInChildren<TMP_Text>(true);

        Image backgroundimage = rect.GetComponentInChildren<Image>(true);


        string levelName = mytext.text;
        var reader = FindAnyObjectByType<JSONReader>();
        if (reader.IsDeveloperLevel(levelName))
        {
            backgroundimage.color = new Color (0.8662781f, 0.8666667f, 0.9333334f);
        }

    }

}

using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerClickHandler
{
    public static string selectedMaterial = "Nothing";
    public static bool changed;
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        string[] materials = new string[] {"grass", "dirt", "water"};
        selectedMaterial = name;
        changed = true;
    }
} 
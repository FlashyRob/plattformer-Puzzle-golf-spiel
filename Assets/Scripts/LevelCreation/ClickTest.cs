using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerClickHandler
{
    public static string selectedMaterial = "nothing";
    public static bool changed;
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        selectedMaterial = name;
        changed = true;
    }
} 
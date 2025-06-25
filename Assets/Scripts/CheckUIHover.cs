using UnityEngine;
using UnityEngine.EventSystems;

public class CheckUIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool hoverUI = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverUI = true;
    }



    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hoverUI = false;
    }
}

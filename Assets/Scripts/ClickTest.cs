using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerClickHandler
{
    public string selectedMaterial;
    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {

        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!");
        selectedMaterial = name;
    }
} 
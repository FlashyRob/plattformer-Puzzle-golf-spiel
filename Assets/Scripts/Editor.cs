using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    private GameObject[] block;
    public GameObject hud;
    public string editorMode = "place";
    public string[] materials = new string[] {"Grass", "Dirt", "Water"};
    public Vector3 mousePos;
    List<string> blockName = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        block = Resources.LoadAll<GameObject>("Blocks");
        for (int i = 0; i < block.Length; i++)
        {
            blockName.Add(block[i].name);
        }
        Initialize();
    }

    private void Initialize()
    {
        RectTransform rt;
        HorizontalLayoutGroup lg;
        Image im;

        var editorParent = new GameObject();
        editorParent.name = "EditorParent";
        editorParent.transform.parent = hud.transform;
        editorParent.layer = 5;
        rt = editorParent.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 80);
        rt.pivot = new Vector2(0.5f, 0);
        im = editorParent.AddComponent<Image>();
        im.color = new Color(164f / 256f, 164f / 256f, 164f / 256f);
        editorParent.AddComponent<CheckUIHover>();

        var blockSelectorParent = new GameObject();
        blockSelectorParent.name = "BlockSelectorParent";
        blockSelectorParent.transform.parent = editorParent.transform;
        rt = blockSelectorParent.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.localPosition = new Vector2(-200, 40);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 0);
        rt.pivot = new Vector2(0, 0.5f);
        lg = blockSelectorParent.AddComponent<HorizontalLayoutGroup>();
        lg.padding = new RectOffset(20, 0, 0, 0);
        lg.spacing = 10;
        lg.childAlignment = TextAnchor.MiddleLeft;
        lg.childControlWidth = false;
        lg.childControlHeight = false;
        lg.childForceExpandWidth = false;
        lg.childForceExpandHeight = false;

        for (int i = 0; i < materials.Length; i++)
        {
            var block = new GameObject();
            block.name = materials[i];
            block.transform.parent = blockSelectorParent.transform;
            rt = block.AddComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);
            rt.anchoredPosition = new Vector2(45, -40);
            rt.sizeDelta = new Vector2(50, 50);
            rt.pivot = new Vector2(0.5f, 0.5f);
            block.AddComponent<ClickTest>();
            im = block.AddComponent<Image>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (
            (
                (Input.GetKey(KeyCode.Mouse0) && editorMode == "drag") ||
                (Input.GetKeyDown(KeyCode.Mouse0) && editorMode == "place")
            ) && 
            !CheckUIHover.hoverUI
        )
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(
                Mathf.Round(mousePos.x),
                Mathf.Round(mousePos.y),
                0
            );
            string currentBlockName = "block:" + mousePos.x + "," + mousePos.y;
            GameObject currentBlockObject = GameObject.Find(currentBlockName);
            if (currentBlockObject != null) {
                currentBlockObject.GetComponent<RemoveBlock>().kill();
            }
            Debug.Log(blockName);
            Debug.Log(blockName.IndexOf(ClickTest.selectedMaterial));
            
            GameObject newBlock = Instantiate(block[blockName.IndexOf(ClickTest.selectedMaterial)], mousePos, Quaternion.identity);
            newBlock.name = currentBlockName;
            newBlock.AddComponent<RemoveBlock>();
        }
    }
}

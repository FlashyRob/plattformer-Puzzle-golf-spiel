using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Editor : MonoBehaviour
{
    private GameObject[] block;
    public GameObject hud;
    public string editorMode = "place";
    private string[] materials = new string[] {
        "Terrain (16x16) 1_46",
        "Terrain (16x16) 1_47",
        "Terrain (16x16) 1_48",
        "Terrain (16x16) 1_65",
        "Terrain (16x16) 1_67",
        "Terrain (16x16) 1_68",
        "Terrain (16x16) 1_69",
        "Terrain (16x16) 1_66"
    };
    public Vector3 mousePos;
    List<string> blockName = new List<string>();

    private Updates update;
    private EditorToUpdateData editorToUpdate;
    private CheckWheatherTwoBlocksAreConnected position;
    private JSONReader reader;



    private GameObject createdBlocks;
    void Awake()
    {
        update = FindAnyObjectByType<Updates>();
        editorToUpdate =  FindAnyObjectByType<EditorToUpdateData>();
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        reader = FindAnyObjectByType<JSONReader>();

        block = Resources.LoadAll<GameObject>("Blocks");
        for (int i = 0; i < block.Length; i++)
        {
            blockName.Add(block[i].name);
        }
        Initialize();
    }

    private void Initialize()
    {
        try
        {
            Destroy(hud.transform.GetChild(0).gameObject);
        } catch { }
        if (createdBlocks != null)
        {
            Destroy(createdBlocks);
        }
        createdBlocks = new GameObject("Created Blocks");

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
            var blockSelector = new GameObject();
            blockSelector.name = materials[i];
            blockSelector.transform.parent = blockSelectorParent.transform;
            rt = blockSelector.AddComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);
            rt.anchoredPosition = new Vector2(45, -40);
            rt.sizeDelta = new Vector2(50, 50);
            rt.pivot = new Vector2(0.5f, 0.5f);
            blockSelector.AddComponent<ClickTest>();
            im = blockSelector.AddComponent<Image>();
            var materialPrefabIdx = blockName.IndexOf(materials[i]);
            if (materialPrefabIdx == -1)
            {
                Debug.LogError("Could not find blockName index for for material " + materials[i], blockSelector);
                continue;
            }
            var oc = block[materialPrefabIdx].GetComponent<SpriteRenderer>();
            im.sprite = oc.sprite;
            im.color = oc.color;
        }
    }

    Vector3 GetMousePos()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(
            Mathf.Round(mousePos.x),
            Mathf.Round(mousePos.y),
            0
        );
        return mousePos;
    }
    bool CheckValid(Vector3 mousePos)
    {
        return mousePos.x < 0 || mousePos.y < 0 ? false : true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Initialize();
        }

        if (
            (
                (Input.GetKey(KeyCode.Mouse0) && editorMode == "drag") ||
                (Input.GetKeyDown(KeyCode.Mouse0) && editorMode == "place")
            ) && 
            !CheckUIHover.hoverUI
        )
        {
            mousePos = GetMousePos();
            if (!CheckValid(mousePos)) { return; }

            string currentBlockName = "block:" + mousePos.x + "," + mousePos.y;
            if (ClickTest.selectedMaterial == "Nothing")
            {
                return;
            }
            GameObject currentBlockPrefab = block[blockName.IndexOf(ClickTest.selectedMaterial)];
            GameObject currentBlockObject = GameObject.Find(currentBlockName);

            if (currentBlockObject != null) {
                currentBlockObject.GetComponent<RemoveBlock>().kill();
            }
                        
            GameObject newBlock = Instantiate(currentBlockPrefab, mousePos, Quaternion.identity, createdBlocks.transform);
            newBlock.name = currentBlockName;
            newBlock.AddComponent<RemoveBlock>();

            var spriteRenderer = newBlock.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1; // show on top of other elements

            editorToUpdate.addDataToBlockData((int) mousePos.x, (int) mousePos.y, currentBlockPrefab.name);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && editorMode == "delete" && !CheckUIHover.hoverUI)
        {
            mousePos = GetMousePos();
            if (!CheckValid(mousePos)) { return; }

            string currentBlockName = "block:" + mousePos.x + "," + mousePos.y;
            GameObject currentBlockObject = GameObject.Find(currentBlockName);
            if (currentBlockObject != null) {
                currentBlockObject.GetComponent<RemoveBlock>().kill();
            }
            
            reader.RemoveBlock(position.GetIndexFromXY((int) mousePos.x, (int) mousePos.y));
        }
    }

    public void SetMaterial(string[] newMaterials)
    {
        materials = newMaterials;
        Initialize();
    }

    public blockData GetBlockAt(int x, int y)
    {
        blockData blockData = new blockData();
        blockData = (update.GetBlock(position.GetIndexFromXY(x, y)));
        return blockData;
    }
}

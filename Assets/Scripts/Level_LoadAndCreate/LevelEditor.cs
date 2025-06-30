using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Creates UI elements and reacts to mous-clicks to create level blocks.
/// </summary>
public class LevelEditor : MonoBehaviour
{
    private GameObject[] block;
    private GameObject[] textures;
    public GameObject hud;
    public string editorMode = "place";
    public Vector3 mousePos;
    public Vector3 mousePosOld;
    private string[] materials = new string[] {
        "Terrain (16x16) 1_46",
        "Terrain (16x16) 1_47",
        "Terrain (16x16) 1_48",
        "Terrain (16x16) 1_65",
        "Terrain (16x16) 1_67",
        "Terrain (16x16) 1_68",
        "Terrain (16x16) 1_69",
        "Terrain (16x16) 1_66",
        "wire_curve",
        "wire_straight",
        "wire_t",
        "lamp",
        "battery",
    }; 
    private int[] materialRotations;
    private GameObject[] materialObjects;

    List<string> blockName = new List<string>();
    List<string> texturesName = new List<string>();

    private Updates update;
    private CheckWheatherTwoBlocksAreConnected position;
    private JSONReader reader;
    private EditorToUpdateData editorToUpdate;

    private GameObject createdBlocks;
    private GameObject select;
    private GameObject currentBlockObject;
    private GameObject currentBlockPrefab;

    public void StartEditor()
    {
        materialRotations = new int[materials.Length];
        for (int i = 0; i < materialRotations.Length; i++)
            materialRotations[i] = 0;
        materialObjects = new GameObject[materials.Length];


        update = FindAnyObjectByType<Updates>();
        position = FindAnyObjectByType<CheckWheatherTwoBlocksAreConnected>();
        reader = FindAnyObjectByType<JSONReader>();
        editorToUpdate = FindAnyObjectByType<EditorToUpdateData>();


        block = Resources.LoadAll<GameObject>("Blocks");
        textures = Resources.LoadAll<GameObject>("Textures");

        for (int i = 0; i < block.Length; i++)
        {
            blockName.Add(block[i].name);
        }

        for (int i = 0; i < textures.Length; i++)
        {
            texturesName.Add(textures[i].name);
        }

        select = Instantiate(textures[texturesName.IndexOf("none")], mousePos, Quaternion.identity);
        select.name = "select";

        Initialize();
    }


    private void Initialize()
    {
        if (!hud)
        {
            hud = FindAnyObjectByType<Canvas>().gameObject;
        }
        try
        {
            Destroy(hud.transform.GetChild(0).gameObject);
        } catch { }
        createdBlocks = GameObject.Find("CreatedBlocks");

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
            GameObject blockSelector = new GameObject();
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

            materialObjects[i] = blockSelector;
        }

        /*
        var optionsParent = new GameObject();
        optionsParent.name = "OptionsParent";
        optionsParent.transform.parent = editorParent.transform;
        rt = optionsParent.AddComponent<RectTransform>();
        rt.localScale = new Vector3(0, 0, 0);*/

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
        mousePosOld = mousePos;
        mousePos = GetMousePos();
        if (!CheckValid(mousePos)) return;
        if (ClickTest.selectedMaterial == "Nothing") return;
        if (CheckUIHover.hoverUI) return;

        string currentBlockName = "block:" + mousePos.x + "," + mousePos.y;

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentBlockObject = GameObject.Find(currentBlockName);
            if (currentBlockObject != null)
            {
                blockData getBlock = GetBlockAt((int)mousePos.x, (int)mousePos.y);
                currentBlockObject.transform.Rotate(new Vector3(0, 0, -90));

                getBlock.inputDirections = editorToUpdate.BlockNamesToDirections(getBlock.type).inputDirections;
                getBlock.inputDirections = editorToUpdate.directions1AndDirectionToDirection2(getBlock.inputDirections, getBlock.direction);
                getBlock.outputDirections = editorToUpdate.BlockNamesToDirections(getBlock.type).outputDirections;
                getBlock.outputDirections = editorToUpdate.directions1AndDirectionToDirection2(getBlock.outputDirections, getBlock.direction);
                reader.EditBlockDirection(getBlock, (getBlock.direction + 1) % 4);

                int materialIndex = System.Array.IndexOf(materials, ClickTest.selectedMaterial);
                materialRotations[materialIndex] = (getBlock.direction + 1) % 4;
                select.transform.Rotate(new Vector3(0, 0, -90));
                materialObjects[materialIndex].transform.Rotate(new Vector3(0, 0, -90));
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (reader.BlockExists(position.GetIndexFromXY((int)mousePos.x, (int)mousePos.y)))
            {
                editorMode = "delete";
            }
            else
            {
                editorMode = "place";
            }
        }

        bool mousePosChanged = !(mousePos == mousePosOld);
        if (mousePosChanged)
        {
            select.transform.position = mousePos;
        }

        if (ClickTest.changed)
        {
            ClickTest.changed = false;
            currentBlockPrefab = block[blockName.IndexOf(ClickTest.selectedMaterial)];
            SpriteRenderer selectSprite = select.GetComponent<SpriteRenderer>();
            SpriteRenderer prefabSprite = currentBlockPrefab.GetComponent<SpriteRenderer>();
            selectSprite.sprite = prefabSprite.sprite;
            selectSprite.color = prefabSprite.color - new Color(0, 0, 0, 0.5f);
        }

        if (
            ((Input.GetKey(KeyCode.Mouse0) && mousePosChanged) ||
            Input.GetKeyDown(KeyCode.Mouse0)) &&
            editorMode == "place"
        )
        {
            int posIndex = position.GetIndexFromXY((int) mousePos.x, (int)mousePos.y);
            blockData previousBlock = update.GetBlock(posIndex);
            currentBlockObject = GameObject.Find(currentBlockName);
            if (previousBlock.type == currentBlockName) return;
            if (currentBlockObject != null)
            {
                currentBlockObject.GetComponent<RemoveBlock>().kill();
                reader.RemoveBlock(posIndex);
            }

            GameObject newBlock = Instantiate(currentBlockPrefab, mousePos, Quaternion.identity, createdBlocks.transform);
            newBlock.name = currentBlockName;
            newBlock.AddComponent<RemoveBlock>();

            blockData ptBlock = new blockData();
            ptBlock.type = currentBlockPrefab.name;
            ptBlock.direction = 0;
            ptBlock.index = posIndex;
            ptBlock.inputDirections = editorToUpdate.BlockNamesToDirections(ptBlock.type).inputDirections;
            ptBlock.inputDirections = editorToUpdate.directions1AndDirectionToDirection2(ptBlock.inputDirections, (ptBlock.direction + 3) % 4);
            ptBlock.outputDirections = editorToUpdate.BlockNamesToDirections(ptBlock.type).outputDirections;
            ptBlock.outputDirections = editorToUpdate.directions1AndDirectionToDirection2(ptBlock.outputDirections, (ptBlock.direction + 3) % 4);
            reader.AddBlock(ptBlock);
        }
        else if (
            ((Input.GetKey(KeyCode.Mouse0) && mousePosChanged) ||
            Input.GetKeyDown(KeyCode.Mouse0)) &&
            editorMode == "delete"
        )
        {
            currentBlockObject = GameObject.Find(currentBlockName);
            if (currentBlockObject != null)
            {
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

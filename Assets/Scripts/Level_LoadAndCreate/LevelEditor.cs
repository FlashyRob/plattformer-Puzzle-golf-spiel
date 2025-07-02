using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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
        "Terrain (16x16) 1_68",
        "Terrain (16x16) 1_66",
        "door",
        "finish",
        "trapdoor",
        "wire_curve",
        "wire_straight",
        "wire_t",
        "wire_cross",
        "lamp",
        "battery",
        "switch",
        "and_gate",
        "or_gate",
        "not_gate",
        "xor_gate",
        "lever",
        "cross",
        "condensator"
    }; 
    private int[] materialRotations;
    private GameObject[] materialObjects;
    private int[] materialCounts = new int[] {
        5,
        3,
        0,
        1,
        8,
        8,
        8,
        4,
        4,
        100,
    };
    private ChangeBlockCount[] materialCountObjects;

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

    public blockData hoverBlock;

    public void StartEditor()
    {
        materialRotations = new int[materials.Length];
        for (int i = 0; i < materialRotations.Length; i++)
            materialRotations[i] = 0;
        materialObjects = new GameObject[materials.Length];
        materialCountObjects = new ChangeBlockCount[materials.Length];


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
            Destroy(hud.transform.Find("EditorParent"));
        } catch { }
        createdBlocks = GameObject.Find("CreatedBlocks");

        RectTransform rt;
        HorizontalLayoutGroup lg;
        Image im;
        TMPro.TextMeshProUGUI tm;

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

        GameObject blockSelectorParent = new GameObject();
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
            var oc = block[materialPrefabIdx].GetComponentInChildren<SpriteRenderer>();
            im.sprite = oc.sprite;
            im.color = oc.color;

            materialObjects[i] = blockSelector;
        }

        if (GenerateLevel.creative) return;

        GameObject blockCountParent = new GameObject();
        blockCountParent.name = "BlockCountParent";
        blockCountParent.transform.parent = editorParent.transform;
        rt = blockCountParent.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(23, -50);
        rt.pivot = new Vector2(0, 0.5f);
        lg = blockCountParent.AddComponent<HorizontalLayoutGroup>();
        lg.padding = new RectOffset(22, 0, 5, 0);
        lg.childAlignment = TextAnchor.MiddleLeft;
        lg.spacing = 35;
        lg.childControlWidth = false;
        lg.childControlHeight = false;
        lg.childForceExpandWidth = false;
        lg.childForceExpandHeight = false;

        for (int i = 0; i < materials.Length; i++) { 
            GameObject blockCount = new GameObject();
            blockCount.name = materials[i];
            blockCount.transform.parent = blockCountParent.transform;
            rt = blockCount.AddComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);
            rt.sizeDelta = new Vector2(25, 25);
            rt.pivot = new Vector2(0.5f, 0.5f);
            im = blockCount.AddComponent<Image>();
            im.color = new Color(1, 1, 1, 0.5f);

            GameObject textItem = new GameObject();
            textItem.name = "Text";
            textItem.transform.parent = blockCount.transform;
            rt = textItem.AddComponent<RectTransform>();
            rt.localPosition = new Vector3(0, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(1, 1);
            rt.anchoredPosition = new Vector2(0, 0);
            rt.sizeDelta = new Vector2(0, 0);
            rt.pivot = new Vector2(0.5f, 0.5f);
            tm = textItem.AddComponent<TMPro.TextMeshProUGUI>();
            tm.text = materialCounts[i].ToString();
            tm.color = new Color(0, 0, 0, 1);
            tm.fontSize = 22;
            tm.horizontalAlignment = HorizontalAlignmentOptions.Right;

            materialCountObjects[i] = textItem.AddComponent<ChangeBlockCount>();
        }
    }

    bool CheckValid(Vector3 mousePos)
    {
        return mousePos.x < 0 || mousePos.y < 0 ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosOld = mousePos;
        mousePos = GenerateLevel.mousePos;

        hoverBlock = GetBlockAt((int)mousePos.x, (int)mousePos.y);

        if (!CheckValid(mousePos)) return;
        if (CheckUIHover.hoverUI) return;
        hoverBlock = GetBlockAt((int) mousePos.x, (int) mousePos.y);

        string currentBlockName = "block:" + mousePos.x + "," + mousePos.y;

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentBlockObject = GameObject.Find(currentBlockName);
            if (currentBlockObject != null)
            {
                update.updateLoop = false;

                blockData getBlock = hoverBlock;
                if (GenerateLevel.creative == true || hoverBlock.editable == true)
                    currentBlockObject.transform.Rotate(new Vector3(0, 0, -90));

                    getBlock.inputDirections = editorToUpdate.BlockNamesToDirections(getBlock.type).inputDirections;
                    getBlock.inputDirections = editorToUpdate.directions1AndDirectionToDirection2(getBlock.inputDirections, getBlock.direction);
                    getBlock.outputDirections = editorToUpdate.BlockNamesToDirections(getBlock.type).outputDirections;
                    getBlock.outputDirections = editorToUpdate.directions1AndDirectionToDirection2(getBlock.outputDirections, getBlock.direction);
                    reader.EditBlockDirection(getBlock, (getBlock.direction + 1) % 4);

                    int materialIndex = System.Array.IndexOf(materials, getBlock.type);
                    materialRotations[materialIndex] = getBlock.direction;
                    materialObjects[materialIndex].transform.rotation = currentBlockObject.transform.rotation;
                    if (getBlock.type == ClickTest.selectedMaterial)
                    {
                        select.transform.rotation = currentBlockObject.transform.rotation;
                    }
            }
            else if (ClickTest.selectedMaterial != "Nothing")
            {
                int materialIndex = System.Array.IndexOf(materials, ClickTest.selectedMaterial);
                materialRotations[materialIndex] += 1;
                select.transform.rotation = Quaternion.Euler(0, 0, materialRotations[materialIndex] * -90);
                materialObjects[materialIndex].transform.rotation = Quaternion.Euler(0, 0, materialRotations[materialIndex] * -90);
            }
        }

        if (ClickTest.selectedMaterial == "Nothing") return;
        bool blockExists = reader.BlockExists(position.GetIndexFromXY((int)mousePos.x, (int)mousePos.y));

        if (Input.GetMouseButtonDown(0))
        {
            if (blockExists)
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

            if (blockExists)
            {
                select.SetActive(false);
            } else
            {
                select.SetActive(true);
            }
        }

        if (ClickTest.changed)
        {
            ClickTest.changed = false;
            currentBlockPrefab = block[blockName.IndexOf(ClickTest.selectedMaterial)];
            SpriteRenderer selectSprite = select.GetComponent<SpriteRenderer>();
            SpriteRenderer prefabSprite = currentBlockPrefab.GetComponentInChildren<SpriteRenderer>();
            selectSprite.sprite = prefabSprite.sprite;
            selectSprite.color = prefabSprite.color - new Color(0, 0, 0, 0.5f);
            select.transform.rotation = Quaternion.Euler(0, 0, materialRotations[System.Array.IndexOf(materials, ClickTest.selectedMaterial)] * -90);
        }

        if (
            ((Input.GetKey(KeyCode.Mouse0) && mousePosChanged) ||
            Input.GetKeyDown(KeyCode.Mouse0)) &&
            editorMode == "place"
        )
        {
            int currentIndex = System.Array.IndexOf(materials, ClickTest.selectedMaterial);
            int posIndex = position.GetIndexFromXY((int)mousePos.x, (int)mousePos.y);
            blockData previousBlock = update.GetBlock(posIndex);
            currentBlockObject = GameObject.Find(currentBlockName);
            if (previousBlock.type == currentBlockName) return;
            if (!GenerateLevel.creative)
            {
                if (materialCounts[currentIndex] <= 0 || !update.GetBlock(currentIndex).editable) return;
                materialCounts[currentIndex] -= 1;
                materialCountObjects[currentIndex].update(materialCounts[currentIndex]);
            }
            if (currentBlockObject != null)
            {
                if (!GenerateLevel.creative)
                {
                    blockData getBlock = hoverBlock;
                    int oldIndex = System.Array.IndexOf(materials, getBlock.type);
                    materialCounts[oldIndex] += 1;
                    materialCountObjects[oldIndex].update(materialCounts[oldIndex]);
                }
                currentBlockObject.GetComponent<RemoveBlock>().kill();
                reader.RemoveBlock(posIndex);
            }

            GameObject newBlock = Instantiate(
                currentBlockPrefab,
                mousePos,
                Quaternion.Euler(0, 0, materialRotations[currentIndex] * -90), 
                createdBlocks.transform
            );
            newBlock.name = currentBlockName;
            newBlock.AddComponent<RemoveBlock>();
            UpdateCable updateCable = newBlock.GetComponent<UpdateCable>();
            if (updateCable != null) updateCable.index = posIndex;

            blockData ptBlock = new blockData();
            ptBlock.type = currentBlockPrefab.name;
            ptBlock.direction = materialRotations[System.Array.IndexOf(materials, currentBlockPrefab.name)];
            ptBlock.index = posIndex;
            ptBlock.inputDirections = editorToUpdate.BlockNamesToDirections(ptBlock.type).inputDirections;
            ptBlock.inputDirections = editorToUpdate.directions1AndDirectionToDirection2(ptBlock.inputDirections, (ptBlock.direction + 3) % 4);
            ptBlock.outputDirections = editorToUpdate.BlockNamesToDirections(ptBlock.type).outputDirections;
            ptBlock.outputDirections = editorToUpdate.directions1AndDirectionToDirection2(ptBlock.outputDirections, (ptBlock.direction + 3) % 4);
            ptBlock.activeSides = new bool[4];
            ptBlock.connectios_top = new List<connections>();
            ptBlock.connectios_bottom = new List<connections>();
            ptBlock.connectios_right = new List<connections>();
            ptBlock.connectios_left = new List<connections>();
            if (GenerateLevel.creative)  // F�r sp�tere Version einbauen, dass man im Editor mit einer Taste togglen kann das der block non editable wird. Diese m�ssten graphisch gehighlighted werde und k�nnten dann vom spieler abgebaut werden und in dessen inventar kommen
            {
                ptBlock.editable = false;
            }
            else
            {
                ptBlock.editable = true;
            }
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

                blockData getBlock = hoverBlock;
                int currentIndex = System.Array.IndexOf(materials, getBlock.type);

                if (!GenerateLevel.creative)
                {
                    if (currentIndex == -1) { Debug.LogError("Block abgebaut, der nicht im Inf ist"); return; };
                    if (!hoverBlock.editable) return;
                    materialCounts[currentIndex] += 1;
                    materialCountObjects[currentIndex].update(materialCounts[currentIndex]);
                }

                currentBlockObject.GetComponent<RemoveBlock>().kill();

                reader.RemoveBlock(position.GetIndexFromXY((int) mousePos.x, (int) mousePos.y));
 

            }
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
        blockData = update.GetBlock(position.GetIndexFromXY(x, y));
        return blockData;
    }
}

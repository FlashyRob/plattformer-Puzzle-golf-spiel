using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Creates UI elements and reacts to mous-clicks to create level blocks.
/// </summary>
public class LevelEditor : MonoBehaviour
{
    private GameObject[] block;
    private GameObject[] textures;
    private GameObject hud;
    public string editorMode = "place";
    public Vector3 mousePos;
    public Vector3 mousePosOld;
    private float screenWidthOld;

    private string[] materials = new string[] {
        "nothing",
        "PlayerStart",
        "finish",
        "Terrain (16x16) 1_46",
        "Terrain (16x16) 1_47",
        "Terrain (16x16) 1_68",
        "Terrain (16x16) 1_66",
        "Terrain (16x16) 1_56",
        "Terrain (16x16) 1_57",
        "Terrain (16x16) 1_58",
        "LargeSpike",
        "wire_curve",
        "wire_straight",
        "wire_t",
        "wire_cross",
        "lamp",
        "battery",
        "switch",
        "door",
        "trapdoor_left",
        "trapdoor_right",
        "and_gate",
        "or_gate",
        "not_gate",
        "xor_gate",
        "lever",
        "cross",
        "condensator",
        "button",
        "Terrain (16x16) 1_15",
        "Box",
        "MoveablePlatformHorizontal",
        "MoveablePlatformVertical",
        "dot",
        "Terrain (16x16) 1_60",
        "Terrain (16x16) 1_61",
        "Terrain (16x16) 1_62",
        "Terrain (16x16) 1_63",
        "Terrain (16x16) 1_64",
        "Terrain (16x16) 1_65",
        "Terrain (16x16) 1_66",

        "Terrain (16x16) 1_79",
        "Terrain (16x16) 1_80",
        "Terrain (16x16) 1_81",
        "Terrain (16x16) 1_82",
        "Terrain (16x16) 1_83",
        "Terrain (16x16) 1_93",
        "Terrain (16x16) 1_94",
        "Terrain (16x16) 1_95",

        "Terrain (16x16) 1_10",
        "Terrain (16x16) 1_11",
        "Terrain (16x16) 1_12",
        "Terrain (16x16) 1_13",

        "Terrain (16x16) 1_0",
        "Terrain (16x16) 1_1",
        "Terrain (16x16) 1_2",
        "Terrain (16x16) 1_3",
        "Terrain (16x16) 1_4",

        "Terrain (16x16) 1_27",
        "Terrain (16x16) 1_28",
        "Terrain (16x16) 1_29",
        "Terrain (16x16) 1_30",
        "Terrain (16x16) 1_40",
        "Terrain (16x16) 1_41",
        "Terrain (16x16) 1_42",

        "Terrain (16x16) 1_96",
        "Terrain (16x16) 1_97",
        "Terrain (16x16) 1_98",
        "Terrain (16x16) 1_99",
        "Terrain (16x16) 1_100",
        "Terrain (16x16) 1_114",
        "Terrain (16x16) 1_115",
        "Terrain (16x16) 1_116",
        "Terrain (16x16) 1_117",
        "Terrain (16x16) 1_118",
        "Terrain (16x16) 1_132",
        "Terrain (16x16) 1_133",
        "Terrain (16x16) 1_134",

        "left mouse",
        "right mouse",






    }; 
    private int[] materialRotations;
    private GameObject[] materialObjects;
    private int[] materialCounts = new int[] {
        10,
        10,
        10,
        10,
        8,
        8,
        8,
        4,
        4,
        10,
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

    GameObject content;
    GameObject blockSelectorParent;
    GameObject blockCountParent;

    public void StartEditor()
    {
        materialCounts = new int[materials.Length];

        for (int i = 0; i < materialCounts.Length; i++)
        {
            materialCounts[i] = 99;
        }

        materialRotations = new int[materials.Length];

        for (int i = 0; i < materialRotations.Length; i++)
        {
            materialRotations[i] = 0;
        }
            
        materialObjects = new GameObject[materials.Length];
        if (materialCounts.Length < materials.Length)
        {
            int[] oldCount = materialCounts;
            materialCounts = new int[materials.Length];
            for (int i = 0; i < oldCount.Length; i++)
            {
                materialCounts[i] = oldCount[i];
            }
        }
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

        screenWidthOld = Screen.width;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, blockSelectorParent.GetComponent<RectTransform>().sizeDelta.y - 70);
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
        GridLayoutGroup lg;
        Image im;
        TMPro.TextMeshProUGUI tm;
        ScrollRect sr;
        Mask mk;
        ContentSizeFitter sf;

        GameObject editorParent = new GameObject();
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
        sr = editorParent.AddComponent<ScrollRect>();
        sr.horizontal = false;
        sr.movementType = ScrollRect.MovementType.Clamped;

        GameObject viewPort = new GameObject();
        viewPort.name = "ViewPort";
        viewPort.transform.parent = editorParent.transform;
        rt = viewPort.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 0);
        rt.pivot = new Vector2(0, 0);
        im = viewPort.AddComponent<Image>();
        im.color = new Color(0, 0, 0, 1f / 256f);
        mk = viewPort.AddComponent<Mask>();

        sr.viewport = rt;

        content = new GameObject();
        content.name = "Content";
        content.transform.parent = viewPort.transform;
        rt = content.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 50);
        rt.pivot = new Vector2(0.5f, 1);

        sr.content = rt;

        blockSelectorParent = new GameObject();
        blockSelectorParent.name = "BlockSelectorParent";
        blockSelectorParent.transform.parent = content.transform;
        rt = blockSelectorParent.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 0);
        rt.pivot = new Vector2(0.5f, 1);
        lg = blockSelectorParent.AddComponent<GridLayoutGroup>();
        lg.padding = new RectOffset(10, 0, 10, 0);
        lg.childAlignment = TextAnchor.UpperLeft;
        lg.cellSize = new Vector2(40, 40);
        lg.spacing = new Vector2(10, 10);
        sf = blockSelectorParent.AddComponent<ContentSizeFitter>();
        sf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        sf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        for (int i = 0; i < materials.Length; i++)
        {
            GameObject blockSelector = new GameObject();
            blockSelector.name = materials[i];
            blockSelector.transform.parent = blockSelectorParent.transform;
            rt = blockSelector.AddComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);
            rt.anchoredPosition = new Vector2(45, -40);
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
            im.preserveAspect = true;

            materialObjects[i] = blockSelector;
        }

        blockCountParent = new GameObject();
        blockCountParent.name = "BlockCountParent";
        blockCountParent.transform.parent = content.transform;
        rt = blockCountParent.AddComponent<RectTransform>();
        rt.localScale = new Vector3(1, 1, 1);
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.anchoredPosition = new Vector2(0, 0);
        rt.sizeDelta = new Vector2(0, 0);
        rt.pivot = new Vector2(0.5f, 1);
        lg = blockCountParent.AddComponent<GridLayoutGroup>();
        lg.padding = new RectOffset(25, 0, 25, 0);
        lg.childAlignment = TextAnchor.UpperLeft;
        lg.cellSize = new Vector2(25, 25);
        lg.spacing = new Vector2(25, 25);
        sf = blockCountParent.AddComponent<ContentSizeFitter>();
        sf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        sf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        for (int i = 0; i < materialCounts.Length; i++) { 
            GameObject blockCount = new GameObject();
            blockCount.name = materials[i];
            blockCount.transform.parent = blockCountParent.transform;
            rt = blockCount.AddComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);
            rt.pivot = new Vector2(0.5f, 0.5f);
            im = blockCount.AddComponent<Image>();
            im.color = new Color(1, 1, 1, 0.5f);
            im.raycastTarget = false;

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
            tm.raycastTarget = false;

            materialCountObjects[i] = textItem.AddComponent<ChangeBlockCount>();
        }

        if (GenerateLevel.creative) {
            
            blockCountParent.SetActive(false);
        }
        if (!GenerateLevel.editorActive)
        {
            editorParent.SetActive(false);
        }
    }

    bool CheckValid(Vector3 mousePos)
    {
        return mousePos.x < 0 || mousePos.y < 0 ? false : true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && GenerateLevel.creative == false)
        {
            GenerateLevel.creative = true;
            blockCountParent.SetActive(false);
            return;
        }
        else if (Input.GetKeyDown(KeyCode.C) && GenerateLevel.creative == true)
        {
            GenerateLevel.creative = false;
            blockCountParent.SetActive(true);
            return;
        }

        if (screenWidthOld != Screen.width)
        {
            screenWidthOld = Screen.width;
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, blockSelectorParent.GetComponent<RectTransform>().sizeDelta.y - 70);
        }

        mousePosOld = mousePos;
        mousePos = GenerateLevel.mousePos;
        hoverBlock = GetBlockAt((int)mousePos.x, (int)mousePos.y);

        if (!CheckValid(mousePos)) return;
        if (CheckUIHover.hoverUI) return;

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
            else if (ClickTest.selectedMaterial != "nothing")
            {
                int materialIndex = System.Array.IndexOf(materials, ClickTest.selectedMaterial);
                materialRotations[materialIndex] += 1;
                select.transform.rotation = Quaternion.Euler(0, 0, materialRotations[materialIndex] * -90);
                materialObjects[materialIndex].transform.rotation = Quaternion.Euler(0, 0, materialRotations[materialIndex] * -90);
            }
        }
        
        if (ClickTest.selectedMaterial == "nothing" && ClickTest.changed) select.SetActive(false);
        if (ClickTest.selectedMaterial == "nothing") return;
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
            select.SetActive(true);
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
            if (currentBlockObject != null && currentBlockName != "nothing")
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
                int currentIndex = System.Array.IndexOf(materials, hoverBlock.type);

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

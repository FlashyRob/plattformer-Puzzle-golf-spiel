using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class LevelCreatorUI : MonoBehaviour
{
    private TMP_InputField saveAsInput;
    private TMP_Dropdown knownLevels;
    private Button saveButton;
    private Button loadButton;
    private Button exitButton;
    private JSONReader reader;
    private GenerateLevel generateLevel;
    private RectTransform rect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = GetComponent<RectTransform>();
        saveButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "SaveButton");
        loadButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "LoadButton");
        exitButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "ExitButton");
        saveAsInput = rect.GetComponentsInChildren<TMP_InputField>(true).FirstOrDefault(t => t.name == "SaveAsInput");
        knownLevels = rect.GetComponentsInChildren<TMP_Dropdown>(true).FirstOrDefault(t => t.name == "KnownLevels");
        reader = FindAnyObjectByType<JSONReader>();
        generateLevel = FindAnyObjectByType<GenerateLevel>();

        // Add the saved levels to the dropdown and display the current level in save as field.
        UpdateKnownLevels();
        DisplaySaveName(reader.saveName);

        // adding which methods I want to run when they buttons are clicked.
        saveButton.onClick.AddListener(OnSave);
        loadButton.onClick.AddListener(OnLoad);
        exitButton.onClick.AddListener(OnExit);
    }

    void UpdateKnownLevels()
    {
        var levels = reader.GetAllLevels();

        knownLevels.ClearOptions(); // clears existing options

        // Prepare new option list
        var newOptions = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Empty")
            };

        foreach (var level in levels)
        {
            if (level == "tmp") continue;
            newOptions.Add(new TMP_Dropdown.OptionData(level));
        }

        knownLevels.AddOptions(newOptions);
        knownLevels.value = 0;             // Reset selection to first option
        knownLevels.RefreshShownValue();  // Update dropdown display
    }

    void DisplaySaveName(string name)
    {
        // show the name in the Input field and select it as the current option in knownLevels if possible.
        saveAsInput.text = name;
        saveAsInput.placeholder.color = Color.black;

        for(int i = 0; i < knownLevels.options.Count; i++)
        {
            if(knownLevels.options[i].text == reader.saveName)
            {
                knownLevels.value = i;
            }
        }
        knownLevels.RefreshShownValue();
    }



    // these react to UI Button presses
    public void OnSave()
    {
        if(saveAsInput.text == "")
        {
            Debug.Log("Cannot save level. SaveText is not set");
            saveAsInput.placeholder.color = Color.red;
        }
        else
        {
            saveAsInput.placeholder.color = Color.black;
            reader.saveName = saveAsInput.text;
            Debug.Log("Save Level as " + reader.saveName);
            reader.SaveSaveFile();
            UpdateKnownLevels();
            DisplaySaveName(reader.saveName);
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnLoad()
    {
        var selected = knownLevels.options[knownLevels.value].text;
        if(selected == "Empty")
        {
            reader.saveName = "tmp";
            reader.ClearLevel();
            saveAsInput.text = "";
        }
        else
        {
            reader.saveName = selected;
            reader.LoadLevel();
            DisplaySaveName(reader.saveName);
        }
        generateLevel.Load();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnExit()
    {
        Debug.LogError("On Button Exit is not implemented yet");
    }
}

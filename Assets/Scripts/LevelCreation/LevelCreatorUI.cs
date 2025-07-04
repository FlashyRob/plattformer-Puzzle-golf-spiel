using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelCreatorUI : MonoBehaviour
{
    private TMP_InputField saveAsInput;
    private TMP_Dropdown knownLevels;
    private Button saveButton;
    private Button loadButton;
    private Button playButton;
    private Button exitButton;
    private JSONReader reader;
    private GenerateLevel generateLevel;

    private Button maybeDeleteButton;
    private Transform confirmDeletePanel;
    private Button performDeleteButton;
    private Button cancelDeleteButton;
    private TMP_Text confirmDeleteText;

    public bool levelSaved = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("LevelCreator UI start");
        var rect = GetComponent<RectTransform>();
        saveButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "SaveButton");
        loadButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "LoadButton");
        playButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "PlayButton");
        exitButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "ExitButton");
        maybeDeleteButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "MaybeDeleteButton");
        confirmDeletePanel = rect.Find("ConfirmDelete");
        performDeleteButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "PerformDeleteButton");
        cancelDeleteButton = rect.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "CancelDeleteButton");
        confirmDeleteText = rect.GetComponentsInChildren<TMP_Text>(true).FirstOrDefault(t => t.name == "ConfirmDeleteText");
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
        playButton.onClick.AddListener(OnPlay);
        knownLevels.onValueChanged.AddListener(OnLoad);

        maybeDeleteButton.onClick.AddListener(OnMaybeDelete);
        performDeleteButton.onClick.AddListener(OnPerformDelete);
        cancelDeleteButton.onClick.AddListener(OnCancelDelete);

        confirmDeletePanel.gameObject.SetActive(false);

    }

    void UpdateKnownLevels()
    {
        var levels = reader.GetAllLevels();

        knownLevels.ClearOptions(); // clears existing options

        // Prepare new option list
        var newOptions = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("Empty", null, Color.blue)
            };

        foreach (var level in levels)
        {
            if (level == "tmp") continue;
            newOptions.Add(new TMP_Dropdown.OptionData(level, null, Color.red));
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
            var scannerino = FindAnyObjectByType<ScannerinoCrocodilo>();
            if (!levelSaved) // avoid running the scanner over a level where we haven't changed anything.
            {
                scannerino.Scanner(); // ensure the network is scanned before saving the level because the play Mode does scan;
                reader.SaveSaveFile(true);
            }
            UpdateKnownLevels();
            DisplaySaveName(reader.saveName);
        }
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnLoad(int selected)
    {
        OnLoad();
    }

    public void OnLoad()
    {
        var selected = knownLevels.options[knownLevels.value].text;
        if(selected == "Empty")
        {
            ClearLevel();
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

    public void OnPlay()
    {
        Debug.Log("Play");
        OnSave();
        if (saveAsInput.text != "")
        {
            SelectedLevelPersistent.Instance.level = reader.saveName;
            SceneManager.LoadScene("PlayMode");
        }

    }

    public void OnExit()
    {
        Debug.Log("Application Quit. (Only works in build)");
        Application.Quit();
        return;
        var reader = FindAnyObjectByType<JSONReader>();
        SelectedLevelPersistent.Instance.level = reader.saveName;
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelSaved()
    {
        levelSaved = true;
        saveAsInput.textComponent.fontStyle = FontStyles.Normal;
    }

    public void LevelUnsaved()
    {
        levelSaved = false;
        saveAsInput.textComponent.fontStyle = FontStyles.Italic;
    }

    public void OnMaybeDelete()
    {
        confirmDeletePanel.gameObject.SetActive(true);
        confirmDeleteText.text = "Delete level " + knownLevels.options[knownLevels.value].text +" ?";
    }
    private void ClearLevel()
    {
        Debug.Log("Clear Level");
        reader.saveName = "tmp";
        reader.ClearLevel();
        saveAsInput.text = "";
        UpdateKnownLevels();
    }
    public void OnPerformDelete()
    {
        Debug.Log("On Perform Delete");
        confirmDeletePanel.gameObject.SetActive(false);
        var selected = knownLevels.options[knownLevels.value].text;
        Debug.Log("selected " + selected);
        if (selected == "Empty")
        {
            ClearLevel();
            generateLevel.Load();
            return;
        }
        // there is one level other than "empty" that is selected
        int value = knownLevels.value;
        var levelToDelete = knownLevels.options[value].text;
        reader.DeleteJson(levelToDelete);

        int next_value = (value + 1) % knownLevels.options.Count;
        var next_level = knownLevels.options[next_value].text;
        if(next_level == "Empty")
        {
            ClearLevel();
            generateLevel.Load();
        }
        else
        {
            UpdateKnownLevels();
            reader.saveName = next_level;
            foreach (var option in knownLevels.options)
            {
                if(option.text == levelToDelete)
                {
                    reader.saveName = levelToDelete; // this happens when you couldnt delete the level. Maybe player tried to delete developer level in build game
                    break;
                }
            }
            generateLevel.Load();
            DisplaySaveName(reader.saveName);
        }
    }

    public void OnCancelDelete()
    {
        confirmDeletePanel.gameObject.SetActive(false);
    }
}

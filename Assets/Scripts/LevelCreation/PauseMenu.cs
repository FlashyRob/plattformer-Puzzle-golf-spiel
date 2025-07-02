using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    private Button resumeButton;
    private Button reloadButton;
    private Button toEditorButton;
    private Button mainMenuButton;
    private RectTransform panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel = GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>();
        resumeButton = panel.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "ResumeButton");
        reloadButton = panel.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "ReloadButton");
        toEditorButton = panel.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "ToEditorButton");
        mainMenuButton = panel.GetComponentsInChildren<Button>(true).FirstOrDefault(t => t.name == "MainMenuButton");

        reloadButton.onClick.AddListener(OnReload);
        toEditorButton.onClick.AddListener(OnToEditor);
        mainMenuButton.onClick.AddListener(OnMainMenu);

        var buttons = new Button[] { resumeButton, reloadButton, toEditorButton, mainMenuButton };
        foreach(Button button in buttons)
        {
            button.onClick.AddListener(CloseMenu);
            button.onClick.AddListener(SetCurrentLevelNameToPersistent);
        }
        CloseMenu();
    }

    public void SetCurrentLevelNameToPersistent()
    {
        var reader = FindAnyObjectByType<JSONReader>();
        SelectedLevelPersistent.Instance.level = reader.saveName;
    }
    public void OnReload()
    {
        SceneManager.LoadScene("PlayMode");
    }

    public void OnToEditor()
    {
        SceneManager.LoadScene("LevelCreator");
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panel.gameObject.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    private void OpenMenu()
    {
        panel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        Debug.Log("Close Menu");
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}

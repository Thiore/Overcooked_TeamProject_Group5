using UnityEngine;
using UnityEngine.UI;

public class MenuInitializer : MonoBehaviour
{
    public GameObject storyPanel;
    public GameObject arcadePanel;
    public GameObject battlePanel;

    public Button storyButton;
    public Button arcadeButton;
    public Button battleButton;
    public Button characterButton;
    public Button setButton;

    private void Awake()
    {
        // Find buttons by name
        storyButton = GameObject.Find("Story_Button").GetComponent<Button>();
        arcadeButton = GameObject.Find("Arcade_Button").GetComponent<Button>();
        battleButton = GameObject.Find("Battle_Button").GetComponent<Button>();
        characterButton = GameObject.Find("Character_Button").GetComponent<Button>();
        setButton = GameObject.Find("Set_Button").GetComponent<Button>();

        // Temporarily activate panels to find their child buttons
        storyPanel = GameObject.Find("Story_Panel");
        arcadePanel = GameObject.Find("Arcade_Panel");
        battlePanel = GameObject.Find("Battle_Panel");

        storyPanel.SetActive(true);
        arcadePanel.SetActive(true);
        battlePanel.SetActive(true);

        MenuController menuController = GetComponent<MenuController>();
        menuController.InitializeSubPanelButtons();

        // Deactivate panels
        storyPanel.SetActive(false);
        arcadePanel.SetActive(false);
        battlePanel.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    private Button[] buttons;
    private int currentButtonIndex = 0;

    private GameObject storyPanel;
    private GameObject arcadePanel;
    private GameObject battlePanel;

    private Button[] storyButtons;
    private Button[] arcadeButtons;
    private Button[] battleButtons;

    private Button[] currentSubButtons;
    private int currentSubButtonIndex = 0;

    private void Awake()
    {
        // Initialize buttons array
        buttons = new Button[]
        {
            GameObject.Find("Story_Button").GetComponent<Button>(),
            GameObject.Find("Arcade_Button").GetComponent<Button>(),
            GameObject.Find("Battle_Button").GetComponent<Button>(),
            GameObject.Find("Character_Button").GetComponent<Button>(),
            GameObject.Find("Set_Button").GetComponent<Button>()
        };

        // Find panels by name
        storyPanel = GameObject.Find("Story_Panel");
        arcadePanel = GameObject.Find("Arcade_Panel");
        battlePanel = GameObject.Find("Battle_Panel");

        // Select the first button
        SelectButton(currentButtonIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeButtonSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeButtonSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleButtonPress();
        }
    }

    private void ChangeButtonSelection(int direction)
    {
        ResetButtonColor(currentButtonIndex);
        currentButtonIndex = (currentButtonIndex + direction + buttons.Length) % buttons.Length;
        SelectButton(currentButtonIndex);
    }

    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        buttons[index].GetComponentInChildren<Text>().color = Color.yellow;
        TogglePanel(index);
    }

    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    private void TogglePanel(int index)
    {
        storyPanel.SetActive(index == 0);
        arcadePanel.SetActive(index == 1);
        battlePanel.SetActive(index == 2);

        if (index < 3)
        {
            ActivateSubPanel(index);
        }
    }

    private void HandleButtonPress()
    {
        if (currentSubButtons != null && currentSubButtons.Length > 0)
        {
            currentSubButtons[currentSubButtonIndex].onClick.Invoke();
        }
        else
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }
    }

    public void InitializeSubPanelButtons()
    {
        // Initialize sub-panel buttons array
        storyButtons = storyPanel.GetComponentsInChildren<Button>(true);
        arcadeButtons = arcadePanel.GetComponentsInChildren<Button>(true);
        battleButtons = battlePanel.GetComponentsInChildren<Button>(true);
    }

    public void ActivateSubPanel(int index)
    {
        switch (index)
        {
            case 0:
                currentSubButtons = storyButtons;
                break;
            case 1:
                currentSubButtons = arcadeButtons;
                break;
            case 2:
                currentSubButtons = battleButtons;
                break;
            default:
                currentSubButtons = null;
                break;
        }

        if (currentSubButtons != null)
        {
            currentSubButtonIndex = 0;
            SelectSubButton(currentSubButtonIndex);
        }
    }

    private void SelectSubButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(currentSubButtons[index].gameObject);
        currentSubButtons[index].GetComponentInChildren<Text>().color = Color.yellow;
    }

    private void ResetSubButtonColor(int index)
    {
        currentSubButtons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    private void HandleSubButtonPress()
    {
        currentSubButtons[currentSubButtonIndex].onClick.Invoke();
    }
}

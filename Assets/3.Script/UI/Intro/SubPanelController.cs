using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SubPanelController : MonoBehaviour
{
    private Button[] storyButtons;
    private Button[] arcadeButtons;
    private Button[] battleButtons;

    private Button[] currentSubButtons;
    private int currentSubButtonIndex = 0;

    private void Awake()
    {
        // Initialize sub-panel buttons array
        storyButtons = GameObject.Find("Story_Panel").GetComponentsInChildren<Button>();
        arcadeButtons = GameObject.Find("Arcade_Panel").GetComponentsInChildren<Button>();
        battleButtons = GameObject.Find("Battle_Panel").GetComponentsInChildren<Button>();

        DeactivateSubPanels();
    }

    private void Update()
    {
        if (currentSubButtons != null)
        {
            HandleSubPanelInput();
        }
    }

    private void HandleSubPanelInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSubButtonSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSubButtonSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleSubButtonPress();
        }
    }

    private void ChangeSubButtonSelection(int direction)
    {
        ResetSubButtonColor(currentSubButtonIndex);
        currentSubButtonIndex = (currentSubButtonIndex + direction + currentSubButtons.Length) % currentSubButtons.Length;
        SelectSubButton(currentSubButtonIndex);
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

    public void DeactivateSubPanels()
    {
        storyButtons = null;
        arcadeButtons = null;
        battleButtons = null;
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    // ���� �޴� ��ư
    public Button storyButton;
    public Button arcadeButton;
    public Button battleButton;
    public Button characterButton;
    public Button setButton;

    // ���� �г�
    public GameObject storyPanel;
    public GameObject arcadePanel;
    public GameObject battlePanel;

    // ���� �г� ��ư��
    private Button[] storyButtons;
    private Button[] arcadeButtons;
    private Button[] battleButtons;

    // ���� �޴� ��ư �迭
    private Button[] buttons;
    private int currentButtonIndex = 0;
    private Button[] currentSubButtons;
    private int currentSubButtonIndex = 0;

    private void Awake()
    {
        // ���� �޴� ��ư �迭 �ʱ�ȭ
        buttons = new Button[]
        {
            storyButton,
            arcadeButton,
            battleButton,
            characterButton,
            setButton
        };

        // ù ��° ��ư ����
        SelectButton(currentButtonIndex);
    }

    private void Update()
    {
        if (GameManager.Instance.isInputEnabled)
        {
            // �¿� ����Ű �Է� ó��
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
        
    }

    // ��ư ���� ����
    private void ChangeButtonSelection(int direction)
    {
        ResetButtonColor(currentButtonIndex);
        currentButtonIndex = (currentButtonIndex + direction + buttons.Length) % buttons.Length;
        SelectButton(currentButtonIndex);
    }

    // ��ư ����
    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        buttons[index].GetComponentInChildren<Text>().color = Color.yellow;
        TogglePanel(index);
    }

    // ��ư ���� �ʱ�ȭ
    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    // �г� Ȱ��ȭ/��Ȱ��ȭ
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

    // ��ư Ŭ�� ó��
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

    // ���� �г� ��ư �ʱ�ȭ
    public void InitializeSubPanelButtons()
    {
        if (storyPanel != null)
        {
            storyButtons = storyPanel.GetComponentsInChildren<Button>(true);
        }
        if (arcadePanel != null)
        {
            arcadeButtons = arcadePanel.GetComponentsInChildren<Button>(true);
        }
        if (battlePanel != null)
        {
            battleButtons = battlePanel.GetComponentsInChildren<Button>(true);
        }
    }

    // ���� �г� Ȱ��ȭ
    private void ActivateSubPanel(int index)
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

    // ���� �г� ��ư ����
    private void SelectSubButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(currentSubButtons[index].gameObject);
        currentSubButtons[index].GetComponentInChildren<Text>().color = Color.yellow;
    }

    // ���� �г� ��ư ���� �ʱ�ȭ
    private void ResetSubButtonColor(int index)
    {
        currentSubButtons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    // ���� �г� ��ư Ŭ�� ó��
    private void HandleSubButtonPress()
    {
        currentSubButtons[currentSubButtonIndex].onClick.Invoke();
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    // 메인 메뉴 버튼
    public Button storyButton;
    public Button arcadeButton;
    public Button battleButton;
    public Button characterButton;
    public Button setButton;

    // 서브 패널
    public GameObject storyPanel;
    public GameObject arcadePanel;
    public GameObject battlePanel;

    // 서브 패널 버튼들
    private Button[] storyButtons;
    private Button[] arcadeButtons;
    private Button[] battleButtons;

    // 메인 메뉴 버튼 배열
    private Button[] buttons;
    private int currentButtonIndex = 0;
    private Button[] currentSubButtons;
    private int currentSubButtonIndex = 0;

    private void Awake()
    {
        // 메인 메뉴 버튼 배열 초기화
        buttons = new Button[]
        {
            storyButton,
            arcadeButton,
            battleButton,
            characterButton,
            setButton
        };

        // 첫 번째 버튼 선택
        SelectButton(currentButtonIndex);
    }

    private void Update()
    {
        if (GameManager.Instance.isInputEnabled)
        {
            // 좌우 방향키 입력 처리
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

    // 버튼 선택 변경
    private void ChangeButtonSelection(int direction)
    {
        ResetButtonColor(currentButtonIndex);
        currentButtonIndex = (currentButtonIndex + direction + buttons.Length) % buttons.Length;
        SelectButton(currentButtonIndex);
    }

    // 버튼 선택
    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        buttons[index].GetComponentInChildren<Text>().color = Color.yellow;
        TogglePanel(index);
    }

    // 버튼 색상 초기화
    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    // 패널 활성화/비활성화
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

    // 버튼 클릭 처리
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

    // 서브 패널 버튼 초기화
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

    // 서브 패널 활성화
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

    // 서브 패널 버튼 선택
    private void SelectSubButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(currentSubButtons[index].gameObject);
        currentSubButtons[index].GetComponentInChildren<Text>().color = Color.yellow;
    }

    // 서브 패널 버튼 색상 초기화
    private void ResetSubButtonColor(int index)
    {
        currentSubButtons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    // 서브 패널 버튼 클릭 처리
    private void HandleSubButtonPress()
    {
        currentSubButtons[currentSubButtonIndex].onClick.Invoke();
    }
}

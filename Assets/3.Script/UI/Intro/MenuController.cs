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


    // 메인 메뉴 버튼 배열
    private Button[] buttons;
    private int currentButtonIndex = 0;

    private void Start()
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
    }

    // 버튼 색상 초기화
    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    private void HandleButtonPress()
    {
        Debug.Log($"{currentButtonIndex} 눌림");
        GameManager.Instance.Menu_Button(currentButtonIndex);
    }


}

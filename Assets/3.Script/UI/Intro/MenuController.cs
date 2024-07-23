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

    // Character_Button 패널
    public GameObject characterButtonPanel;

    // 메인 메뉴 버튼 배열
    private Button[] buttons;
    private int currentButtonIndex = 0;

    private Animator camera_ani;

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

        camera_ani = GameObject.Find("Cameraman").GetComponent<Animator>();

        characterButtonPanel = GameObject.Find("Character_Button_Panel");
        // Character_Button 패널 비활성화
        characterButtonPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.isInputEnabled==1)
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
        if (currentButtonIndex == 3)
        {
            camera_ani.SetTrigger("Zoom");
            // Character_Button 패널 활성화
            characterButtonPanel.SetActive(true);
            // MenuController 입력 비활성화
            GameManager.Instance.isInputEnabled +=1;
        }
        else
        {
            GameManager.Instance.Menu_Button(currentButtonIndex);
        }
    }

    // Character_Button 패널 비활성화 및 MenuController 입력 활성화
    public void CloseCharacterButtonPanel()
    {
        characterButtonPanel.SetActive(false);
        GameManager.Instance.isInputEnabled -=1;
        camera_ani.SetTrigger("GameStart");
    }
}

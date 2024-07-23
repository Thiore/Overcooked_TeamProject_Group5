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

    // Character_Button �г�
    public GameObject characterButtonPanel;

    // ���� �޴� ��ư �迭
    private Button[] buttons;
    private int currentButtonIndex = 0;

    private Animator camera_ani;

    private void Start()
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

        camera_ani = GameObject.Find("Cameraman").GetComponent<Animator>();

        characterButtonPanel = GameObject.Find("Character_Button_Panel");
        // Character_Button �г� ��Ȱ��ȭ
        characterButtonPanel.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.isInputEnabled==1)
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
    }

    // ��ư ���� �ʱ�ȭ
    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    private void HandleButtonPress()
    {
        if (currentButtonIndex == 3)
        {
            camera_ani.SetTrigger("Zoom");
            // Character_Button �г� Ȱ��ȭ
            characterButtonPanel.SetActive(true);
            // MenuController �Է� ��Ȱ��ȭ
            GameManager.Instance.isInputEnabled +=1;
        }
        else
        {
            GameManager.Instance.Menu_Button(currentButtonIndex);
        }
    }

    // Character_Button �г� ��Ȱ��ȭ �� MenuController �Է� Ȱ��ȭ
    public void CloseCharacterButtonPanel()
    {
        characterButtonPanel.SetActive(false);
        GameManager.Instance.isInputEnabled -=1;
        camera_ani.SetTrigger("GameStart");
    }
}

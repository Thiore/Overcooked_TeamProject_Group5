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


    // ���� �޴� ��ư �迭
    private Button[] buttons;
    private int currentButtonIndex = 0;

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
    }

    // ��ư ���� �ʱ�ȭ
    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }

    private void HandleButtonPress()
    {
        Debug.Log($"{currentButtonIndex} ����");
        GameManager.Instance.Menu_Button(currentButtonIndex);
    }


}

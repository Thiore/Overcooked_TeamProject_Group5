using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Enter_Game_Controller : MonoBehaviour
{
    private Button[] buttons=new Button[2];
    private int currentButtonIndex = 0;
    private void Start()
    {
        buttons[0]= GetComponentsInChildren<Button>()[0];
        buttons[1]= GetComponentsInChildren<Button>()[1];
    }
    private void Update()
    {
        if (GameManager.Instance.isInputEnabled == 2)
        {
            // 좌우 방향키 입력 처리
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeButtonSelection(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeButtonSelection(1);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleButtonPress();
            }
        }
    }
    private void ChangeButtonSelection(int direction)
    {
        ResetButtonColor(currentButtonIndex);
        currentButtonIndex = (currentButtonIndex + direction + buttons.Length) % buttons.Length;
        SelectButton(currentButtonIndex);
    }
    private void ResetButtonColor(int index)
    {
        buttons[index].GetComponentInChildren<Text>().color = Color.white;
    }
    private void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
        buttons[index].GetComponentInChildren<Text>().color = Color.yellow;
    }
    private void HandleButtonPress()
    {
        GameManager.Instance.Enter_Game(currentButtonIndex);
    }
}

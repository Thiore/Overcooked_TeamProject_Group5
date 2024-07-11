using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseScreen;
    public List<Button> buttons;
    private int selectedButtonIndex = 0;

    private void Start()
    {
        UpdateButtonSelection();
    }

    private void Update()
    {
        if (GameManager.Instance.isPause)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectedButtonIndex = (selectedButtonIndex > 0) ? selectedButtonIndex - 1 : buttons.Count - 1;
                UpdateButtonSelection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectedButtonIndex = (selectedButtonIndex < buttons.Count - 1) ? selectedButtonIndex + 1 : 0;
                UpdateButtonSelection();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                buttons[selectedButtonIndex].onClick.Invoke();
            }
        }
    }

    private void UpdateButtonSelection()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == selectedButtonIndex) ? Color.yellow : Color.white;
            buttons[i].colors = colors;
        }
    }
}

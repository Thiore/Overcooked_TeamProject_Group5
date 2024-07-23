using UnityEngine;
using UnityEngine.UI;

public class MenuInitializer : MonoBehaviour
{
    private MenuController menuController;
    private Character_Btn_Controller characterBtnController;

    private void Awake()
    {
        menuController = GetComponent<MenuController>();

        // 버튼을 찾아 할당합니다
        menuController.storyButton = GameObject.Find("Story_Button").GetComponent<Button>();
        menuController.arcadeButton = GameObject.Find("Arcade_Button").GetComponent<Button>();
        menuController.battleButton = GameObject.Find("Battle_Button").GetComponent<Button>();
        menuController.characterButton = GameObject.Find("Character_Button").GetComponent<Button>();
        menuController.setButton = GameObject.Find("Set_Button").GetComponent<Button>();

        // Character_Btn_Controller 설정
        characterBtnController =GetComponentInChildren<Character_Btn_Controller>();
        characterBtnController.characterButtons = new GameObject[2];
        characterBtnController.characterButtons[0] = GameObject.Find("Left_Button");
        characterBtnController.characterButtons[1] = GameObject.Find("Right_Button");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MenuInitializer : MonoBehaviour
{
    private MenuController menuController;

    private void Awake()
    {
        menuController = GetComponent<MenuController>();

        // 버튼을 찾아 할당합니다
        menuController.storyButton = GameObject.Find("Story_Button").GetComponent<Button>();
        menuController.arcadeButton = GameObject.Find("Arcade_Button").GetComponent<Button>();
        menuController.battleButton = GameObject.Find("Battle_Button").GetComponent<Button>();
        menuController.characterButton = GameObject.Find("Character_Button").GetComponent<Button>();
        menuController.setButton = GameObject.Find("Set_Button").GetComponent<Button>();

        // 패널을 찾아 할당합니다
        menuController.storyPanel = GameObject.Find("Story_Panel");
        menuController.arcadePanel = GameObject.Find("Arcade_Panel");
        menuController.battlePanel = GameObject.Find("Battle_Panel");

        // 패널들을 활성화하여 자식 버튼들을 찾습니다
        menuController.storyPanel.SetActive(true);
        menuController.arcadePanel.SetActive(true);
        menuController.battlePanel.SetActive(true);

        // 패널의 자식 버튼들을 초기화합니다
        menuController.InitializeSubPanelButtons();

        // 패널을 비활성화합니다
        menuController.storyPanel.SetActive(false);
        menuController.arcadePanel.SetActive(false);
        menuController.battlePanel.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class MenuInitializer : MonoBehaviour
{
    private MenuController menuController;

    private void Awake()
    {
        menuController = GetComponent<MenuController>();

        // ��ư�� ã�� �Ҵ��մϴ�
        menuController.storyButton = GameObject.Find("Story_Button").GetComponent<Button>();
        menuController.arcadeButton = GameObject.Find("Arcade_Button").GetComponent<Button>();
        menuController.battleButton = GameObject.Find("Battle_Button").GetComponent<Button>();
        menuController.characterButton = GameObject.Find("Character_Button").GetComponent<Button>();
        menuController.setButton = GameObject.Find("Set_Button").GetComponent<Button>();

        // �г��� ã�� �Ҵ��մϴ�
        menuController.storyPanel = GameObject.Find("Story_Panel");
        menuController.arcadePanel = GameObject.Find("Arcade_Panel");
        menuController.battlePanel = GameObject.Find("Battle_Panel");

        // �гε��� Ȱ��ȭ�Ͽ� �ڽ� ��ư���� ã���ϴ�
        menuController.storyPanel.SetActive(true);
        menuController.arcadePanel.SetActive(true);
        menuController.battlePanel.SetActive(true);

        // �г��� �ڽ� ��ư���� �ʱ�ȭ�մϴ�
        menuController.InitializeSubPanelButtons();

        // �г��� ��Ȱ��ȭ�մϴ�
        menuController.storyPanel.SetActive(false);
        menuController.arcadePanel.SetActive(false);
        menuController.battlePanel.SetActive(false);
    }
}

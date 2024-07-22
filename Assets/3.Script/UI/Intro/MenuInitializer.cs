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
    }

}

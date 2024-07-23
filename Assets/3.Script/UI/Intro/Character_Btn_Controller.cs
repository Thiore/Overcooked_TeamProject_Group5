using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Btn_Controller : MonoBehaviour
{
    public GameObject[] characterButtons;
    private int currentIndex = 0;

    private GameObject[] CharacterFace = new GameObject[5];
    public int current_face_index=0;

    private void Start()
    {
        characterButtons = new GameObject[2];
        characterButtons[0] = GameObject.Find("Left_Button");
        characterButtons[1] = GameObject.Find("Right_Button");

        CharacterFace[0]= GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Beard");
        CharacterFace[1]= GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Buck");
        CharacterFace[2]= GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Dora");
        CharacterFace[3]= GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Gertie");
        CharacterFace[4]= GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Granny_Grey");

    }

    private void Update()
    {
        if (GameManager.Instance.isInputEnabled == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                characterButtons[0].GetComponent<Animator>().SetTrigger("Press");
                SwitchCharacter(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                characterButtons[1].GetComponent<Animator>().SetTrigger("Press");
                SwitchCharacter(1);
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {

                FindObjectOfType<MenuController>().CloseCharacterButtonPanel();
            }
        }
    }

    private void SwitchCharacter(int index)
    {
        if (current_face_index==4)
        {
            if (index == 1)
            {
                CharacterFace[current_face_index].SetActive(false);
                current_face_index = 0;
                CharacterFace[current_face_index].SetActive(true);
            }
            else
            {
                CharacterFace[current_face_index].SetActive(false);
                current_face_index += index;
                CharacterFace[current_face_index].SetActive(true);
            }
        }else if (current_face_index == 0)
        {
            if (index == -1)
            {
                CharacterFace[current_face_index].SetActive(false);
                current_face_index = 4;
                CharacterFace[current_face_index].SetActive(true);
            }
            else
            {
                CharacterFace[current_face_index].SetActive(false);
                current_face_index += index;
                CharacterFace[current_face_index].SetActive(true);
            }
        }
        else
        {
            CharacterFace[current_face_index].SetActive(false);
            current_face_index += index;
            CharacterFace[current_face_index].SetActive(true);
        }
        
    }
}

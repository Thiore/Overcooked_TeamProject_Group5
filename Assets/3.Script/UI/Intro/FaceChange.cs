using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceChange : MonoBehaviour
{
    private GameObject[] CharacterFace = new GameObject[5];

    private void Start()
    {
        CharacterFace[0] = GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Beard");
        CharacterFace[1] = GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Buck");
        CharacterFace[2] = GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Dora");
        CharacterFace[3] = GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Gertie");
        CharacterFace[4] = GameObject.Find("UI_Chef/UIPlayer/Chef/Mesh/Chef_Granny_Grey");

        for (int i = 0; i < 5; i++)
        {
            CharacterFace[i].SetActive(false);
        }

        switch (GameManager.Instance.Faceindex)
        {
            case 0:
                CharacterFace[0].SetActive(true);
                break;
            case 1:
                CharacterFace[1].SetActive(true);
                break;
            case 2:
                CharacterFace[2].SetActive(true);
                break;
            case 3:
                CharacterFace[3].SetActive(true);
                break;
            case 4:
                CharacterFace[4].SetActive(true);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Select : MonoBehaviour
{
    private GameObject[] Stage = new GameObject[5];
    private int selected_stage = 1;
    private GameObject[] Buttons = new GameObject[2];
    private float moveSpeed = 5f; // 이동 속도 조정
    private bool InputAble = true;

    private void Start()
    {
        for (int i = 1; i <= 5; i++)
        {
            Stage[i - 1] = GameObject.Find($"Arcade_Screen/Stage_{i}");
        }
        Buttons[0] = GameObject.Find("Left_Button");
        Buttons[1] = GameObject.Find("Right_Button");
        Buttons[0].SetActive(false);
    }

    private void Update()
    {
        Select_Stage();
    }

    private void Select_Stage()
    {
        if (InputAble)
        {
            switch (selected_stage)
            {
                case 1:
                    Buttons[0].SetActive(false);
                    Buttons[1].SetActive(true);
                    
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Change_Stage(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GameManager.Instance.LoadGame(selected_stage);
                    }
                    break;
                case 5:
                    Buttons[0].SetActive(true);
                    Buttons[1].SetActive(false);
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Change_Stage(-1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GameManager.Instance.LoadGame(selected_stage);
                    }
                    break;
                default:
                    Buttons[0].SetActive(true);
                    Buttons[1].SetActive(true);
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Change_Stage(-1);
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Change_Stage(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        GameManager.Instance.LoadGame(selected_stage);
                    }
                    break;
            }
        }

    }

    private void Change_Stage(int index)
    {
        if (index == 1)
        {
            StartCoroutine(MoveStages(-1165f));
        }
        else
        {
            StartCoroutine(MoveStages(1165f));
        }
        selected_stage += index;
    }

    private IEnumerator MoveStages(float offset)
    {
        InputAble = false;
        for (int i = 0; i < 2; i++)
        {
            Buttons[i].SetActive(false);
        }
        float elapsedTime = 0;
        Vector2[] startPos = new Vector2[Stage.Length];
        Vector2[] targetPos = new Vector2[Stage.Length];

        for (int i = 0; i < Stage.Length; i++)
        {
            RectTransform rectTransform = Stage[i].GetComponent<RectTransform>();
            startPos[i] = rectTransform.anchoredPosition;
            targetPos[i] = new Vector2(rectTransform.anchoredPosition.x + offset, rectTransform.anchoredPosition.y);
        }

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            for (int i = 0; i < Stage.Length; i++)
            {
                RectTransform rectTransform = Stage[i].GetComponent<RectTransform>();
                rectTransform.anchoredPosition = Vector2.Lerp(startPos[i], targetPos[i], elapsedTime);
            }
            yield return null;
        }

        // Ensure final position is exactly the target position
        for (int i = 0; i < Stage.Length; i++)
        {
            RectTransform rectTransform = Stage[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition = targetPos[i];
        }
        InputAble = true;
    }

}
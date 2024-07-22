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
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        Change_Stage(1);
                        Buttons[0].SetActive(true);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Load_Stage();
                    }
                    break;
                case 5:
                    Buttons[1].SetActive(false);
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        Change_Stage(-1);
                        Buttons[1].SetActive(true);
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Load_Stage();
                    }
                    break;
                default:
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
                        Load_Stage();
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

    private void Load_Stage()
    {
        switch (selected_stage)
        {
            case 1:
                GameManager.Instance.LoadGame(selected_stage);
                break;
            case 2:
                GameManager.Instance.LoadGame(selected_stage);
                break;
            case 3:
                GameManager.Instance.LoadGame(selected_stage);
                break;
            case 4:
                GameManager.Instance.LoadGame(selected_stage);
                break;
            case 5:
                GameManager.Instance.LoadGame(selected_stage);
                break;

            default:
                return;
        }
    }
}

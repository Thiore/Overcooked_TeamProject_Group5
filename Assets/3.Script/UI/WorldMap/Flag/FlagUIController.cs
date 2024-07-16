using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagUIController : MonoBehaviour
{
    private Flag flag;
    private GameObject ui;
    private Canvas flag_canvas;
    private int[] target_Score = new int[3];
    private int bestScore;
    private Text BestScore;
    private Text[] Target_Score = new Text[3];
    private Image[] FullStar = new Image[3];
    private void Awake()
    {
        InitUI();
        SetUI();
        DeActiveUI();
    }
    
    public void ActiveUI()
    {
        ui.SetActive(true);
    }
    public void DeActiveUI()
    {
        ui.SetActive(false);
    }
    private void InitUI()
    {
        TryGetComponent(out flag);
        ui = GameObject.Find("UI");
        BestScore = GameObject.Find("UI/UI_Canvas/Main_Panel/Player_Info_Panel/Player_Score").GetComponent<Text>();
        for (int i = 0; i < 3; i++)
        {
            Target_Score[i] = GameObject.Find($"UI/UI_Canvas/Main_Panel/Stage_Info/Target_Panel/EmptyStar_{i + 1}/Target_Score").GetComponent<Text>();
            FullStar[i] = GameObject.Find($"UI/UI_Canvas/Main_Panel/Stage_Info/Target_Panel/EmptyStar_{i + 1}/FullStar").GetComponent<Image>();
        }
    }
    private void SetUI()
    {
        target_Score = DataManager.Instance.GetStageInformation(flag.stage_index).targetScore;
        bestScore = DataManager.Instance.GetStageInformation(flag.stage_index).bestScore;
        BestScore.text = bestScore.ToString();
        for (int i = 0; i < 3; i++)
        {
            if (target_Score[i] > bestScore)
            {
                FullStar[i].gameObject.SetActive(false);
            }
            Target_Score[i].text = target_Score[i].ToString();
        }
    }
}

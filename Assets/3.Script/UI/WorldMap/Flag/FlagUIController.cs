using UnityEngine;
using UnityEngine.UI;

public class FlagUIController : MonoBehaviour
{
    private Flag flag;
    private GameObject ui;
    private Canvas flag_canvas;
    private int[] target_Score = new int[3];
    public int bestScore;
    private Text BestScore;
    private Text[] Target_Score = new Text[3];
    private Image[] FullStar = new Image[3];

    public delegate void OnUISetCallback();
    public event OnUISetCallback OnUISet;

    private void Start()
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
        ui = GameObject.Find($"{this.gameObject.name}/UI");
        BestScore = GameObject.Find($"{this.gameObject.name}/UI/UI_Canvas/Main_Panel/Player_Info_Panel/Player_Score").GetComponent<Text>();
        for (int i = 0; i < 3; i++)
        {
            Target_Score[i] = GameObject.Find($"{this.gameObject.name}/UI/UI_Canvas/Main_Panel/Stage_Info/Target_Panel/EmptyStar_{i + 1}/Target_Score").GetComponent<Text>();
            FullStar[i] = GameObject.Find($"{this.gameObject.name}/UI/UI_Canvas/Main_Panel/Stage_Info/Target_Panel/EmptyStar_{i + 1}/FullStar").GetComponent<Image>();
        }
    }

    private void SetUI()
    {
        target_Score = DataManager.Instance.GetStageInformation(flag.stage_index).targetScore;
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log($"{flag.stage_index}");
            //Debug.Log($"{DataManager.Instance.GetStageInformation(flag.stage_index).targetScore[i]}");
            //Debug.Log($"{target_Score[i]}");
        }
        //Debug.Log($"{this.gameObject.name} : {DataManager.Instance.GetStageInformation(flag.stage_index).bestScore}");
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
        //SetFlagMaterialOffset(bestScore);
        OnUISet?.Invoke(); // SetUI 메서드가 완료된 후 이벤트 호출
    }
    //private void SetFlagMaterialOffset(int score)
    //{
    //    Vector2 offset = Vector2.zero;

    //    if (score >= 0 && score <= 39)
    //    {
    //        offset = new Vector2(0.69f, -0.25f);
    //    }
    //    else if (score >= 40 && score <= 59)
    //    {
    //        offset = new Vector2(0f, -0.25f);
    //    }
    //    else if (score >= 60 && score <= 99)
    //    {
    //        offset = new Vector2(0f, -0.5f);
    //    }
    //    else if (score >= 100)
    //    {
    //        offset = new Vector2(0f, -0.75f);
    //    }

    //    Transform childTransform = transform.Find("m_map_completed_flag_01_0.001_m_map_completed_flag_01_0.001"); // 자식 오브젝트 이름으로 변경
    //    if (childTransform != null)
    //    {
    //        Renderer renderer = childTransform.GetComponent<Renderer>();
    //        if (renderer != null && renderer.material != null)
    //        {
    //            renderer.material.SetTextureOffset("_MainTex", offset);
    //        }
    //    }
    //}
}

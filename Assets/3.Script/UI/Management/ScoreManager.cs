using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static ScoreManager instance;
    
    // 싱글톤 인스턴스에 접근하기 위한 프로퍼티
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("ScoreManager instance is null!");
            }
            return instance;
        }
    }
    public int stage_index;
    public Score_UI_Manager score_ui;
    public int Tip_Score { get; private set; }  // 팁으로 인해 얻은 추가 점수를 기록하는 변수
    public int[] TargetScore = new int[3]; // 목표 점수 배열
    [SerializeField]public int score { get; private set; } // 현재 점수
    
    public int tip { get; set; } // 팁 배수
    public int addScoreCount { get; private set; } // 점수를 얻은 횟수
    public int TotalAddScore;
    public int subScoreCount { get; private set; } // 점수를 잃은 횟수
    public int subScore { get; private set; } // 차감된 점수 합계

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 오브젝트가 씬 전환 시 파괴되지 않도록 설정
        }
        else if (instance != this)
        {
            // 이미 인스턴스가 존재하면 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        stage_index = GameManager.Instance.stage_index;
        // 점수 초기화
        InitializeScores();
        UpdateTargetScore();
    }


    // 점수를 초기화하는 메서드
    public void InitializeScores()
    {
        score = 100;
        Tip_Score = 0;
        tip = 1;
        addScoreCount = 0;
        subScoreCount = 0;
        subScore = 0;
    }

    public void UpdateTargetScore()
    {
        TargetScore = DataManager.Instance.GetStageInformation(stage_index).targetScore;
    }

    // 점수를 추가하는 메서드
    public void AddScore(int points)
    {
        int tipMultiplier = tip; // 팁 배수
        int additionalPoints = Mathf.CeilToInt(points * 0.1f * tipMultiplier); // 팁에 의한 추가 점수
        score += points; // 기본 점수 추가
        TotalAddScore += points;
        Tip_Score += additionalPoints; // 팁 점수 추가
        addScoreCount += 1;
        score += additionalPoints; // 총 점수 업데이트
        score_ui.ScoreAni.SetTrigger("GetScore");
    }

    // 점수를 차감하는 메서드
    public void SubScore(int points)
    {
        score -= points;
        if (score < 0)
        {
            score = 0;
        }
        subScoreCount += 1;
        subScore += points;
        score_ui.ScoreAni.SetTrigger("GetScore");
    }
}

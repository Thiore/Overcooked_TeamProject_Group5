using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    private static ScoreManager instance;
    
    // �̱��� �ν��Ͻ��� �����ϱ� ���� ������Ƽ
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
    public int Tip_Score { get; private set; }  // ������ ���� ���� �߰� ������ ����ϴ� ����
    public int[] TargetScore = new int[3]; // ��ǥ ���� �迭
    [SerializeField]public int score { get; private set; } // ���� ����
    
    public int tip { get; set; } // �� ���
    public int addScoreCount { get; private set; } // ������ ���� Ƚ��
    public int TotalAddScore;
    public int subScoreCount { get; private set; } // ������ ���� Ƚ��
    public int subScore { get; private set; } // ������ ���� �հ�

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ������Ʈ�� �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else if (instance != this)
        {
            // �̹� �ν��Ͻ��� �����ϸ� �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        stage_index = GameManager.Instance.stage_index;
        // ���� �ʱ�ȭ
        InitializeScores();
        UpdateTargetScore();
    }


    // ������ �ʱ�ȭ�ϴ� �޼���
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

    // ������ �߰��ϴ� �޼���
    public void AddScore(int points)
    {
        int tipMultiplier = tip; // �� ���
        int additionalPoints = Mathf.CeilToInt(points * 0.1f * tipMultiplier); // ���� ���� �߰� ����
        score += points; // �⺻ ���� �߰�
        TotalAddScore += points;
        Tip_Score += additionalPoints; // �� ���� �߰�
        addScoreCount += 1;
        score += additionalPoints; // �� ���� ������Ʈ
        score_ui.ScoreAni.SetTrigger("GetScore");
    }

    // ������ �����ϴ� �޼���
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

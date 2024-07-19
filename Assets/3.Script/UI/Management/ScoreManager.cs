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
    public Text ScoreText;    // ������ ǥ���� UI �ؽ�Ʈ
    public Text TipText;      // ���� ǥ���� UI �ؽ�Ʈ
    public Animator ScoreAni; // ������ ����� �� �ִϸ��̼��� ����� �ִϸ�����
    private GameObject coinbanner;
    public int Tip_Score { get; private set; }  // ������ ���� ���� �߰� ������ ����ϴ� ����
    public int[] TargetScore = new int[3]; // ��ǥ ���� �迭
    public int score { get; private set; } // ���� ����
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
        coinbanner = FindObjectOfType<Canvas>().gameObject.transform.Find("Coin_Banner").GetComponent<Image>().gameObject;
        
        // �ڽ� ������Ʈ �� �̸��� "ScoreText"�� Text ������Ʈ�� �����ɴϴ�.
        ScoreText = coinbanner.transform.Find("ScoreText").GetComponent<Text>();

        // �ڽ� ������Ʈ �� �̸��� "TipText"�� Text ������Ʈ�� �����ɴϴ�.
        TipText = coinbanner.transform.Find("DarkBack").GetComponentInChildren<Text>();

        // �ڽ� ������Ʈ �� �̸��� "Coin_Image"�� Animator ������Ʈ�� �����ɴϴ�.
        ScoreAni = coinbanner.transform.Find("Coin_Image").GetComponent<Animator>();
        stage_index = GameManager.Instance.stage_index;

        // ���� �ʱ�ȭ
        InitializeScores();
        UpdateTargetScore();
    }

    private void Update()
    {
        // ���� ������ UI�� ������Ʈ
        if (score.ToString() != ScoreText.text)
        {
            ScoreText.text = $"{score}";
        }

        // �� ������ UI�� ������Ʈ
        TipText.text = $"�� x {tip}";
    }

    // ������ �ʱ�ȭ�ϴ� �޼���
    public void InitializeScores()
    {
        score = 0;
        Tip_Score = 0;
        tip = 1;
        addScoreCount = 0;
        subScoreCount = 0;
        subScore = 0;
        for (int i = 0; i < TargetScore.Length; i++)
        {
            TargetScore[i] = 0;
        }
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
        ScoreAni.SetTrigger("GetScore");
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
        ScoreAni.SetTrigger("GetScore");
    }
}

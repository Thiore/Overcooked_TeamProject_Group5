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

    public Text ScoreText;    // ������ ǥ���� UI �ؽ�Ʈ
    public Text TipText;      // ���� ǥ���� UI �ؽ�Ʈ
    public Animator ScoreAni; // ������ ����� �� �ִϸ��̼��� ����� �ִϸ�����
    public int Tip_Score { get; private set; }  // ������ ���� ���� �߰� ������ ����ϴ� ����
    public int[] TargetScore = new int[3]; // ��ǥ ���� �迭
    public int Score { get; private set; }       // ���� ����

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
        // �ڽ� ������Ʈ �� �̸��� "ScoreText"�� Text ������Ʈ�� �����ɴϴ�.
        ScoreText = transform.Find("ScoreText").GetComponent<Text>();

        // �ڽ� ������Ʈ �� �̸��� "TipText"�� Text ������Ʈ�� �����ɴϴ�.
        TipText = transform.Find("DarkBack").GetComponentInChildren<Text>();

        // �ڽ� ������Ʈ �� �̸��� "Coin_Image"�� Animator ������Ʈ�� �����ɴϴ�.
        ScoreAni = transform.Find("Coin_Image").GetComponent<Animator>();

        // ���� �ʱ�ȭ
        InitializeScores();
    }

    private void Update()
    {
        // GameManager �ν��Ͻ��� ������ ���� ����
        if (GameManager.Instance != null)
        {
            // ���� ������ GameManager���� �����ͼ� UI ������Ʈ
            int currentScore = GameManager.Instance.score;
            if (currentScore.ToString() != ScoreText.text)
            {
                ScoreText.text = $"{currentScore}";
                ScoreAni.SetTrigger("GetScore");
            }
        }

        // �� ������ UI�� ������Ʈ
        TipText.text = $"�� x {GameManager.Instance.tip}";
    }

    // ������ �ʱ�ȭ�ϴ� �޼���
    public void InitializeScores()
    {
        Score = 0;
        Tip_Score = 0;
        for (int i = 0; i < TargetScore.Length; i++)
        {
            TargetScore[i] = 0;
        } 
    }

    // ������ �߰��ϴ� �޼���
    public void AddScore(int points)
    {
        int tipMultiplier = GameManager.Instance.tip; // �� ���
        int additionalPoints = Mathf.CeilToInt(points * 0.1f * tipMultiplier); // ���� ���� �߰� ����
        Score += points; // �⺻ ���� �߰�
        Tip_Score += additionalPoints; // �� ���� �߰�
        GameManager.Instance.score = Score + Tip_Score; // �� ���� ������Ʈ
    }

    // ������ �����ϴ� �޼���
    public void SubScore(int points)
    {
        Score -= points;
        if (Score < 0)
        {
            Score = 0;
        }
        GameManager.Instance.score = Score; // �� ���� ������Ʈ
    }
}

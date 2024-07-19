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

    public Text ScoreText;    // 점수를 표시할 UI 텍스트
    public Text TipText;      // 팁을 표시할 UI 텍스트
    public Animator ScoreAni; // 점수가 변경될 때 애니메이션을 재생할 애니메이터
    public int Tip_Score { get; private set; }  // 팁으로 인해 얻은 추가 점수를 기록하는 변수
    public int[] TargetScore = new int[3]; // 목표 점수 배열
    public int Score { get; private set; }       // 현재 점수

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
        // 자식 오브젝트 중 이름이 "ScoreText"인 Text 컴포넌트를 가져옵니다.
        ScoreText = transform.Find("ScoreText").GetComponent<Text>();

        // 자식 오브젝트 중 이름이 "TipText"인 Text 컴포넌트를 가져옵니다.
        TipText = transform.Find("DarkBack").GetComponentInChildren<Text>();

        // 자식 오브젝트 중 이름이 "Coin_Image"인 Animator 컴포넌트를 가져옵니다.
        ScoreAni = transform.Find("Coin_Image").GetComponent<Animator>();

        // 점수 초기화
        InitializeScores();
    }

    private void Update()
    {
        // GameManager 인스턴스가 존재할 때만 실행
        if (GameManager.Instance != null)
        {
            // 현재 점수를 GameManager에서 가져와서 UI 업데이트
            int currentScore = GameManager.Instance.score;
            if (currentScore.ToString() != ScoreText.text)
            {
                ScoreText.text = $"{currentScore}";
                ScoreAni.SetTrigger("GetScore");
            }
        }

        // 팁 정보를 UI에 업데이트
        TipText.text = $"팁 x {GameManager.Instance.tip}";
    }

    // 점수를 초기화하는 메서드
    public void InitializeScores()
    {
        Score = 0;
        Tip_Score = 0;
        for (int i = 0; i < TargetScore.Length; i++)
        {
            TargetScore[i] = 0;
        } 
    }

    // 점수를 추가하는 메서드
    public void AddScore(int points)
    {
        int tipMultiplier = GameManager.Instance.tip; // 팁 배수
        int additionalPoints = Mathf.CeilToInt(points * 0.1f * tipMultiplier); // 팁에 의한 추가 점수
        Score += points; // 기본 점수 추가
        Tip_Score += additionalPoints; // 팁 점수 추가
        GameManager.Instance.score = Score + Tip_Score; // 총 점수 업데이트
    }

    // 점수를 차감하는 메서드
    public void SubScore(int points)
    {
        Score -= points;
        if (Score < 0)
        {
            Score = 0;
        }
        GameManager.Instance.score = Score; // 총 점수 업데이트
    }
}

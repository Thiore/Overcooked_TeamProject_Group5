using UnityEngine;
using UnityEngine.UI;

public class TicketManager : MonoBehaviour
{
    public GameObject ResultTicket; // 결과 티켓 UI 오브젝트
    public Text[] StarTexts; // 별점 텍스트 배열
    public Image[] FullStars; // 가득 찬 별 이미지 배열
    public Text ResultSum; // 결과 요약 텍스트
    public Text ResultSumScore; // 결과 점수 요약 텍스트
    public Text TotalScore; // 총 점수 텍스트

    private void Start()
    {
        // Result_Ticket 오브젝트 찾기
        ResultTicket = GameObject.Find("Main_Canvas/Result_Ticket");

        // 별점 텍스트와 가득 찬 별 이미지를 배열로 초기화
        StarTexts = new Text[3];
        FullStars = new Image[3];

        for (int i = 0; i < 3; i++)
        {
            StarTexts[i] = ResultTicket.transform.Find($"Star_{i + 1}/Text").GetComponent<Text>();
            FullStars[i] = ResultTicket.transform.Find($"Star_{i + 1}/Full_Star_{i + 1}").GetComponent<Image>();
        }

        // 결과 요약 텍스트와 총 점수 텍스트 초기화
        ResultSum = ResultTicket.transform.Find("Result_Sum").GetComponent<Text>();
        ResultSumScore = ResultTicket.transform.Find("Result_Sum_Score").GetComponent<Text>();
        TotalScore = ResultTicket.transform.Find("Total_Score").GetComponent<Text>();

        // UI 업데이트
        UpdateResultUI();
    }

    // 결과 UI를 업데이트하는 메서드
    public void UpdateResultUI()
    {
        int[] targetScores = ScoreManager.Instance.TargetScore; // 목표 점수 배열
        int currentScore = ScoreManager.Instance.Score; // 현재 점수

        for (int i = 0; i < StarTexts.Length; i++)
        {
            StarTexts[i].text = targetScores[i].ToString();
            FullStars[i].gameObject.SetActive(currentScore > targetScores[i]); // 목표 점수를 초과한 경우 별 이미지 활성화
        }

        int addScoreCount = GameManager.Instance.addScoreCount; // 점수를 얻은 횟수
        int subScoreCount = GameManager.Instance.subScoreCount; // 점수를 잃은 횟수
        ResultSum.text = $"배달된 주문 x {addScoreCount}\n팁\n실패한 주문 x {subScoreCount}";

        int addScore = ScoreManager.Instance.Score; // 추가된 점수
        int tipScore = ScoreManager.Instance.Tip_Score; // 팁 점수
        int subScore = GameManager.Instance.subScore; // 차감된 점수
        ResultSumScore.text = $"{addScore}\n{tipScore}\n{subScore}";

        TotalScore.text = $"{currentScore}"; // 총 점수 업데이트
    }
}

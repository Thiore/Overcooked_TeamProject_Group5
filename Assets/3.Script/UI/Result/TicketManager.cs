using UnityEngine;
using UnityEngine.UI;

public class TicketManager : MonoBehaviour
{
    public GameObject ResultTicket; // ��� Ƽ�� UI ������Ʈ
    public Text[] StarTexts; // ���� �ؽ�Ʈ �迭
    public Image[] FullStars; // ���� �� �� �̹��� �迭
    public Text ResultSum; // ��� ��� �ؽ�Ʈ
    public Text ResultSumScore; // ��� ���� ��� �ؽ�Ʈ
    public Text TotalScore; // �� ���� �ؽ�Ʈ

    private void Start()
    {
        // Result_Ticket ������Ʈ ã��
        ResultTicket = GameObject.Find("Main_Canvas/Result_Ticket");

        // ���� �ؽ�Ʈ�� ���� �� �� �̹����� �迭�� �ʱ�ȭ
        StarTexts = new Text[3];
        FullStars = new Image[3];

        for (int i = 0; i < 3; i++)
        {
            StarTexts[i] = ResultTicket.transform.Find($"Star_{i + 1}/Text").GetComponent<Text>();
            FullStars[i] = ResultTicket.transform.Find($"Star_{i + 1}/Full_Star_{i + 1}").GetComponent<Image>();
        }

        // ��� ��� �ؽ�Ʈ�� �� ���� �ؽ�Ʈ �ʱ�ȭ
        ResultSum = ResultTicket.transform.Find("Result_Sum").GetComponent<Text>();
        ResultSumScore = ResultTicket.transform.Find("Result_Sum_Score").GetComponent<Text>();
        TotalScore = ResultTicket.transform.Find("Total_Score").GetComponent<Text>();

        // UI ������Ʈ
        UpdateResultUI();
    }

    // ��� UI�� ������Ʈ�ϴ� �޼���
    public void UpdateResultUI()
    {
        int[] targetScores = ScoreManager.Instance.TargetScore; // ��ǥ ���� �迭
        int currentScore = ScoreManager.Instance.Score; // ���� ����

        for (int i = 0; i < StarTexts.Length; i++)
        {
            StarTexts[i].text = targetScores[i].ToString();
            FullStars[i].gameObject.SetActive(currentScore > targetScores[i]); // ��ǥ ������ �ʰ��� ��� �� �̹��� Ȱ��ȭ
        }

        int addScoreCount = GameManager.Instance.addScoreCount; // ������ ���� Ƚ��
        int subScoreCount = GameManager.Instance.subScoreCount; // ������ ���� Ƚ��
        ResultSum.text = $"��޵� �ֹ� x {addScoreCount}\n��\n������ �ֹ� x {subScoreCount}";

        int addScore = ScoreManager.Instance.Score; // �߰��� ����
        int tipScore = ScoreManager.Instance.Tip_Score; // �� ����
        int subScore = GameManager.Instance.subScore; // ������ ����
        ResultSumScore.text = $"{addScore}\n{tipScore}\n{subScore}";

        TotalScore.text = $"{currentScore}"; // �� ���� ������Ʈ
    }
}

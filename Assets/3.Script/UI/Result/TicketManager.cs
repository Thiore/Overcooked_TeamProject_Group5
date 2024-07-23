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
    public Text TitleText;

    private void Start()
    {
        // Result_Ticket ������Ʈ ã��
        ResultTicket = GameObject.Find("Main_Canvas/Result_Ticket");

        // ���� �ؽ�Ʈ�� ���� �� �� �̹����� �迭�� �ʱ�ȭ
        StarTexts = new Text[3];
        FullStars = new Image[3];

        for (int i = 0; i < 3; i++)
        {
            StarTexts[i] = ResultTicket.transform.Find($"Star_{i + 1}/Target_Score_{i+1}").GetComponent<Text>();
            FullStars[i] = ResultTicket.transform.Find($"Star_{i + 1}/Full_Star_{i + 1}").GetComponent<Image>();
        }

        // ��� ��� �ؽ�Ʈ�� �� ���� �ؽ�Ʈ �ʱ�ȭ
        ResultSum = ResultTicket.transform.Find("Result_Sum").GetComponent<Text>();
        ResultSumScore = ResultTicket.transform.Find("Result_Sum_Score").GetComponent<Text>();
        TotalScore = ResultTicket.transform.Find("Total_Score").GetComponent<Text>();
        TitleText = ResultTicket.transform.Find("Title").GetComponentInChildren<Text>();

        // UI ������Ʈ
        UpdateResultUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.LoadScene("Bus_Flag");
        }
    }

    // ��� UI�� ������Ʈ�ϴ� �޼���
    public void UpdateResultUI()
    {
        int[] targetScores = ScoreManager.Instance.TargetScore; // ��ǥ ���� �迭
        int currentScore = ScoreManager.Instance.score; // ���� ����
        TotalScore.text = $"{currentScore}"; // �� ���� ������Ʈ
        if (currentScore > DataManager.Instance.GetStageInformation(GameManager.Instance.stage_index).bestScore)
        {
            DataManager.Instance.GetStageInformation(GameManager.Instance.stage_index).bestScore = currentScore;
            TotalScore.text = $"BEST Score !! {currentScore}"; // �� ���� ������Ʈ
        }
        TitleText.text=$"Stage {GameManager.Instance.stage_index}";
        for (int i = 0; i < StarTexts.Length; i++)
        {
            StarTexts[i].text = targetScores[i].ToString();
            FullStars[i].gameObject.SetActive(currentScore > targetScores[i]); // ��ǥ ������ �ʰ��� ��� �� �̹��� Ȱ��ȭ
        }

        int addScoreCount = ScoreManager.Instance.addScoreCount; // ������ ���� Ƚ��
        int subScoreCount = ScoreManager.Instance.subScoreCount; // ������ ���� Ƚ��
        ResultSum.text = $"��޵� �ֹ� x {addScoreCount}\n��\n������ �ֹ� x {subScoreCount}";

        int addScore = ScoreManager.Instance.TotalAddScore; // �߰��� ����
        int tipScore = ScoreManager.Instance.Tip_Score; // �� ����
        int subScore = ScoreManager.Instance.subScore; // ������ ����
        ResultSumScore.text = $"{addScore}\n{tipScore}\n{subScore}";

    }
}

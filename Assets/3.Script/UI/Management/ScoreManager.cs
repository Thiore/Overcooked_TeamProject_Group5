using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text ScoreText;
    private Text TipText;
    private Animator ScoreAni;
    private void Start()
    {
        // �ڽ� ������Ʈ �� �̸��� "ScoreText"�� Text ������Ʈ�� �����ɴϴ�.
        ScoreText = transform.Find("ScoreText").GetComponent<Text>();

        // �ڽ� ������Ʈ �� �̸��� "TipText"�� Text ������Ʈ�� �����ɴϴ�.
        TipText = transform.Find("DarkBack").GetComponentInChildren<Text>();

        ScoreAni = transform.Find("Coin_Image").GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            int currentScore = GameManager.Instance.score;
            if (currentScore.ToString() != ScoreText.text)
            {
                ScoreText.text = $"{currentScore}";
                ScoreAni.SetTrigger("GetScore");
            }
            
        }
        TipText.text = $"�� x {GameManager.Instance.tip}";
    }
}

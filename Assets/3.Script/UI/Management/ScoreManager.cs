using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text ScoreText;
    private Text TipText;
    private Animator ScoreAni;
    private void Start()
    {
        // 자식 오브젝트 중 이름이 "ScoreText"인 Text 컴포넌트를 가져옵니다.
        ScoreText = transform.Find("ScoreText").GetComponent<Text>();

        // 자식 오브젝트 중 이름이 "TipText"인 Text 컴포넌트를 가져옵니다.
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
        TipText.text = $"팁 x {GameManager.Instance.tip}";
    }
}

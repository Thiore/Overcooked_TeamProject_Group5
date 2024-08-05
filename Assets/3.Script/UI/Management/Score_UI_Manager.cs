using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_UI_Manager : MonoBehaviour
{

    private Text ScoreText;    // 점수를 표시할 UI 텍스트
    private Text TipText;      // 팁을 표시할 UI 텍스트
    public Animator ScoreAni; // 점수가 변경될 때 애니메이션을 재생할 애니메이터
    private GameObject coinbanner;

    private void Start()
    {
        coinbanner = GameObject.Find("Coin_Banner").GetComponent<Image>().gameObject;

        // 자식 오브젝트 중 이름이 "ScoreText"인 Text 컴포넌트를 가져옵니다.
        ScoreText = coinbanner.transform.Find("ScoreText").GetComponent<Text>();
        Debug.Log($"ScoreText : {ScoreText.gameObject.name}");

        // 자식 오브젝트 중 이름이 "TipText"인 Text 컴포넌트를 가져옵니다.
        TipText = coinbanner.transform.Find("DarkBack").GetComponentInChildren<Text>();

        // 자식 오브젝트 중 이름이 "Coin_Image"인 Animator 컴포넌트를 가져옵니다.
        ScoreAni = coinbanner.transform.Find("Coin_Image").GetComponent<Animator>();

        ScoreManager.Instance.score_ui = this;
    }
    private void Update()
    {
        
        // 현재 점수를 UI에 업데이트
        if (ScoreManager.Instance.score.ToString() != ScoreText.text)
        {
            ScoreText.text = $"{ScoreManager.Instance.score}";
        }

        // 팁 정보를 UI에 업데이트
        TipText.text = $"팁 x {ScoreManager.Instance.tip}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_UI_Manager : MonoBehaviour
{

    private Text ScoreText;    // ������ ǥ���� UI �ؽ�Ʈ
    private Text TipText;      // ���� ǥ���� UI �ؽ�Ʈ
    public Animator ScoreAni; // ������ ����� �� �ִϸ��̼��� ����� �ִϸ�����
    private GameObject coinbanner;

    private void Start()
    {
        coinbanner = GameObject.Find("Coin_Banner").GetComponent<Image>().gameObject;

        // �ڽ� ������Ʈ �� �̸��� "ScoreText"�� Text ������Ʈ�� �����ɴϴ�.
        ScoreText = coinbanner.transform.Find("ScoreText").GetComponent<Text>();
        Debug.Log($"ScoreText : {ScoreText.gameObject.name}");

        // �ڽ� ������Ʈ �� �̸��� "TipText"�� Text ������Ʈ�� �����ɴϴ�.
        TipText = coinbanner.transform.Find("DarkBack").GetComponentInChildren<Text>();

        // �ڽ� ������Ʈ �� �̸��� "Coin_Image"�� Animator ������Ʈ�� �����ɴϴ�.
        ScoreAni = coinbanner.transform.Find("Coin_Image").GetComponent<Animator>();

        ScoreManager.Instance.score_ui = this;
    }
    private void Update()
    {
        
        // ���� ������ UI�� ������Ʈ
        if (ScoreManager.Instance.score.ToString() != ScoreText.text)
        {
            ScoreText.text = $"{ScoreManager.Instance.score}";
        }

        // �� ������ UI�� ������Ʈ
        TipText.text = $"�� x {ScoreManager.Instance.tip}";
    }
}

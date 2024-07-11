using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float initTime = 60f;
    public float playTime = 0;
    public int tip = 1;
    private float tipTime = 0;
    private Animator timeAni;
    private Text timeText;

    private void Start()
    {
        timeAni = GetComponent<Animator>();
        timeText = GetComponentInChildren<Text>();
        GameManager.Instance.isPause = false;
        GameManager.Instance.isPlaying = true;
    }

    private void Update()
    {
        if (!GameManager.Instance.isPause && GameManager.Instance.isPlaying)
        {
            tipTime += Time.deltaTime;
            playTime += Time.deltaTime;
            float remainingTime = initTime - playTime;

            if (tipTime >= 20)
            {
                tip += 1;
                tipTime = 0f;
            }

            if (remainingTime <= 0)
            {
                playTime = 0;
                GameManager.Instance.EndGame();
            }

            UpdateTimeDisplay(remainingTime);
        }
    }

    private void UpdateTimeDisplay(float remainingTime)
    {
        if (remainingTime <= 30f && remainingTime >= 29f)
        {
            timeAni.SetBool("isThirtySecLeft", true);
        }
        else if (remainingTime <= 10f && remainingTime >= 9f)
        {
            timeAni.SetBool("isThirtySecLeft", false);
            timeAni.SetBool("isTenSecLeft", true);
        }

        if (remainingTime >= 60f)
        {
            int min = (int)remainingTime / 60;
            int sec = (int)remainingTime % 60;
            timeText.text = sec > 9 ? $"0{min}:{sec}" : $"0{min}:0{sec}";
        }
        else
        {
            int sec = (int)remainingTime;
            timeText.text = sec > 9 ? $"00:{sec}" : $"00:0{sec}";
        }
    }
}

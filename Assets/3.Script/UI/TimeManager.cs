using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private Animator timeAni;
    private Text timeText;
    private void Start()
    {
        timeAni = GetComponent<Animator>();
        timeText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (GameManager.Instance != null)
        {
            float remainingTime = GameManager.Instance.initTime- GameManager.Instance.playTime;
            if (remainingTime <= 30f&&remainingTime>=29f)
            {
                timeAni.SetBool("isThirtySecLeft", true);
            }else if (remainingTime <= 10f && remainingTime >= 9f)
            {
                timeAni.SetBool("isThirtySecLeft", false);
                timeAni.SetBool("isTenSecLeft", true);
            }
            if (remainingTime >= 60f)
            {
                int min = (int)remainingTime / 60;
                if ((int)remainingTime - min * 60 > 9)
                {
                    timeText.text = $"0{min} : {(int)remainingTime - min * 60}";
                }
                else
                {
                    timeText.text = $"0{min} : 0{(int)remainingTime - min * 60}";
                }
               
            }
            else
            {
                if ((int)remainingTime > 9)
                {
                    timeText.text = $"00:{(int)remainingTime}";
                }
                else
                {
                    timeText.text = $"00 :0{(int)remainingTime}";
                }
                
            }
           
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Update()
    {
        slider.maxValue = GameManager.Instance.initTime;
        slider.value = slider.maxValue-GameManager.Instance.playTime;
        if (slider.maxValue/slider.value>=5)
        {
            Image img = transform.Find("Fill Area").GetComponentInChildren<Image>();
            img.color = Color.red;
        }else if(slider.maxValue / slider.value >= 2)
        {
            Image img = transform.Find("Fill Area").GetComponentInChildren<Image>();
            img.color = Color.yellow;
        }
        else
        {
            Image img = transform.Find("Fill Area").GetComponentInChildren<Image>();
            img.color = Color.green;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fryer : Cookingtool
{

    [SerializeField] private Renderer[] _renderer;
    [SerializeField] private Texture2D FalloffTexture;
    public float CookTime;
    public readonly float FinishCookTime = 6f;

    private void Awake()
    {
        isSoup = false;
        isCooking = false;
        isFinish = false;
        CookTime = 0;

    }
    private void Update()
    {


        if (isSoup && !isFinish)
        {
            if (slider == null)
            {
                isPlate = false;
                transform.parent.TryGetComponent(out CounterController counter);
                slider = counter.Slider.GetComponent<Slider>();
                slider.maxValue = FinishCookTime;
                slider.value = CookTime;
                slider.gameObject.SetActive(true);
                
            }
            if (slider != null)
            {
                slider.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 80f;
            }
            CookTime += Time.deltaTime;
            slider.value = CookTime;
            Debug.Log(CookTime);
            if (CookTime > FinishCookTime)
            {
                if (!isFinish)
                {
                    Ingre.SetReadyCook();
                    //Soup_Anim.SetTrigger("Cook");
                    CookTime = 0f;
                    SaveRange.GetChild(0).gameObject.SetActive(false);
                    isFinish = true;
                    slider.gameObject.SetActive(false);
                    slider = null;

                    Debug.Log("조리끝");
                }

                //초록 UI 띠링
                return;
            }
        }
        if (isPlate)
        {
            
                Ingre = null;
                CookTime = 0f;
                isSoup = false;
                gameObject.name = "Pot";
            

        }
    }

}

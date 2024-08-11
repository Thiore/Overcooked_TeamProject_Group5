using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FryingPan : Cookingtool
{

    [SerializeField] private Renderer[] _renderer;
    [SerializeField] private Texture2D FalloffTexture;
    public float CookTime;
    public readonly float FinishCookTime = 6f;

    private Renderer targetRenderer;
    Color cookColor;
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
                targetRenderer = transform.GetChild(0).GetComponent<Renderer>();
                cookColor = targetRenderer.material.GetColor("_EmissionColor");
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
            if (cookColor.r > 0f)
            {
                cookColor.r -= Time.deltaTime * 3f / 255f;
                cookColor.g -= Time.deltaTime * 3f / 255f;
                cookColor.b -= Time.deltaTime * 3f / 255f;
                targetRenderer.material.SetColor("_EmissionColor", cookColor);
            }
            slider.value = CookTime;
            //Debug.Log(CookTime);
            if (CookTime > FinishCookTime)
            {
                if (!isFinish)
                {
                    Ingre.SetReadyCook();
                    //Soup_Anim.SetTrigger("Cook");
                    CookTime = 0f;
                    //SaveRange.GetChild(0).gameObject.SetActive(false);
                    isFinish = true;
                    slider.gameObject.SetActive(false);
                    slider = null;

                    //Debug.Log("조리끝");
                }

                //초록 UI 띠링
                return;
            }
        }
        if (isPlate)
        {
            isSoup = false;
            Ingre = null;
            CookTime = 0f;
            gameObject.name = "FryPan";
        }
    }

    public override void ResetCook(Ingredient ingre)
    {
        isSoup = false;
        isFinish = false;
        Ingre = ingre;
        Ingre.transform.SetParent(transform);
        Ingre.transform.position = transform.position;
        Ingre.transform.rotation = transform.rotation;
    }

}

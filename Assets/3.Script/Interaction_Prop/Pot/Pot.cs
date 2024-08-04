using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookingtool
{
    [SerializeField] private Renderer[] renderer;
    [SerializeField] private Texture2D FalloffTexture;

    

   

    public float CookTime;
    public readonly float FinishCookTime = 6f;
    //public readonly float OverCookTime = 6f;
    //public readonly float FireTime = 10f;
    private Color BaseColor;

    

    private void Awake()
    {
        BaseColor = this.renderer[0].material.GetColor("_BaseColor");
        isSoup = false;
        isCooking = false;
        isFinish = false;
        CookTime = 0;

    }
    private void Update()
    {
        

        if(isSoup&&!isFinish)
        { 
                
                CookTime += Time.deltaTime;
                Debug.Log(CookTime);
            if (CookTime > FinishCookTime)
            {
                if (!isFinish)
                {
                    Ingre.SetReadyCook();
                    Soup_Anim.SetTrigger("Cook");
                    CookTime = 0f;
                    SaveRange.GetChild(0).gameObject.SetActive(false);
                    isFinish = true;
                    Debug.Log("조리끝");
                }

                //초록 UI 띠링
                return;
            }
            else if (CookTime > FinishCookTime * 0.3f)
            {
                if (!isCooking)
                {
                    Soup_Anim.SetTrigger("Cook");
                    isCooking = true;
                }
            }

        }


        else
        {
            if (BaseColor != this.renderer[0].material.GetColor("_BaseColor"))
            {
                for (int i = 0; i < this.renderer.Length; i++)
                {
                    this.renderer[i].material.SetColor("_BaseColor", BaseColor);
                }
                transform.GetChild(0).gameObject.SetActive(false);
                Ingre.Change_Ingredient(Ingre.cooking);
                Ingre = null;
                CookTime = 0f;
                isSoup = false;
                gameObject.name = "Pot";
            }
            
        }
    }

    private void FixedUpdate()
    {
        float Timerange = CookTime * 0.06f;
        if (Timerange > 0.11 && Timerange < 0.89f)
        {
            Color alphaColor = FalloffTexture.GetPixelBilinear(Timerange, 0.5f);
            float alpha = alphaColor.a;
            ChangeSoupMaterial(alpha);
        }
    }
    private void ChangeSoupMaterial(float alpha)
    {
        Color newColor = BaseColor * alpha;
        for (int i = 0; i < this.renderer.Length; i++)
        {
            this.renderer[i].material.SetColor("_BaseColor", newColor);
        }

    }

}

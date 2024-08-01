using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookingtool
{
    [SerializeField] private Renderer[] renderer;
    [SerializeField] private Texture2D FalloffTexture;

    [SerializeField] private Animator Soup_Anim;

    private Transform SaveRange = null;//가스레인지에서 떨어진 후 해당 가스레인지를 끄기위해
    private Ingredient Ingre = null;//냄비가 들고있는 재료

    public float CookTime;
    public readonly float FinishCookTime = 6f;
    //public readonly float OverCookTime = 6f;
    //public readonly float FireTime = 10f;
    private Color BaseColor;

    private bool isSoup;

    private void Awake()
    {
        BaseColor = this.renderer[0].material.GetColor("_BaseColor");
        isSoup = false;
        CookTime = 0;

    }
    private void Update()
    {

        if(transform.childCount.Equals(2))
        {

            if(transform.parent.CompareTag("GasRange"))
            {
                
                if (!transform.parent.GetChild(0).gameObject.activeSelf) // fire
                {
                    SaveRange = transform.parent;
                    transform.parent.GetChild(0).gameObject.SetActive(true);
                    transform.GetChild(1).TryGetComponent(out Ingre);
                    gameObject.name = transform.GetChild(1).name;
                }
                
                CookTime += Time.deltaTime;
                Debug.Log(CookTime);
                if (CookTime > FinishCookTime)
                {
                    if(isSoup)
                    {
                        Ingre.SetReadyCook();
                        Soup_Anim.SetTrigger("Cook");
                        isSoup = false;
                        Debug.Log("조리끝");
                    }
                    
                    //초록 UI 띠링
                    return;
                }
                else if(CookTime>FinishCookTime*0.3f)
                {
                    if(!isSoup)
                    {
                        Soup_Anim.SetTrigger("Cook");
                        isSoup = true;
                    }
                }

                return;
            }
            else
            {
                if (SaveRange.GetChild(0).gameObject.activeSelf) // fire
                    SaveRange.GetChild(0).gameObject.SetActive(false);
                SaveRange = null;
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



    public override bool CookedCheck(GameObject obj)
    {
        if (obj.TryGetComponent(out Ingredient ingre))
        {
            Debug.Log(this.name + " 재료확인");
        }
        return false;
    }

    public override void StartCook()
    {
        base.StartCook();
    }

}

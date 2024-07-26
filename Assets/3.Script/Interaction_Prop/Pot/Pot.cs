using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    [SerializeField] private Renderer[] renderer;
    [SerializeField] private Texture2D FalloffTexture;

    [SerializeField] private Animator Soup_Anim;

    private Transform SaveRange = null;//∞°Ω∫∑π¿Œ¡ˆø°º≠ ∂≥æÓ¡¯ »ƒ «ÿ¥Á ∞°Ω∫∑π¿Œ¡ˆ∏¶ ≤Ù±‚¿ß«ÿ
    private Ingredient Ingre = null;//≥ø∫Ò∞° µÈ∞Ì¿÷¥¬ ¿Á∑·

    public float CookTime;
    public readonly float FinishCookTime = 4f;
    public readonly float OverCookTime = 6f;
    public readonly float FireTime = 10f;
    private Color BaseColor;

    private bool isSoup;

    private void Awake()
    {
        BaseColor = renderer[0].material.GetColor("_BaseColor");
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
                if(CookTime > FireTime)
                {
                    if(!isSoup)
                    {
                        Soup_Anim.SetTrigger("Cook");
                        Ingre.cooking = eCooked.trash;
                        isSoup = true;
                        Debug.Log("Fire");
                    }
                    //∫“≥≤
                    return;
                }
                else if(CookTime > OverCookTime)
                {
                    if (isSoup)
                    {
                        isSoup = false;
                        Debug.Log("±Ù∫˝±Ù∫˝");
                    }
                    //ª°∞£ UI±Ù∫˝±Ù∫˝  
                    return;
                }
                else if (CookTime > FinishCookTime)
                {
                    if(!isSoup)
                    {
                        Ingre.cooking = eCooked.ReadyCook;
                        Soup_Anim.SetTrigger("Cook");
                        isSoup = true;
                        Debug.Log("¡∂∏Æ≥°");
                    }
                    
                    //√ ∑œ UI ∂Ï∏µ
                    return;
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
            if (BaseColor != renderer[0].material.GetColor("_BaseColor"))
            {
                for (int i = 0; i < renderer.Length; i++)
                {
                    renderer[i].material.SetColor("_BaseColor", BaseColor);
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
        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].material.SetColor("_BaseColor", newColor);
        }

    }




}

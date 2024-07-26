using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : Cookingtool
{
    [SerializeField] private SkinnedMeshRenderer[] renderer;
    [SerializeField] private Texture2D FalloffTexture;

    [SerializeField] private Animator Soup_Anim;

    private Transform SaveRange = null;//가스레인지에서 떨어진 후 해당 가스레인지를 끄기위해
    private Ingredient Ingre = null;//냄비가 들고있는 재료

    private float CookTime;
    private readonly float FinishCookTime = 4f;
    private readonly float OverCookTime = 6f;
    private readonly float FireTime = 10f;
    private Color BaseColor;

    private void Awake()
    {
        BaseColor = renderer[0].material.GetColor("_BaseColor");
        
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
                }
                
                CookTime += Time.deltaTime;
                if(CookTime > FireTime)
                {
                    Ingre.cooking = eCooked.trash;
                    //불남
                }
                else if(CookTime > OverCookTime)
                {
                    //빨간 UI깜빡깜빡
                }
                else if (CookTime > FinishCookTime)
                {
                    Ingre.cooking = eCooked.ReadyCook;
                    //초록 UI 띠링
                }


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
            }
            
        }
    }

    private void FixedUpdate()
    {
        float Timerange = CookTime * 0.1f;
        if (Timerange > 0.11 && Timerange < 0.89f)
        {
            float alpha = FalloffTexture.GetPixelBilinear(Timerange, 0.5f).a;
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

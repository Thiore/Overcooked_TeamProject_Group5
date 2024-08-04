using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : Cookingtool
{

    private Ingredient ingre = null;
    public float CookTime;
    public readonly float FinishCookTime = 4f;
    public readonly float OverCookTime = 6f;
    public readonly float FireTime = 10f;

   

    //public override bool CookedCheck(GameObject obj)
    //{
    //    if(obj.TryGetComponent(out Ingredient ingre))
    //    {
    //        if(ingre.cooking.Equals(eCooked.Chopping))
    //        {
    //            ingre.transform.SetParent(this.transform);
    //            ingre.transform.position = this.transform.position;
    //            ingre.transform.rotation = this.transform.rotation;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //        Debug.Log(this.name + " 재료확인");

    //    }
    //    return false;
    //}

    //public override void StartCook()
    //{
    //    base.StartCook();
    //}

}

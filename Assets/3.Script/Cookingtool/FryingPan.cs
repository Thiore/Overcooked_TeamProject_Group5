using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : Cookingtool
{
    private void Awake()
    {
        this.ingredients = new GameObject[1];
        this.availableList = new List<GameObject>();
    }

    public override bool CookedCheck(GameObject obj)
    {
        if(obj.TryGetComponent(out Ingredient ingre))
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

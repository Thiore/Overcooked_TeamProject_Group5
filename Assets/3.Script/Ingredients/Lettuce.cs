using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lettuce : Ingredient
{
    private Animator Anim;

    private int LastTime;

    protected override void Awake()
    {
        TryGetComponent(out Anim);
        Chop_Anim = true;


        LastTime = 0;
    }
    protected override void OnEnable()
    {
        ChopTime = 0;
        isChop = false;

        cooking = eCooked.Normal;


    }

    public override void Change_Ingredient(eCooked cooked)
    {


    }

    protected override void ChildChopAnim(float chopTime)
    {
        if (ChopTime > LastTime)
        {
            LastTime = Mathf.CeilToInt(ChopTime);
            Anim.SetTrigger("Chop");
        }
        if (LastTime > 7f)
        {
            LastTime = 0;
        }
    }
}

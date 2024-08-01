using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rice : Ingredient
{
    protected override void Awake()
    {
       
    }

    protected override void OnEnable()
    {
        ChopTime = 0;
        isCook = false;

        Change_Ingredient(eCooked.Normal);
    }
}

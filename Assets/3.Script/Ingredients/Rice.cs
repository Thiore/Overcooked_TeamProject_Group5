using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rice : Ingredient
{
  

    protected override void OnEnable()
    {
        ChopTime = 0;
        isCook = false;

        Change_Ingredient(eCooked.Normal);
    }

    public override void Change_Ingredient(eCooked cooked)
    {
        Ingredient_Mesh.mesh = Change_Mesh[0];
        Ingredient_renderer.material = Change_Material[0];
        Ingredient_Col.sharedMesh = Change_Mesh[0];
    }
}

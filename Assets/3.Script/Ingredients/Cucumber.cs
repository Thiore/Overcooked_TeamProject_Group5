using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Ingredient
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
            Anim.SetInteger("ChopCount", LastTime);
        }
        if(LastTime>7f)
        {
            LastTime = 0;
        }
    }

    

    

    //public override void Change_PlateIngredient()
    //{
    //    OnPlate = true;

    //    if(ChopIngre_renderer.sharedMesh != null)
    //    {
    //        ChopIngre_renderer.sharedMesh = null;
    //        ChopIngre_Col.enabled = false;
    //    }

    //    Ingredient_Mesh.mesh = Plate_Mesh;
    //    Ingredient_renderer.material = Plate_Material;
    //    Ingredient_Col.sharedMesh = Plate_Mesh;
    //}

    
}

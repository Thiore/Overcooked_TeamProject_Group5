using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate_spawn : MonoBehaviour
{
    [SerializeField] private Material crate;
    [SerializeField] private Material[] crate_Material;
    [SerializeField] private Crate_Data crate_Data;
    [SerializeField] private GameObject crate_Prefabs;


    private void Awake()
    {
        Debug.Log(crate_Data.Info.Length);
        for (int i = 0; i < crate_Data.Info.Length;i++)
        {
            Crate_Tex_Offset(crate_Data.Info[i], i);
        }
    }

    private void Crate_Tex_Offset(Crate_Info info, int num)
    {
        for(int i = 0; i < 5;i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(info.Ingredients.Equals((eIngredients)(i*5 + j)))
                {
                    
                    crate_Material[num].mainTextureOffset = new Vector2(0.2f * j, 1 - 0.2f * (i+1));
                    Debug.Log(info.Ingredients.ToString());
                    GameObject obj = Instantiate(crate_Prefabs);
                    obj.transform.position = info.Position;
                    obj.transform.rotation = info.Rotation;
                    Debug.Log(obj.name);
                    Renderer crate_renderer = obj.GetComponent<Renderer>();
                    Material[] newMat = new Material[2];
                    newMat[0] = crate;
                    newMat[1] = crate_Material[num];
                    crate_renderer.materials = newMat;
                    obj.GetComponent<spawn_Ingredient>().Set_IngredientInfo(info);
                    
                    return;
                }
            }
        }
    }

}

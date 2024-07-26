using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Ingredient
{
    [SerializeField] private Mesh[] Change_Mesh;
    [SerializeField] private Material[] Change_Material;


    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;
    private MeshCollider Ingredient_Col;

    private void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
        TryGetComponent(out Ingredient_Col);
        
        for (int i = 0; i < playerAnim.Length; i++)
        {
            playerAnim[i] = null;
        }
        cooking = eCooked.Normal;
    }
}

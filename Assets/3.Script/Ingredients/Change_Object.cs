using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Change_Object : MonoBehaviour
{
    [SerializeField] private Mesh Change_Mesh;
    [SerializeField] private Material Change_Material;
    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;

    private bool isChange;

    private void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
        //Ingredient_Mesh.
    }

    private void OnEnable()
    {
        isChange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Cook_Ingredient();
        }
    }

    public void Cook_Ingredient()
    {
        Ingredient_Mesh.mesh = Change_Mesh;
        Ingredient_renderer.material = Change_Material;
        
    }
}

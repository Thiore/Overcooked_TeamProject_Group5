using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Crate_Data Ingredients_Info;
    [SerializeField] private Ingredient[] IngrePrefabs;
    [SerializeField] private AddIngredient[] AddIngrePrefabs;

    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private Mesh[] Plate_Mesh;
    [SerializeField] private Material[] Plate_Materials;

    private MeshFilter mesh;
    private Renderer renderer;

    public bool isWash { get; private set; }

    private List<Recipe> recipes;


    private void Awake()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
        TryGetComponent(out mesh);
        TryGetComponent(out renderer);
    }

    private void OnEnable()
    {
        Change_Plate(isWash);
    }

    private void Update()
    {
        if(!transform.childCount.Equals(0))
        {
            gameObject.name = transform.GetChild(0).name;
        }
    }
    public void SetWash(bool isWash)
    {
        this.isWash = isWash;
    }

    private void Change_Plate(bool isWash)
    {
        if (!isWash)
        {
            mesh.mesh = Plate_Mesh[0];
            renderer.material = Plate_Materials[0];
        }
        else
        {
            mesh.mesh = Plate_Mesh[1];
            renderer.material = Plate_Materials[1];
        }
    }
}

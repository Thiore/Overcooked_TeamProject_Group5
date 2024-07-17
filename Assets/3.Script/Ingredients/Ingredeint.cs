using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eIngredients
{
    Tomato = 0,
    Onion,
    Potato,
    Dough,
    Seaweed,
    Mushroom,
    Meat,
    Lettuce,
    Rice,
    Flour,
    Bun,
    Fish,
    Pepperoni,
    Egg,
    Chicken,
    Tortilla,
    Cheese,
    Carrot,
    Chocolate,
    Honey,
    Pasta,
    Prawn,
    Cucumber
}

public enum eCooked
{
    Normal = 0,
    Chopping,
    Cooking
}

public class Ingredeint : MonoBehaviour
{
    public spawn_Ingredient crate;

    [SerializeField] private Mesh[] Change_Mesh;
    [SerializeField] private Material[] Change_Material;

    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;

    public eCooked cooking;
    public eIngredients myIngredients;


    private int TestNum = 0;

    private void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
    }
    private void OnEnable()
    {
        cooking = eCooked.Normal;


        Change_Ingredient(eCooked.Normal);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            TestNum++;
            if(TestNum>1)
            {
                TestNum = 0;
            }
            Change_Ingredient((eCooked)TestNum);
        }
    }

    public void Change_Ingredient(eCooked cooked)
    {
        int CookEnum = (int)cooked;
        
        Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        Ingredient_renderer.material = Change_Material[CookEnum];
        Debug.Log(Ingredient_renderer.material);
        Debug.Log(Change_Material[CookEnum]);
    }

    public void Die()
    {
        gameObject.SetActive(false);
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        crate.DestroyIngredient(this);
    }
}

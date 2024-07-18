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

public enum eCookingProcess
{
    Normal = 0,
    Chopping,
    Chop_Cook,
    Chop_Cook_Cook,
    Cook_Cook
}

public enum eCooked
{
    Normal = 0,
    Chopping,
    Cooking1,
    Cooking2
}

public class Ingredeint : MonoBehaviour
{
    public spawn_Ingredient crate;

    [SerializeField] private Mesh[] Change_Mesh;
    [SerializeField] private Material[] Change_Material;

    [SerializeField] private Transform[] CotrolBone;
    [SerializeField] private Transform[] JointBone;
    private Transform[] CopyBone;


    private Renderer Ingredient_renderer;
    private MeshFilter Ingredient_Mesh;

    public eCooked cooking { get; private set; }
    public eIngredients myIngredients;
    private eCookingProcess CookProcess;

    [SerializeField] private bool Chop_Script;
    private bool Chop_Anim = false;

    private float ChopTime;
    [SerializeField]private float FinishChopTime = default;


    private int TestNum = 0;

    private void Awake()
    {
        TryGetComponent(out Ingredient_renderer);
        TryGetComponent(out Ingredient_Mesh);
        if(JointBone.Length.Equals(0))
        {
            CopyBone = JointBone;
        }
        if (FinishChopTime.Equals(default))
        {
            FinishChopTime = 5f;
        }
    }
    private void OnEnable()
    {
        cooking = eCooked.Normal;
        ChopTime = 0;


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

    public void SetCookProcess(eCookingProcess process, bool Anim)
    {
        CookProcess = process;
        Chop_Anim = Anim;
    }

    public void Die()
    {
        gameObject.SetActive(false);
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        crate.DestroyIngredient(this);
    }
}

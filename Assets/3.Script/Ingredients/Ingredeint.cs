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
    Cucumber,
    Egg,
    Chicken,
    Tortilla,
    Cheese,
    Carrot,
    Chocolate,
    Honey,
    Pasta,
    Prawn
}

public enum eCookingProcess
{
    Normal = 0,
    Chopping,
    Chop_Cook,
    Cook
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
   // public eIngredients myIngredients;
    private eCookingProcess CookProcess;

    [SerializeField] private bool Chop_Script;
    private bool Chop_Anim = false;

    private float ChopTime;
    [SerializeField]private float FinishChopTime = default;

    private bool isChopping;

    private GameObject player;



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
        isChopping = false;
        ChopTime = 0;

        if(Ingredient_Mesh.mesh.Equals(Change_Mesh[0]))
        {
            Change_Ingredient(eCooked.Normal);
            Debug.Log("들어오면안됨");
        }
       
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.I)&&cooking.Equals(eCooked.Normal))
        {
            ChopTime += Time.deltaTime;
            Debug.Log($"잘리는중{ChopTime}");
            if(ChopTime >FinishChopTime)
            {
                ChopTime = 0;
                Change_Ingredient(eCooked.Chopping);
            }
            
        }
    }

    public void Change_Ingredient(eCooked cooked)
    {
        cooking = cooked;
        int CookEnum = (int)cooked;
        
        Ingredient_Mesh.mesh = Change_Mesh[CookEnum];
        Ingredient_renderer.material = Change_Material[CookEnum];
        
    }

    public void SetCookProcess(eCookingProcess process, bool Anim)
    {
        CookProcess = process;
        Chop_Anim = Anim;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            player = other.gameObject;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(player)
    }

    public void Die()
    {
        gameObject.SetActive(false);
        transform.position = crate.transform.position;
        transform.localScale = new Vector3(1f, 1f, 1f);
        crate.DestroyIngredient(this);
    }
}

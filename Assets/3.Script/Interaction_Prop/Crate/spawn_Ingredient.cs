using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_Ingredient : MonoBehaviour
{
    private eIngredients Ingredient_enum;

    [SerializeField] private Ingredient[] ingredient_prefab;
    private Ingredient myIngredient;
    
    private Queue<Ingredient> ingredient_queue = new Queue<Ingredient>();
    
    //private Player_StateController player;

    private Animator anim;

    public Crate_Data Data { get; private set; }
    private Crate_Info info;

    private void Awake()
    {
        TryGetComponent(out anim);
        
    }
    private void Start()
    {
        Ingredient_enum = info.Ingredients;

        //myIngredient = ingredient_prefab[0];
        for (int i = 0; i < ingredient_prefab.Length; i++)
        {
            if (Ingredient_enum.ToString().Equals(ingredient_prefab[i].name))
            {
                myIngredient = ingredient_prefab[i];
                return;
            }

        }
    }

    public GameObject PickAnim()
    {
        anim.SetTrigger("Pick");
        return newIngredient();
    }


    public GameObject newIngredient()
    {
        if (ingredient_queue.Count > 0)
        {
            Ingredient obj = ingredient_queue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj.gameObject;
        }
        else
        {
            Ingredient newobj = Instantiate(myIngredient);
            newobj.gameObject.SetActive(false);
            newobj.SetCookProcess(info);
            if (info.utensils.Equals(eCookutensils.Normal))
                newobj.name = myIngredient.name;
            else
                newobj.name = myIngredient.name + info.utensils.ToString();

            newobj.crate = this;

            return newobj.gameObject;
        }


    }
    
    public void DestroyIngredient(Ingredient ingredient)
    {
        ingredient_queue.Enqueue(ingredient);
    }

    public void Set_IngredientInfo(Crate_Info info, Crate_Data data)
    {
        this.info = info;
        this.Data = data;
    }
    

}

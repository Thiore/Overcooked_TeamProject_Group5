using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_Ingredient : MonoBehaviour
{
    private eIngredients Ingredient_enum;

    [SerializeField] private GameObject[] ingredient_prefab;

    private GameObject myIngredient;
    private Queue<Ingredient> ingredient_queue = new Queue<Ingredient>();
    private Player_StateController player;

    private Animator anim;


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
        Debug.Log(ingredient_queue.Count);
        if(ingredient_queue.Count>0)
        {
            Ingredient obj = ingredient_queue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj.gameObject;
        }
        else
        {
            Ingredient newobj = Instantiate(myIngredient.GetComponent<Ingredient>());
            newobj.SetCookProcess(info.CookingProcess, info.Chop_Anim,info.Ingredients);
            newobj.crate = this;
            return newobj.gameObject;
        }
    }
    public void DestroyIngredient(Ingredient ingredient)
    {
        ingredient_queue.Enqueue(ingredient);
        Debug.Log(ingredient_queue.Count);
    }

    public void Set_IngredientInfo(Crate_Info info)
    {
        this.info = info;
    }

}

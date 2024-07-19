using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_Ingredient : MonoBehaviour
{
    private eIngredients Ingredient_enum;

    [SerializeField] private GameObject[] ingredient_prefab;

    private GameObject myIngredient;
    private Queue<Ingredeint> ingredient_queue = new Queue<Ingredeint>();
    private Player_StateController player;

    private Animator anim;

    private bool isOpen = false;

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
        isOpen = false;
        return newIngredient();
    }


    public GameObject newIngredient()
    {
        Debug.Log(ingredient_queue.Count);
        if(ingredient_queue.Count>0)
        {
            Ingredeint obj = ingredient_queue.Dequeue();
            obj.gameObject.SetActive(true);
            return obj.gameObject;
        }
        else
        {
            Ingredeint newobj = Instantiate(myIngredient.GetComponent<Ingredeint>());
            newobj.SetCookProcess(info.CookingProcess, info.Chop_Anim);

            return newobj.gameObject;
        }
    }
  

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("µé¾î¿È");
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            isOpen = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        
        isOpen = false;
    }

    public void DestroyIngredient(Ingredeint ingredeint)
    {
        ingredient_queue.Enqueue(ingredeint);
    }

    public void Set_IngredientInfo(Crate_Info info)
    {
        this.info = info;
    }

}

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
        Ingredient_enum = info.Ingredients;

        myIngredient = ingredient_prefab[0];
        //for (int i = 0; i <ingredient_prefab.Length;i++)
        //{
        //    if(Ingredient_enum.ToString().Equals(ingredient_prefab[i].name))
        //    {
        //        myIngredient = ingredient_prefab[i];
        //        return;
        //    }

        //}
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.O)&&isOpen)
        {
            PickAnim();
            
        }
    }
    public void PickAnim()
    {
        anim.SetTrigger("Pick");
        isOpen = false;
        newIngredient();
    }


    public void newIngredient()
    {
        Debug.Log(ingredient_queue.Count);
        if(ingredient_queue.Count>0)
        {
            Ingredeint obj = ingredient_queue.Dequeue();
            obj.gameObject.SetActive(true);
            //player.TakeIngredients(obj.gameObject);
        }
        else
        {
            Ingredeint newobj = Instantiate(myIngredient.GetComponent<Ingredeint>());
            newobj.SetCookProcess(info.CookingProcess, info.Chop_Anim);
           
            //player.TakeIngredients(newobj.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("µé¾î¿È");
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            player = other.gameObject.GetComponent<Player_StateController>();
            if(player == null)
            {
                Debug.Log("¾Èµé¾î¿È?");
            }
            //if (!player.isBellow)
            //{
            //    isOpen = true;
            //}
        }
    }
    private void OnTriggerExit(Collider other)
    {
        player = null;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_Ingredient : MonoBehaviour
{
    private eIngredients Ingredient_enum;

    [SerializeField] private Ingredient[] ingredient_prefab;

    private Ingredient myIngredient;
    
    private Queue<Ingredient> ingredient_queue = new Queue<Ingredient>();
    private Queue<GameObject> Recipe_queue;
    //private Player_StateController player;

    private Animator anim;

    private Crate_Data Data;
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
            GameObject obj = Recipe_queue.Dequeue();
            ChildIngredient(obj.transform.GetChild(1));
            obj.name = transform.GetChild(1).GetChild(0).name;
            obj.gameObject.SetActive(true);
            return obj.gameObject;
        }
        else
        {
            GameObject newobj = new GameObject();
            AddIngredient AddIn = newobj.AddComponent<AddIngredient>();
            AddIn.info = Data.Info;
            GameObject AddObj = new GameObject("AddObj");
            AddObj.transform.SetParent(newobj.transform);
            AddObj.transform.position = newobj.transform.position;
            GameObject IngreList = new GameObject("IngreList");
            IngreList.transform.position = newobj.transform.position;
            ChildIngredient(IngreList.transform);
            newobj.name = IngreList.transform.GetChild(0).name;
            AddIn.crate = this;
            return newobj.gameObject;
        }

        
    }
    private Transform ChildIngredient(Transform parent)
    {
        if (ingredient_queue.Count > 0)
        {
            Ingredient obj = ingredient_queue.Dequeue();
            obj.transform.SetParent(parent);
            obj.transform.position = parent.position;
            obj.transform.rotation = Quaternion.identity;
            obj.gameObject.SetActive(true);
            return obj.transform;
        }
        else
        {
            Ingredient newobj = Instantiate(myIngredient,parent.position,parent.rotation,parent);
            newobj.SetCookProcess(info);
            if (info.utensils.Equals(eCookutensils.Normal))
                newobj.name = myIngredient.name;
            else
                newobj.name = myIngredient.name + info.utensils.ToString();
                newobj.crate = this;

            return newobj.transform;
        }
    }
    public void DestroyAdd(GameObject addIngredient)
    {
        if(!addIngredient.transform.GetChild(1).childCount.Equals(0))
        {
            for(int i = transform.GetChild(1).childCount -1;i<0;i--)
            {
                addIngredient.transform.GetChild(1).GetChild(i).GetComponent<Ingredient>().Die();
            }
        }
        for(int i = 0; i < addIngredient.transform.GetChild(0).childCount;i++)
        {
            if(addIngredient.transform.GetChild(0).GetChild(i).gameObject.activeSelf)
            {
                addIngredient.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
        }
        
        Recipe_queue.Enqueue(addIngredient);
    }
    public void DestroyIngredient(Ingredient ingredient)
    {
        ingredient_queue.Enqueue(ingredient);
    }

    public void Set_IngredientInfo(Crate_Info info, Crate_Data data, Queue<GameObject> recipe)
    {
        this.info = info;
        this.Data = data;
        this.Recipe_queue = recipe;
    }
    

}

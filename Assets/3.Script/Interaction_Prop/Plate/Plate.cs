using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Crate_Data Ingredients_Info;
    [SerializeField] private Ingredient[] IngrePrefabs;
    [SerializeField] private AddIngredient[] AddIngrePrefabs;

    [SerializeField] private Transform[] Sink_Pos;
    [SerializeField] private MeshFilter[] Plate_Mesh;
    [SerializeField] private Material[] materials;


    public bool isWash { get; private set; }

    private List<Recipe> recipes;
    private List<eIngredients> Ingre_List = new List<eIngredients>();
    private Dictionary<string, List<string>> Recipe_Ingre_Dic = new Dictionary<string, List<string>>();
    private List<string> Recipe = new List<string>();


    private void Awake()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
        
      
            
        
        for (int i = 0; i<Ingredients_Info.Info.Length;i++)
        {
            for(int j = 0; j < AddIngrePrefabs.Length;j++)
            {
                if(AddIngrePrefabs[j].name.Contains(Ingredients_Info.Info[i].Ingredients.ToString()))
                {
                    AddIngredient obj = Instantiate(AddIngrePrefabs[j]);
                    obj.transform.SetParent(transform.GetChild(0));
                    obj.gameObject.SetActive(false);
                }
            }
            for(int j = 0; j<IngrePrefabs.Length;j++)
            {
                if(IngrePrefabs[j].name.Contains(Ingredients_Info.Info[i].Ingredients.ToString()))
                {
                    Ingredient obj = Instantiate(IngrePrefabs[j]);
                    obj.transform.SetParent(transform.GetChild(1));
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        
    }
    public void SetWash(bool isWash)
    {
        this.isWash = isWash;
    }

    public bool SetIngredient(Ingredient Ingredient)
    {
        if(Ingre_List.Count.Equals(0))
        {
            for(int i = 0; i < recipes.Count;i++)
            {
                for(int j = 0; j < recipes[i].ingredient.Count;j++)
                {
                    Recipe_Ingre_Dic.Add(recipes[i].recipe, recipes[i].ingredient);
                    Recipe.Add(recipes[i].recipe);
                    if(Ingre_List.Count.Equals(0))
                    {
                        Ingre_List.Add(Ingredient.myIngredients);
                        for (int k = 0; k < transform.GetChild(1).childCount; k++)
                        {
                            if(transform.GetChild(1).GetChild(k).name.Equals(Ingredient.name))
                            {
                                transform.GetChild(1).GetChild(k).gameObject.SetActive(true);
                            }
                        }
                    }
                        
                }
            }
        }

        if(Recipe.Count>1)
        {

        }
        else
        {
            if(Ingre_List.Contains(Ingredient.myIngredients))
            {

            }
        }

        //for(int i = 0; i < Recipe_Ingre_List.Length;i++)
        //{
        //    if (Recipe_Ingre_List[i].Contains(Ingredient.myIngredients))
        //    {
        //        return false;
        //    }
        //}
        
        //for (int i = 0; i < recipes.Count;i++)
        //{
        //    for(int j = 0; j < recipes[i].ingredient.Count;j++)
        //    {
                
                
        //    }
        //}



        //for(int i = 0; i < Ingredients_List.Count;i++)
        //{
            
        //    Ingredients_List[i]
        //}
        //for (int i = 0; i < transform.GetChild(0).childCount; i++)
        //{
        //    AddIngredient obj = transform.GetChild(0).GetChild(i).GetComponent<AddIngredient>();

        //    if (obj.gameObject.activeSelf)
        //    {
        //        if (obj.gameObject.name.Contains(Ingredient.name))
        //        {
        //            return false;
        //        }
                
        //    }
        //    else
        //    {

        //    }
            
            
        //}
        return false;
    }



}

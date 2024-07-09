using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;


public class DataManager : MonoBehaviour
{
    public Dictionary<int, Recipe> recipeData;
    private static DataManager instance;

    private void Recipe_DataManager()
    {
        recipeData = LoadRecipeFromJson();
    }

   public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Recipe_DataManager");
                    instance = obj.AddComponent<DataManager>();
                    Debug.Log(obj.name);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        Recipe_DataManager();
    }



    private Dictionary<int, Recipe> LoadRecipeFromJson()
    {
        string jsonFile = Resources.Load<TextAsset>("Data/Recipe_JDB").text;
        var recipes = JsonConvert.DeserializeObject<Recipe[]>(jsonFile).ToDictionary(x => x.id, x => x);
        //Debug.Log("Loaded " + recipes.Count + " recipes from JSON.");

        // Filter recipes for stage 1
        var stage1Recipes = recipes.Values.Where(recipe => recipe.stage == 1).ToList();

        // Select a random recipe from stage 1 recipes
        Recipe selectedRecipe = stage1Recipes[Random.Range(0, stage1Recipes.Count)];

        Debug.Log("Selected recipe: " + selectedRecipe.recipe);
        return recipes;
         

    }
   

   
}


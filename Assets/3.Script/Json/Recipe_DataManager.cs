using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public class Recipe_DataManager : MonoBehaviour
{

    private List<Recipe> recipes;
    public static Recipe_DataManager instance;

   public static Recipe_DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Recipe_DataManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("Recipe_DataManager");
                    instance = obj.AddComponent<Recipe_DataManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        
    }

    private void LoadRecipeFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Recipe_JDB");
        recipes = JsonConvert.DeserializeObject<List<Recipe>>(jsonFile.text);
    }

    //public Recipe_DataManager GetRecipe_ID(int id)
    //{
    //    return recipes.Find(recipe => recipe.id == id);
    //}
}


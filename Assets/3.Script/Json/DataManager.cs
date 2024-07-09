using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;


public class DataManager : MonoBehaviour
{
    private Dictionary<string, Recipe> recipeData;
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



    private Dictionary<string, Recipe> LoadRecipeFromJson()
    {
        string jsonFile = Resources.Load<TextAsset>("Data/Recipe_JDB").text;
       var recipes = JsonConvert.DeserializeObject<Recipe[]>(jsonFile).ToDictionary(x => x.id, x => x);
        Debug.Log("Loaded " + recipes.Count + " recipes from JSON.");
        return recipeData;
        
    }

   
}


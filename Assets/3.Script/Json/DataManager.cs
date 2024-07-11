using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public Dictionary<string, Recipe> recipeData;
    


    // 선택된 레시피와 재료들을 담는 리스트
    private List<Recipe> stageRecipe;
    
    //private List<string> stage1Ingredient;
    private void Recipe_DataManager()
    {
        LoadRecipeFromJson();
        StageRecipeData();
        GetStage1Recipe();
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
                    //Debug.Log(obj.name);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        Recipe_DataManager();
        
    }
    private void LoadStageFromJson()
    {
        string stageJsonFile = Resources.Load<TextAsset>("Data/Stage_JDB").text;
        var stages = JsonConvert.DeserializeObject<Stage[]>(stageJsonFile);
    }
    private void LoadRecipeFromJson()
    {
        string jsonFile = Resources.Load<TextAsset>("Data/Recipe_JDB").text;
        var recipes = JsonConvert.DeserializeObject<Recipe[]>(jsonFile);
        

        // recipes 배열을 Dictionary로 변환
        recipeData = recipes.ToDictionary(x => x.recipe, x => x);
        
        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");

    }
    
    public void StageRecipeData()
    {
        stageRecipe = new List<Recipe>();
        //stage1Ingredient = new List<string>();

        // 선택된 레시피 출력 및 재료 출력
        var stageRecipes = recipeData.Values.Where(recipe => recipe.recipe == "Sushi").ToList();
        Recipe selectedRecipe = stageRecipes[Random.Range(0, stageRecipes.Count)];

        foreach (var recipe in stageRecipes)
        {
            stageRecipe.Add(recipe);//레시피 추가
            //string ingredient = string.Join(",", recipe.ingredient);
            //stage1Ingredient.Add(ingredient);//해당 레시피의 재료 추가
            //Debug.Log("Selected recipe: " + recipe.recipe);
            //Debug.Log("Ingredients for " + recipe.recipe + ": " + ingredient);
        }

        //Debug.Log("Selected recipe: " + selectedRecipe.recipe);
        // 재료들을 문자열로 조합하여 출력
        //string ingredients = string.Join(", ", selectedRecipe.ingredient);
        //Debug.Log("Ingredients for " + selectedRecipe.recipe + ": " + ingredients);

        
    }
    
    public List<Recipe> GetStage1Recipe()
    {
        if(stageRecipe.Count!=0)
        {
            for(int i = 0; i < stageRecipe.Count;i++)
            {
                Debug.Log($"{stageRecipe[i].recipe} : ");
                for(int j = 0; j < stageRecipe[i].ingredient.Count;j++)
                {
                    Debug.Log(stageRecipe[i].ingredient[j]);
                }
            }
        }
        return stageRecipe;
        
    }
   
}

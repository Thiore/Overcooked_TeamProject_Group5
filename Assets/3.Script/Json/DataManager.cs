using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

//public enum stagenum_e
//{
//    sushi1 = 1,
//    sushi2,
//    airballon
//}
public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public Dictionary<int, Recipe> recipeData;

    // 선택된 레시피와 재료들을 담는 리스트
    private List<Recipe> stageRecipe;
    //private stagenum_e stagenum;


    private void Recipe_DataManager()
    {
        //stagenum = stagenum_e.sushi1;
        LoadRecipeFromJson();
        StageRecipeData(1); //스테이지 + 레시피 + 재료 데이터 부르는 메서드
        //GetStageRecipe();
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
                    DataManager obj = new GameObject("Recipe_DataManager").AddComponent<DataManager>();
                    instance = obj;
                    DontDestroyOnLoad(obj);
                }

            }
            return instance;
        }
    }

    private void Awake()
    {
        if (!Application.isPlaying)
            return;
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Recipe_DataManager();

    }
    
    private void LoadRecipeFromJson()
    {
        string jsonFile = Resources.Load<TextAsset>("Data/Recipe_JDB").text;
        var recipes = JsonConvert.DeserializeObject<Recipe[]>(jsonFile);


        // recipes 배열을 Dictionary로 변환
        recipeData = recipes.ToDictionary(x => x.id, x => x);

        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");

    }

    public void StageRecipeData(int stageNumber)
    {
        stageRecipe = new List<Recipe>();
        //stage1Ingredient = new List<string>();

        // 선택된 레시피 출력 및 재료 출력
        var stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();
        //Recipe selectedRecipe = stageRecipes[Random.Range(0, stageRecipes.Count)];
        Debug.Log(stageRecipes.Count);
        //foreach (var recipe in stageRecipes)
        //{
        //    stageRecipe.Add(recipe);//레시피 추가
        //    //string ingredient = string.Join(",", recipe.ingredient);
        //    //stage1Ingredient.Add(ingredient);//해당 레시피의 재료 추가
        //    //Debug.Log("Selected recipe: " + recipe.recipe);
        //    //Debug.Log("Ingredients for " + recipe.recipe + ": " + ingredient);
        //}

        //Debug.Log("Selected recipe: " + selectedRecipe.recipe);
        // 재료들을 문자열로 조합하여 출력
        //string ingredients = string.Join(", ", selectedRecipe.ingredient);
        //Debug.Log("Ingredients for " + selectedRecipe.recipe + ": " + ingredients);


    }

    //public List<Recipe> GetStageRecipe()
    //{
    //    if (stageRecipe.Count != 0)
    //    {
    //        for (int i = 0; i < stageRecipe.Count; i++)
    //        {
    //            Debug.Log($"{stageRecipe[i].recipe} : ");
    //            for (int j = 0; j < stageRecipe[i].ingredient.Count; j++)
    //            {
    //                Debug.Log(stageRecipe[i].ingredient[j]);
    //            }
    //        }
    //    }
    //    return stageRecipe;

    //}

}

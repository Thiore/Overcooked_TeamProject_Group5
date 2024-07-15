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
    public Dictionary<int, Stage> stageData;

    // 선택된 레시피와 재료들을 담는 리스트
    private List<Recipe> stageRecipe;

    // 선택된 스테이지 점수 담는 리스트
    private List<Stage> stageScore;

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

    private void Recipe_DataManager()
    {
        //레시피
        LoadRecipeFromJson();
        StageRecipeData(1); //스테이지 + 레시피 + 재료 데이터 부르는 메서드
        
    }

    private void Stage_DataManager()
    {
        //스코어
        LoadStageFromJson();
        //ScoreManager.Instance.InitializeScores();
        ScoreManager scoreManager = ScoreManager.Instance;
        if (scoreManager != null)
        {
            scoreManager.InitializeScores();
        }
        else
        {
            Debug.LogError("DataManager안에서 ScoreManager의 인스턴스는 null");
        }
        GetStageInformation(1);
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
        Stage_DataManager();
    }
    
    private void LoadRecipeFromJson()
    {
        string recipe_jsonFile = Resources.Load<TextAsset>("Data/Recipe_JDB").text;
        var recipes = JsonConvert.DeserializeObject<Recipe[]>(recipe_jsonFile);


        // recipes 배열을 Dictionary로 변환
        recipeData = recipes.ToDictionary(x => x.id, x => x);
        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");
    }

    public List<List<Recipe>> StageRecipeData(int stageNumber)
    {
        List<Recipe> stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();

        List<List<Recipe>> allRecipes = new List<List<Recipe>>(); // 모든 레시피를 담을 리스트 생성
        List<List<string>> allIngredients = new List<List<string>>(); // 모든 재료를 담을 리스트 생성

        foreach (var recipe in stageRecipes)
        {
            // 각 레시피를 담는 리스트에 추가
            allRecipes.Add(new List<Recipe> { recipe });

            List<string> ingredients = new List<string>();
            foreach (var ingredient in recipe.ingredient)
            {
                ingredients.Add(ingredient); // 각 레시피의 재료를 재료 리스트에 추가
            }
            allIngredients.Add(ingredients); // 각 레시피의 재료 리스트를 전체 재료 리스트에 추가
        }

        return allRecipes;
        //// 각 리스트들을 출력하여 확인
        //foreach (var recipes in allRecipes)
        //{
        //    foreach (var recipe in recipes)
        //    {
        //        Debug.Log("Recipe: " + recipe.recipe);
        //    }
        //}

        //foreach (var ingredients in allIngredients)
        //{
        //    foreach (var ingredient in ingredients)
        //    {
        //        Debug.Log("Ingredient: " + ingredient);
        //    }
        //}

        // allRecipes를 반환하여 호출한 곳에서 필요한 데이터를 사용
    }
    //public List<Recipe> StageRecipeData(int stageNumber)
    //{                       
    //    // 선택된 레시피 출력 및 재료 출력
    //    List<Recipe> stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();


    //    foreach (var recipe in stageRecipes)
    //    {
    //        foreach (var inredient in recipe.ingredient)
    //        { 

    //        }
    //    }

    //    return stageRecipes;



    //    //Recipe selectedRecipe = stageRecipes[Random.Range(0, stageRecipes.Count)];
    //    //Debug.Log(stageRecipes.Count);
    //    //foreach (var recipe in stageRecipes)
    //    //{
    //    //    stageRecipe.Add(recipe);//레시피 추가
    //    //    //string ingredient = string.Join(",", recipe.ingredient);
    //    //    //stage1Ingredient.Add(ingredient);//해당 레시피의 재료 추가
    //    //    //Debug.Log("Selected recipe: " + recipe.recipe);
    //    //    //Debug.Log("Ingredients for " + recipe.recipe + ": " + ingredient);
    //    //}

    //    //Debug.Log("Selected recipe: " + selectedRecipe.recipe);
    //    // 재료들을 문자열로 조합하여 출력
    //    //string ingredients = string.Join(", ", selectedRecipe.ingredient);
    //    //Debug.Log("Ingredients for " + selectedRecipe.recipe + ": " + ingredients);
    //}


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
    
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private void LoadStageFromJson()
    {
        string score_jsonFile = Resources.Load<TextAsset>("Data/Stage_JDB").text;
        var stages = JsonConvert.DeserializeObject<Stage[]>(score_jsonFile);

        stageData = stages.ToDictionary(x => x.stage, x => x);
    }

    public void StageScoreData(int stageNumber)
    {
        stageScore = new List<Stage>();

        var stageScores = stageData.Values.Where(stage => stage.stage == stageNumber).ToList();


    }

    public Stage GetStageInformation(int stage)
    {
        if (stageData.ContainsKey(stage))
        {
            return stageData[stage];
        }

        else
        {
            Debug.LogError($"Stage {stage} 정보 못 불러왔으요");
            return null;
        }
    }

}

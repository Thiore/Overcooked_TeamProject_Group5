using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;


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
        GetAllStageData();
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

    public List<Recipe> StageRecipeData(int stageNumber)
    {
        List<Recipe> stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();
        return stageRecipes;
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
    // 스테이지 스코어
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

    public Stage GetStageInformation(int stageNumber)
    {
        if (stageData.ContainsKey(stageNumber))
        {
            Debug.Log($"stage Number : {stageNumber}");
            for(int i = 0; i < 3; i++)
            {
                Debug.Log($"targetscore : {stageData[stageNumber].targetScore[i]}");
            }
                Debug.Log($"targetscore : {stageData[stageNumber].bestScore}");
            
            return stageData[stageNumber];
        }

        else
        {
            Debug.LogError($"Stage {stageNumber} 정보 못 불러왔으요");
            return null;
        }
    }

    public List<Stage> GetAllStageData()
    {
        return new List<Stage>(stageData.Values);
    }

    /*
    public void SceneLoad(int stage_index){
    RecipePool.SetActivatePool(datamanager.instance.getRecipe(stage_index));
    for(int i=0;i<3;i++{
    ScoreManager.Instance.Targetscre[i]=datamanager.instance.
    

     */
}

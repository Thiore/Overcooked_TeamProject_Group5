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

    // ���õ� �����ǿ� ������ ��� ����Ʈ
    private List<Recipe> stageRecipe;

    // ���õ� �������� ���� ��� ����Ʈ
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
        //������
        LoadRecipeFromJson();
        StageRecipeData(1); //�������� + ������ + ��� ������ �θ��� �޼���
        
    }

    private void Stage_DataManager()
    {
        //���ھ�
        LoadStageFromJson();
        //ScoreManager.Instance.InitializeScores();
        ScoreManager scoreManager = ScoreManager.Instance;
        if (scoreManager != null)
        {
            scoreManager.InitializeScores();
        }
        else
        {
            Debug.LogError("DataManager�ȿ��� ScoreManager�� �ν��Ͻ��� null");
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


        // recipes �迭�� Dictionary�� ��ȯ
        recipeData = recipes.ToDictionary(x => x.id, x => x);
        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");
    }

    public List<List<Recipe>> StageRecipeData(int stageNumber)
    {
        List<Recipe> stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();

        List<List<Recipe>> allRecipes = new List<List<Recipe>>(); // ��� �����Ǹ� ���� ����Ʈ ����
        List<List<string>> allIngredients = new List<List<string>>(); // ��� ��Ḧ ���� ����Ʈ ����

        foreach (var recipe in stageRecipes)
        {
            // �� �����Ǹ� ��� ����Ʈ�� �߰�
            allRecipes.Add(new List<Recipe> { recipe });

            List<string> ingredients = new List<string>();
            foreach (var ingredient in recipe.ingredient)
            {
                ingredients.Add(ingredient); // �� �������� ��Ḧ ��� ����Ʈ�� �߰�
            }
            allIngredients.Add(ingredients); // �� �������� ��� ����Ʈ�� ��ü ��� ����Ʈ�� �߰�
        }

        return allRecipes;
        //// �� ����Ʈ���� ����Ͽ� Ȯ��
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

        // allRecipes�� ��ȯ�Ͽ� ȣ���� ������ �ʿ��� �����͸� ���
    }
    //public List<Recipe> StageRecipeData(int stageNumber)
    //{                       
    //    // ���õ� ������ ��� �� ��� ���
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
    //    //    stageRecipe.Add(recipe);//������ �߰�
    //    //    //string ingredient = string.Join(",", recipe.ingredient);
    //    //    //stage1Ingredient.Add(ingredient);//�ش� �������� ��� �߰�
    //    //    //Debug.Log("Selected recipe: " + recipe.recipe);
    //    //    //Debug.Log("Ingredients for " + recipe.recipe + ": " + ingredient);
    //    //}

    //    //Debug.Log("Selected recipe: " + selectedRecipe.recipe);
    //    // ������ ���ڿ��� �����Ͽ� ���
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
    
    //�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
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
            Debug.LogError($"Stage {stage} ���� �� �ҷ�������");
            return null;
        }
    }

}

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

    // ���õ� �����ǿ� ������ ��� ����Ʈ
    private List<Recipe> stageRecipe;
    //private stagenum_e stagenum;


    private void Recipe_DataManager()
    {
        //stagenum = stagenum_e.sushi1;
        LoadRecipeFromJson();
        StageRecipeData(1); //�������� + ������ + ��� ������ �θ��� �޼���
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


        // recipes �迭�� Dictionary�� ��ȯ
        recipeData = recipes.ToDictionary(x => x.id, x => x);

        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");

    }

    public void StageRecipeData(int stageNumber)
    {
        stageRecipe = new List<Recipe>();
        //stage1Ingredient = new List<string>();

        // ���õ� ������ ��� �� ��� ���
        var stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();
        //Recipe selectedRecipe = stageRecipes[Random.Range(0, stageRecipes.Count)];
        Debug.Log(stageRecipes.Count);
        //foreach (var recipe in stageRecipes)
        //{
        //    stageRecipe.Add(recipe);//������ �߰�
        //    //string ingredient = string.Join(",", recipe.ingredient);
        //    //stage1Ingredient.Add(ingredient);//�ش� �������� ��� �߰�
        //    //Debug.Log("Selected recipe: " + recipe.recipe);
        //    //Debug.Log("Ingredients for " + recipe.recipe + ": " + ingredient);
        //}

        //Debug.Log("Selected recipe: " + selectedRecipe.recipe);
        // ������ ���ڿ��� �����Ͽ� ���
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

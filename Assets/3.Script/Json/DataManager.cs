using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;


public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public Dictionary<int, Recipe> recipeData;
    public Dictionary<int, Stage> stageData;
    public GameSaveData saveData;

    // ���õ� �����ǿ� ������ ��� ����Ʈ
    private List<Recipe> stageRecipe;

    // ���õ� �������� ���� ��� ����Ʈ
    private List<Stage> stageScore;
    private string filePath;

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
        StageRecipeData(GameManager.Instance.stage_index); //�������� + ������ + ��� ������ �θ��� �޼���

    }

    private void Stage_DataManager()
    {
        //���ھ�
        LoadStageFromJson();
        GetAllStageData();
        GetStageInformation(1);
    }

    private void Awake()
    {
        if (!Application.isPlaying)
            return;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        filePath = Path.Combine(Application.dataPath, "gamedata.json");
        LoadRecipeFromJson();

        if (!File.Exists(filePath))
        {
            Debug.Log("json ������ ����");
            
            LoadStageFromJson();
            //���Ͽ��� json���ڿ� �б�
            string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);

            //json ���ڿ��� ���Ͽ� ����
            File.WriteAllText(filePath, json);
        }
        else
        {
            LoadGame();

        }


        //Stage_DataManager();
    }

    private void LoadRecipeFromJson()
    {
        string recipe_jsonFile = Resources.Load<TextAsset>("Data/Recipe_JDB").text;
        var recipes = JsonConvert.DeserializeObject<Recipe[]>(recipe_jsonFile);

        //�丮����
        foreach (var recipe in recipes)
        {
            recipe.tool_count = recipe.ingredient.Count(ingredient => ingredient.EndsWith("Pot") || ingredient.EndsWith("Pan") || ingredient.EndsWith("Fry"));
        }
        // recipes �迭�� Dictionary�� ��ȯ
        recipeData = recipes.ToDictionary(x => x.id, x => x);
        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");
    }

    public List<Recipe> StageRecipeData(int stageNumber)
    {
        List<Recipe> stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();
        return stageRecipes;
    }

    //�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    // �������� ���ھ�
    

    public void StageScoreData(int stageNumber)
    {
        stageScore = new List<Stage>();

        var stageScores = stageData.Values.Where(stage => stage.stage == stageNumber).ToList();


    }

    public Stage GetStageInformation(int stageNumber)
    {
        if (stageData.ContainsKey(stageNumber))
        {
            //Debug.Log($"stage Number : {stageNumber}");
            //for(int i = 0; i < 3; i++)
            //{
            //    Debug.Log($"targetscore : {stageData[stageNumber].targetScore[i]}");
            //}
            //    Debug.Log($"targetscore : {stageData[stageNumber].bestScore}");

            return stageData[stageNumber];
        }

        else
        {
            Debug.LogError($"Stage {stageNumber} ���� �� �ҷ�������");
            return null;
        }
    }
    //public GameSaveData SaveData(int stageNumber)
    //{ 
    //if(saveData.)
    //}
    public List<Stage> GetAllStageData()
    {
        return new List<Stage>(stageData.Values);
    }

    //�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    // ���̺�
    

    //private void Start()
    //{
    //    //json���·� gamedata ������ �̸� ���� / ���� ��� ����
    //    //C:\Users\[Username]\AppData\LocalLow\[CompanyName]\[ProductName]
    //    filePath = Path.Combine(Application.dataPath,"gamedata.json");
        
    //}

    //���� ������ ����
    public void SaveGame()
    {
        //GameSaveData ����ü�� json ���ڿ��� ��ȯ
        string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);

        //json ���ڿ��� ���Ͽ� ����
        File.WriteAllText(filePath, json);
    }

    public void NewGame()
    {
        LoadStageFromJson();
        //���Ͽ��� json���ڿ� �б�
        string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);

        //json ���ڿ��� ���Ͽ� ����
        File.WriteAllText(filePath, json);

        LoadGame();
    }
    //json���Ͽ��� ���� ������ �ε�
    public void  LoadGame()
    {

        if (File.Exists(filePath))
        {
            // ���Ͽ��� json ���ڿ� �б�
            string json = File.ReadAllText(filePath);

            // json ���ڿ��� Dictionary<int, Stage> ����ü�� ��ȯ
            stageData = JsonConvert.DeserializeObject<Dictionary<int, Stage>>(json);
        }
        else
        {
            Debug.LogError("Save file not found.");
        }



    }

    private void LoadStageFromJson()
    {
        string score_jsonFile = Resources.Load<TextAsset>("Data/Stage_JDB").text;
        var stages = JsonConvert.DeserializeObject<Stage[]>(score_jsonFile);

        stageData = stages.ToDictionary(x => x.stage, x => x);
    }
}

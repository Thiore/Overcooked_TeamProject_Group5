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

    // 선택된 레시피와 재료들을 담는 리스트
    private List<Recipe> stageRecipe;

    // 선택된 스테이지 점수 담는 리스트
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
        //레시피
        LoadRecipeFromJson();
        StageRecipeData(GameManager.Instance.stage_index); //스테이지 + 레시피 + 재료 데이터 부르는 메서드

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
            Debug.Log("json 생기지 않음");
            
            LoadStageFromJson();
            //파일에서 json문자열 읽기
            string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);

            //json 문자열을 파일에 저장
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

        //요리도구
        foreach (var recipe in recipes)
        {
            recipe.tool_count = recipe.ingredient.Count(ingredient => ingredient.EndsWith("Pot") || ingredient.EndsWith("Pan") || ingredient.EndsWith("Fry"));
        }
        // recipes 배열을 Dictionary로 변환
        recipeData = recipes.ToDictionary(x => x.id, x => x);
        //Debug.Log("Loaded " + recipeData.Count + " recipes from JSON.");
    }

    public List<Recipe> StageRecipeData(int stageNumber)
    {
        List<Recipe> stageRecipes = recipeData.Values.Where(recipe => recipe.stage == stageNumber).ToList();
        return stageRecipes;
    }

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    // 스테이지 스코어
    

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
            Debug.LogError($"Stage {stageNumber} 정보 못 불러왔으요");
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

    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    // 세이브
    

    //private void Start()
    //{
    //    //json형태로 gamedata 파일의 이름 정의 / 파일 경로 지정
    //    //C:\Users\[Username]\AppData\LocalLow\[CompanyName]\[ProductName]
    //    filePath = Path.Combine(Application.dataPath,"gamedata.json");
        
    //}

    //게임 데이터 저장
    public void SaveGame()
    {
        //GameSaveData 구조체를 json 문자열로 변환
        string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);

        //json 문자열을 파일에 저장
        File.WriteAllText(filePath, json);
    }

    public void NewGame()
    {
        LoadStageFromJson();
        //파일에서 json문자열 읽기
        string json = JsonConvert.SerializeObject(stageData, Formatting.Indented);

        //json 문자열을 파일에 저장
        File.WriteAllText(filePath, json);

        LoadGame();
    }
    //json파일에서 게임 데이터 로드
    public void  LoadGame()
    {

        if (File.Exists(filePath))
        {
            // 파일에서 json 문자열 읽기
            string json = File.ReadAllText(filePath);

            // json 문자열을 Dictionary<int, Stage> 구조체로 변환
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

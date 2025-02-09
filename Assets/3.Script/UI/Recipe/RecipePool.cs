using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RecipePool : MonoBehaviour
{
    public GameObject oneIngredientPrefab; // 1개의 재료를 가지는 레시피 프리팹
    public GameObject twoIngredientsPrefab; // 2개의 재료를 가지는 레시피 프리팹
    public GameObject threeIngredientsPrefab; // 3개의 재료를 가지는 레시피 프리팹
    public GameObject oneIngredient_onetool_Prefab;
    public GameObject twoIngredient_onetool_Prefab;
    public GameObject twoIngredient_twotool_Prefab;
    public GameObject threeIngredient_onetool_Prefab;
    public GameObject threeIngredient_twotool_Prefab;
    [SerializeField] private RectTransform recipePoolPanel; // RECIPE_POOL 패널의 RectTransform
    [SerializeField] private float moveSpeed = 100f; // 오브젝트 이동 속도
    [SerializeField] private int maxVisibleObjects = 5; // 화면에 보일 최대 오브젝트 수

    private List<GameObject> objectPool = new List<GameObject>(); // 오브젝트 풀을 저장하는 리스트
    public List<GameObject> activeObjects = new List<GameObject>(); // 활성화된 오브젝트 목록
    //private List<RecipeObject> allObjects = new List<RecipeObject>(); // 생성된 모든 오브젝트 목록

    private float spawnInterval = 8f; // 오브젝트 생성 간격 (초)
    private float elapsedTime = 0f; // 경과 시간
    private List<Recipe> recipes; // 레시피 데이터 목록
    public List<Recipe> Recipes { get { return recipes; } }

    private void Start()
    {
        GetRecipes(); // 레시피 데이터를 가져옴
        InitializeObjectPool(); // 오브젝트 풀 초기화
        
        //ActivateObject(); // 첫 번째 오브젝트 활성화
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying) // 게임이 진행 중일 때만
        {
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트

            if (elapsedTime >= spawnInterval) // 생성 간격이 지나면
            {
                elapsedTime = 0f;
                ActivateObject(); // 새로운 오브젝트 활성화
            }
            if(activeObjects.Count.Equals(0))
            {
                elapsedTime = 0f;
                ActivateObject();
            }

            MoveActiveObjects(); // 활성화된 오브젝트 이동
        }
    }

    // 오브젝트 풀 초기화
    private void InitializeObjectPool()
    {
        for(int i = 0; i < 20; i++)
        {
            int rand = Random.Range(0, recipes.Count);
            GameObject prefab = GetPrefabForRecipe(recipes[rand]);
            GameObject obj = Instantiate(prefab, recipePoolPanel);
            SetRecipeObjectImages(obj.GetComponent<RecipeObject>(),recipes[rand]); // 각 오브젝트의 이미지를 설정
            obj.name = recipes[rand].recipe;
            //obj.TryGetComponent(out RecipeObject recipeobj);
            //recipeobj.recipe = recipes[rand];
            objectPool.Add(obj); // 풀에 추가
            //allObjects.Add(recipeobj); // 모든 오브젝트 목록에 추가
            obj.SetActive(false); // 오브젝트를 비활성화
        }
        
    }

    // 레시피에 맞는 프리팹을 반환
    private GameObject GetPrefabForRecipe(Recipe recipe)
    {
        switch (recipe.ingredient.Count)
        {
            case 1:
                if (recipe.tool_count == 1)
                {
                    return oneIngredient_onetool_Prefab;
                }
                else
                {
                    return oneIngredientPrefab;

                }
            case 2:

                if (recipe.tool_count ==1)
                {
                    return twoIngredient_onetool_Prefab;
                }else if (recipe.tool_count == 2)
                {
                    return twoIngredient_twotool_Prefab;
                }
                else
                {
                    return twoIngredientsPrefab;
                }
                
                
                
            case 3:

                if (recipe.tool_count == 1)
                {
                    return threeIngredient_onetool_Prefab;
                }
                else if (recipe.tool_count == 2)
                {
                    return threeIngredient_twotool_Prefab;
                }
                else
                {
                    return threeIngredientsPrefab;
                }
                
            default:
                Debug.LogError("Unknown number of ingredients: " + recipe.ingredient.Count);
                return null;
        }
    }

    // 활성화된 오브젝트를 왼쪽으로 이동
    private void MoveActiveObjects()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            GameObject obj = activeObjects[i];
            if (obj.activeSelf) // 오브젝트가 활성화된 경우
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime; // 왼쪽으로 이동

                // 이전 오브젝트와의 거리 유지
                if (i > 0)
                {
                    RectTransform prevRectTransform = activeObjects[i - 1].GetComponent<RectTransform>();
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width - 150 <= prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width)
                    {
                        rectTransform.anchoredPosition = new Vector2(prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width + 150, rectTransform.anchoredPosition.y);
                    }
                }
                else // 첫 번째 오브젝트는 왼쪽 끝에 위치
                {
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width / 2 <= -recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2)
                    {
                        rectTransform.anchoredPosition = new Vector2(-recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
                    }
                }

                // 오브젝트가 활성화된 지 20초가 지나면 비활성화
                if (Time.time - obj.GetComponent<RecipeObject>().activationTime >= 15f*obj.GetComponent<RecipeObject>().recipe.ingredient.Count)
                {
                    DeactivateObject(obj); // 오브젝트 비활성화
                    ScoreManager.Instance.SubScore(10); // 점수 감소
                }
            }
        }
    }

    // 새로운 오브젝트를 활성화
    private void ActivateObject()
    {
        if (objectPool.Count > 0) // 풀에 오브젝트가 남아있을 때
        {
            GameObject obj = objectPool[0]; // 리스트의 첫 번째 오브젝트 가져오기
            objectPool.RemoveAt(0); // 리스트에서 제거
            Image[] childImages = obj.GetComponentsInChildren<Image>();
            foreach (Image img in childImages)
            {
                img.color = Color.white;
            }
            obj.SetActive(true); // 오브젝트 활성화
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, 0); // 오브젝트 위치 설정

            activeObjects.Add(obj); // 활성화된 오브젝트 목록에 추가

            obj.GetComponent<RecipeObject>().activationTime = Time.time; // 활성화 시간 설정

            if (activeObjects.Count > maxVisibleObjects) // 활성화된 오브젝트가 최대치를 넘으면
            {
                GameObject oldestObj = activeObjects[0]; // 가장 오래된 오브젝트 선택
                activeObjects.RemoveAt(0); // 목록에서 제거
                DeactivateObject(oldestObj); // 오브젝트 비활성화
                
            }
        }
        //Debug.Log("레시피 활성화");
    }

    // 오브젝트 비활성화
    private void DeactivateObject(GameObject obj)
    {
        activeObjects.Remove(obj); // 활성화된 목록에서 제거
        obj.SetActive(false); // 오브젝트 비활성화
        objectPool.Add(obj); // 풀에 다시 추가
        //Debug.Log("레시피 비활성화");
    }


    //Debug.Log에 적어둔 메서드 추가 구현 필요
    public void CheckRecipe(string name)
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            if (activeObjects[i].name == name)
            {
                if (i == 0)
                {
                    AllCorrect(i, 20);
                }
                else
                {
                    InCorrect(i, 20);
                }

                return; // 매칭되는 오브젝트를 찾았으므로 메서드 종료
            }
        }
        StartCoroutine(Wrong());
    }

    private void AllCorrect(int index,int point)
    {
        // 모든 자식 Image 컴포넌트를 가져와 색상 변경
        Image[] childImages = activeObjects[index].GetComponentsInChildren<Image>();
        foreach (Image img in childImages)
        {
            img.color = Color.green;
        }

       // Debug.Log(activeObjects[index].name);
        StartCoroutine(DeactivateAfterDelay(activeObjects[index], 0.2f));
        GameManager.Instance.AllCorrect_Recipe(point);
        if (activeObjects.Count == 0)
        {
            ActivateObject();
        }
    }

    private void InCorrect(int index, int point)
    {
        // 모든 자식 Image 컴포넌트를 가져와 색상 변경
        Image[] childImages = activeObjects[index].GetComponentsInChildren<Image>();
        foreach (Image img in childImages)
        {
            img.color = Color.green;
        }
        //Debug.Log(activeObjects[index].name);
        StartCoroutine(DeactivateAfterDelay(activeObjects[index], 0.5f));
        
        GameManager.Instance.Incorrect_Recipe(point);
    }
    private IEnumerator Wrong()
    {
        for(int i = 0; i < activeObjects.Count; i++)
        {
            Image[] childImages = activeObjects[i].GetComponentsInChildren<Image>();
            foreach(Image img in childImages)
            {
                img.color = Color.red;
            }
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < activeObjects.Count; i++)
        {
            Image[] childImages = activeObjects[i].GetComponentsInChildren<Image>();
            foreach (Image img in childImages)
            {
                img.color = Color.white;
            }
        }
        GameManager.Instance.Wrong_Recipe();
    }


    // 지연 후 오브젝트 비활성화
    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateObject(obj); // 오브젝트 비활성화
    }

    // 각 오브젝트의 이미지를 설정
    private void SetRecipeObjectImages(RecipeObject objectIndex, Recipe recipe)
    {

        objectIndex.recipe = recipe;

        //objectIndex.name = objectIndex.recipe.recipe;
        Dictionary<string, string> imageNameMap = new Dictionary<string, string>
            {
                { "Recipe_Icon", objectIndex.recipe.recipe }
            };
        for (int k = 0; k < objectIndex.recipe.ingredient.Count; k++)
        {
            if (objectIndex.recipe.ingredient.Count == 1)
            {
                imageNameMap.Add("Ingredient_Icon", objectIndex.recipe.ingredient[0]);
            }
            else
            {
                imageNameMap.Add($"Ingredient_Icon_{k + 1}", objectIndex.recipe.ingredient[k]);
            }
            if (k < objectIndex.recipe.tool_count)
            {
                string tool = string.Empty;
                if (objectIndex.recipe.ingredient[k].Contains("Fry"))
                {
                    tool = "Fry";
                }
                else if (objectIndex.recipe.ingredient[k].Contains("Pot"))
                {
                    tool = "Pot";
                }
                else if (objectIndex.recipe.ingredient[k].Contains("Pan"))
                {
                    tool = "Pan";
                }
                
                //string tool = objectIndex.recipe.ingredient[k].Substring(objectIndex.recipe.ingredient[k].Length - 3);
                objectIndex.SetToolImage(k, tool);
            }
        }



        objectIndex.SetImagesByName(imageNameMap);

        //}
        //}
    }


    // 레시피 데이터를 가져오는 메서드
    private void GetRecipes()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
    }
}

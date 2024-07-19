using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RecipePool : MonoBehaviour
{
    public GameObject[] recipePrefabs; // 여러 종류의 RECIPE_OBJECT 프리팹
    [SerializeField] private RectTransform recipePoolPanel; // RECIPE_POOL 패널의 RectTransform
    [SerializeField] private float moveSpeed = 100f; // 오브젝트 이동 속도
    [SerializeField] private int maxVisibleObjects = 5; // 화면에 보일 최대 오브젝트 수

    private Queue<GameObject> objectPool = new Queue<GameObject>(); // 오브젝트 풀을 저장하는 큐
    private List<GameObject> activeObjects = new List<GameObject>(); // 활성화된 오브젝트 목록
    private List<GameObject> allObjects = new List<GameObject>(); // 생성된 모든 오브젝트 목록

    private float spawnInterval = 8f; // 오브젝트 생성 간격 (초)
    private float elapsedTime = 0f; // 경과 시간
    private List<Recipe> recipes; // 레시피 데이터 목록
    public List<Recipe> Recipes { get { return recipes; } }

    private void Start()
    {
        recipes = GetRecipes(); // 레시피 데이터를 가져옴

        // 모든 오브젝트를 초기화하고 풀에 추가
        for (int i = 0; i < recipes.Count * 3; i++)
        {
            GameObject obj = Instantiate(recipePrefabs[i % recipePrefabs.Length], recipePoolPanel);
            obj.SetActive(false); // 오브젝트를 비활성화
            objectPool.Enqueue(obj); // 풀에 추가
            allObjects.Add(obj); // 모든 오브젝트 목록에 추가
        }

        SetRecipeObjectImages(recipes); // 각 오브젝트의 이미지를 설정
        ActivateObject(); // 첫 번째 오브젝트 활성화
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

            MoveActiveObjects(); // 활성화된 오브젝트 이동
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
                if (Time.time - obj.GetComponent<RecipeObject>().activationTime >= 20f)
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
            int randomIndex = Random.Range(0, objectPool.Count); // 랜덤 인덱스 선택
            GameObject obj = objectPool.ToArray()[randomIndex]; // 랜덤 오브젝트 선택
            objectPool = new Queue<GameObject>(objectPool); // 큐 재구성
            objectPool.Dequeue(); // 선택된 오브젝트 제거
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
    }

    // 오브젝트 비활성화
    private void DeactivateObject(GameObject obj)
    {
        activeObjects.Remove(obj); // 활성화된 목록에서 제거
        obj.SetActive(false); // 오브젝트 비활성화
        objectPool.Enqueue(obj); // 풀에 다시 추가
    }

    // 정답 오브젝트 처리
    public void CorrectRecipe(GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.green; // 오브젝트 색상 변경
        int points = obj.GetComponent<RecipeObject>().Point; // 점수 가져오기
        ScoreManager.Instance.AddScore(points); // 점수 추가
        StartCoroutine(DeactivateAfterDelay(obj, 0.2f)); // 지연 후 비활성화
    }

    // 지연 후 오브젝트 비활성화
    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateObject(obj); // 오브젝트 비활성화
    }

    // 각 오브젝트의 이미지를 설정
    private void SetRecipeObjectImages(List<Recipe> recipes)
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            Recipe recipe = recipes[i]; // 레시피 데이터
            GameObject obj = allObjects[i]; // 오브젝트
            Dictionary<string, string> imageNameMap = new Dictionary<string, string>
            {
                { "Recipe_Icon", recipe.recipe } // 레시피 아이콘 설정
            };

            // 재료 아이콘 설정
            for (int j = 0; j < recipe.ingredient.Count(); j++)
            {
                imageNameMap.Add($"Ingredient_Icon_{j + 1}", recipe.ingredient[j]);
                
            }

            RecipeObject recipeObject = obj.GetComponent<RecipeObject>();
            recipeObject.SetImagesByName(imageNameMap); // 이미지 설정
        }
    }

    // 레시피 데이터를 가져오는 메서드 (더미 데이터)

    private List<Recipe> GetRecipes()
    {
        // 실제 데이터 가져오는 코드로 대체해야 함
        return new List<Recipe>
        {
            new Recipe { id = 1, stage = 1, recipe = "recipe1", ingredient = new List<string> { "ingredient1" } },
            new Recipe { id = 2, stage = 1, recipe = "recipe2", ingredient = new List<string> { "ingredient1", "ingredient2" } },
            new Recipe { id = 3, stage = 1, recipe = "recipe3", ingredient = new List<string> { "ingredient1", "ingredient2", "ingredient3" } }
        };
    }
}

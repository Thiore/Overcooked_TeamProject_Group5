using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RecipePool : MonoBehaviour
{
    public GameObject[] recipePrefabs; // 여러 종류의 RECIPE_OBJECT 프리팹
    [SerializeField] private RectTransform recipePoolPanel; // RECIPE_POOL 패널
    [SerializeField] private float moveSpeed = 100f; // 오브젝트 이동 속도
    [SerializeField] private int maxVisibleObjects = 5; // 화면에 보일 최대 오브젝트 수

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();

    private float spawnInterval = 8f; // 8초마다 생성
    private float elapsedTime = 0f;

    private void Start()
    {
        // 초기 오브젝트 풀 생성
        for (int i = 0; i < maxVisibleObjects * recipePrefabs.Length; i++)
        {
            GameObject obj = Instantiate(recipePrefabs[i % recipePrefabs.Length], recipePoolPanel);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        // 게임 시작 시 첫 번째 오브젝트 활성화
        ActivateObject();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // 8초마다 새로운 오브젝트 활성화
        if (elapsedTime >= spawnInterval)
        {
            elapsedTime = 0f;
            ActivateObject();
        }

        // 오브젝트가 오른쪽에서 왼쪽으로 이동
        for (int i = 0; i < activeObjects.Count; i++)
        {
            GameObject obj = activeObjects[i];
            if (obj.activeSelf)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;

                // 이전 오브젝트의 오른쪽에서 멈춤
                if (i > 0)
                {
                    RectTransform prevRectTransform = activeObjects[i - 1].GetComponent<RectTransform>();
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width - 150 <= prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width)
                    {
                        rectTransform.anchoredPosition = new Vector2(prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width + 150, rectTransform.anchoredPosition.y);
                    }
                }
                else
                {
                    // 첫 번째 오브젝트는 왼쪽 끝에서 멈춤
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width / 2 <= -recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2)
                    {
                        rectTransform.anchoredPosition = new Vector2(-recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
                    }
                }

                // 오브젝트가 활성화된 지 20초가 지나면 비활성화
                if (Time.time - obj.GetComponent<RecipeObject>().activationTime >= 20f)
                {
                    DeactivateObject(obj);
                    GameManager.Instance.SubScore(10);
                }
            }
        }
    }

    private void ActivateObject()
    {
        if (objectPool.Count > 0)
        {
            // 풀에서 오브젝트를 가져와 활성화
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);

            // 오브젝트의 시작 위치를 패널의 가장 오른쪽으로 설정
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, 0);

            // 활성화된 오브젝트 목록에 추가
            activeObjects.Add(obj);

            // 활성화된 오브젝트에 활성화 시간을 설정
            obj.GetComponent<RecipeObject>().activationTime = Time.time;

            // 활성화된 오브젝트 수가 최대치를 넘으면 가장 오래된 오브젝트를 비활성화
            if (activeObjects.Count > maxVisibleObjects)
            {
                GameObject oldestObj = activeObjects[0];
                activeObjects.RemoveAt(0);
                oldestObj.SetActive(false);
                objectPool.Enqueue(oldestObj);
            }

            // 자식 Image 오브젝트 설정
            SetRecipeObjectImages(obj);
        }
    }

    private void DeactivateObject(GameObject obj)
    {
        activeObjects.Remove(obj);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    public void CorrectRecipe(GameObject obj)
    {
        // obj를 초록색으로 변경
        obj.GetComponent<Image>().color = Color.green;

        // 점수 추가
        int points = obj.GetComponent<RecipeObject>().Point;
        GameManager.Instance.AddScore(points);

        // 0.2초 뒤 오브젝트 비활성화
        StartCoroutine(DeactivateAfterDelay(obj, 0.2f));
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateObject(obj);
    }

    private void SetRecipeObjectImages(GameObject obj)
    {
        // RecipeObject의 자식 Image 오브젝트 이름을 기준으로 설정
        Dictionary<string, string> imageNameMap = new Dictionary<string, string>();

        switch (obj.name)
        {
            case "One_Ingredient_Recipe(Clone)":
                imageNameMap.Add("Recipe_Icon", "image1");
                imageNameMap.Add("Ingredient_Icon", "image2");
                break;
            case "Two_Ingredients_Recipe(Clone)":
                imageNameMap.Add("Recipe_Icon", "image1");
                imageNameMap.Add("Ingredient_Icon_1", "image2");
                imageNameMap.Add("Ingredient_Icon_2", "image3");
                break;
            case "Three_Ingredients_Recipe(Clone)":
                imageNameMap.Add("Recipe_Icon", "image1");
                imageNameMap.Add("Ingredient_Icon_1", "image2");
                imageNameMap.Add("Ingredient_Icon_2", "image3");
                imageNameMap.Add("Ingredient_Icon_3", "image4");
                break;
        }

        RecipeObject recipeObject = obj.GetComponent<RecipeObject>();
        recipeObject.SetImagesByName(imageNameMap);
    }
}

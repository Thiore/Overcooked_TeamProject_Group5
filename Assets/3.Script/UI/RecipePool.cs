using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RecipePool : MonoBehaviour
{
    public GameObject[] recipePrefabs; // ���� ������ RECIPE_OBJECT ������
    [SerializeField] private RectTransform recipePoolPanel; // RECIPE_POOL �г�
    [SerializeField] private float moveSpeed = 100f; // ������Ʈ �̵� �ӵ�
    [SerializeField] private int maxVisibleObjects = 5; // ȭ�鿡 ���� �ִ� ������Ʈ ��

    private Queue<GameObject> objectPool = new Queue<GameObject>();
    private List<GameObject> activeObjects = new List<GameObject>();

    private float spawnInterval = 8f; // 8�ʸ��� ����
    private float elapsedTime = 0f;

    private void Start()
    {
        // �ʱ� ������Ʈ Ǯ ����
        for (int i = 0; i < maxVisibleObjects * recipePrefabs.Length; i++)
        {
            GameObject obj = Instantiate(recipePrefabs[i % recipePrefabs.Length], recipePoolPanel);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        // ���� ���� �� ù ��° ������Ʈ Ȱ��ȭ
        ActivateObject();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // 8�ʸ��� ���ο� ������Ʈ Ȱ��ȭ
        if (elapsedTime >= spawnInterval)
        {
            elapsedTime = 0f;
            ActivateObject();
        }

        // ������Ʈ�� �����ʿ��� �������� �̵�
        for (int i = 0; i < activeObjects.Count; i++)
        {
            GameObject obj = activeObjects[i];
            if (obj.activeSelf)
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime;

                // ���� ������Ʈ�� �����ʿ��� ����
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
                    // ù ��° ������Ʈ�� ���� ������ ����
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width / 2 <= -recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2)
                    {
                        rectTransform.anchoredPosition = new Vector2(-recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
                    }
                }

                // ������Ʈ�� Ȱ��ȭ�� �� 20�ʰ� ������ ��Ȱ��ȭ
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
            // Ǯ���� ������Ʈ�� ������ Ȱ��ȭ
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);

            // ������Ʈ�� ���� ��ġ�� �г��� ���� ���������� ����
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, 0);

            // Ȱ��ȭ�� ������Ʈ ��Ͽ� �߰�
            activeObjects.Add(obj);

            // Ȱ��ȭ�� ������Ʈ�� Ȱ��ȭ �ð��� ����
            obj.GetComponent<RecipeObject>().activationTime = Time.time;

            // Ȱ��ȭ�� ������Ʈ ���� �ִ�ġ�� ������ ���� ������ ������Ʈ�� ��Ȱ��ȭ
            if (activeObjects.Count > maxVisibleObjects)
            {
                GameObject oldestObj = activeObjects[0];
                activeObjects.RemoveAt(0);
                oldestObj.SetActive(false);
                objectPool.Enqueue(oldestObj);
            }

            // �ڽ� Image ������Ʈ ����
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
        // obj�� �ʷϻ����� ����
        obj.GetComponent<Image>().color = Color.green;

        // ���� �߰�
        int points = obj.GetComponent<RecipeObject>().Point;
        GameManager.Instance.AddScore(points);

        // 0.2�� �� ������Ʈ ��Ȱ��ȭ
        StartCoroutine(DeactivateAfterDelay(obj, 0.2f));
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateObject(obj);
    }

    private void SetRecipeObjectImages(GameObject obj)
    {
        // RecipeObject�� �ڽ� Image ������Ʈ �̸��� �������� ����
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

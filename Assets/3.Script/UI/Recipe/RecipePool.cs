using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RecipePool : MonoBehaviour
{
    public GameObject[] recipePrefabs; // ���� ������ RECIPE_OBJECT ������
    [SerializeField] private RectTransform recipePoolPanel; // RECIPE_POOL �г��� RectTransform
    [SerializeField] private float moveSpeed = 100f; // ������Ʈ �̵� �ӵ�
    [SerializeField] private int maxVisibleObjects = 5; // ȭ�鿡 ���� �ִ� ������Ʈ ��

    private Queue<GameObject> objectPool = new Queue<GameObject>(); // ������Ʈ Ǯ�� �����ϴ� ť
    private List<GameObject> activeObjects = new List<GameObject>(); // Ȱ��ȭ�� ������Ʈ ���
    private List<GameObject> allObjects = new List<GameObject>(); // ������ ��� ������Ʈ ���

    private float spawnInterval = 8f; // ������Ʈ ���� ���� (��)
    private float elapsedTime = 0f; // ��� �ð�
    private List<Recipe> recipes; // ������ ������ ���
    public List<Recipe> Recipes { get { return recipes; } }

    private void Start()
    {
        recipes = GetRecipes(); // ������ �����͸� ������

        // ��� ������Ʈ�� �ʱ�ȭ�ϰ� Ǯ�� �߰�
        for (int i = 0; i < recipes.Count * 3; i++)
        {
            GameObject obj = Instantiate(recipePrefabs[i % recipePrefabs.Length], recipePoolPanel);
            obj.SetActive(false); // ������Ʈ�� ��Ȱ��ȭ
            objectPool.Enqueue(obj); // Ǯ�� �߰�
            allObjects.Add(obj); // ��� ������Ʈ ��Ͽ� �߰�
        }

        SetRecipeObjectImages(recipes); // �� ������Ʈ�� �̹����� ����
        ActivateObject(); // ù ��° ������Ʈ Ȱ��ȭ
    }

    private void Update()
    {
        if (GameManager.Instance.isPlaying) // ������ ���� ���� ����
        {
            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ

            if (elapsedTime >= spawnInterval) // ���� ������ ������
            {
                elapsedTime = 0f;
                ActivateObject(); // ���ο� ������Ʈ Ȱ��ȭ
            }

            MoveActiveObjects(); // Ȱ��ȭ�� ������Ʈ �̵�
        }
    }

    // Ȱ��ȭ�� ������Ʈ�� �������� �̵�
    private void MoveActiveObjects()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            GameObject obj = activeObjects[i];
            if (obj.activeSelf) // ������Ʈ�� Ȱ��ȭ�� ���
            {
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition += Vector2.left * moveSpeed * Time.deltaTime; // �������� �̵�

                // ���� ������Ʈ���� �Ÿ� ����
                if (i > 0)
                {
                    RectTransform prevRectTransform = activeObjects[i - 1].GetComponent<RectTransform>();
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width - 150 <= prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width)
                    {
                        rectTransform.anchoredPosition = new Vector2(prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width + 150, rectTransform.anchoredPosition.y);
                    }
                }
                else // ù ��° ������Ʈ�� ���� ���� ��ġ
                {
                    if (rectTransform.anchoredPosition.x - rectTransform.rect.width / 2 <= -recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2)
                    {
                        rectTransform.anchoredPosition = new Vector2(-recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
                    }
                }

                // ������Ʈ�� Ȱ��ȭ�� �� 20�ʰ� ������ ��Ȱ��ȭ
                if (Time.time - obj.GetComponent<RecipeObject>().activationTime >= 20f)
                {
                    DeactivateObject(obj); // ������Ʈ ��Ȱ��ȭ
                    ScoreManager.Instance.SubScore(10); // ���� ����
                }
            }
        }
    }

    // ���ο� ������Ʈ�� Ȱ��ȭ
    private void ActivateObject()
    {
        if (objectPool.Count > 0) // Ǯ�� ������Ʈ�� �������� ��
        {
            int randomIndex = Random.Range(0, objectPool.Count); // ���� �ε��� ����
            GameObject obj = objectPool.ToArray()[randomIndex]; // ���� ������Ʈ ����
            objectPool = new Queue<GameObject>(objectPool); // ť �籸��
            objectPool.Dequeue(); // ���õ� ������Ʈ ����
            obj.SetActive(true); // ������Ʈ Ȱ��ȭ

            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, 0); // ������Ʈ ��ġ ����

            activeObjects.Add(obj); // Ȱ��ȭ�� ������Ʈ ��Ͽ� �߰�

            obj.GetComponent<RecipeObject>().activationTime = Time.time; // Ȱ��ȭ �ð� ����

            if (activeObjects.Count > maxVisibleObjects) // Ȱ��ȭ�� ������Ʈ�� �ִ�ġ�� ������
            {
                GameObject oldestObj = activeObjects[0]; // ���� ������ ������Ʈ ����
                activeObjects.RemoveAt(0); // ��Ͽ��� ����
                DeactivateObject(oldestObj); // ������Ʈ ��Ȱ��ȭ
            }
        }
    }

    // ������Ʈ ��Ȱ��ȭ
    private void DeactivateObject(GameObject obj)
    {
        activeObjects.Remove(obj); // Ȱ��ȭ�� ��Ͽ��� ����
        obj.SetActive(false); // ������Ʈ ��Ȱ��ȭ
        objectPool.Enqueue(obj); // Ǯ�� �ٽ� �߰�
    }

    // ���� ������Ʈ ó��
    public void CorrectRecipe(GameObject obj)
    {
        obj.GetComponent<Image>().color = Color.green; // ������Ʈ ���� ����
        int points = obj.GetComponent<RecipeObject>().Point; // ���� ��������
        ScoreManager.Instance.AddScore(points); // ���� �߰�
        StartCoroutine(DeactivateAfterDelay(obj, 0.2f)); // ���� �� ��Ȱ��ȭ
    }

    // ���� �� ������Ʈ ��Ȱ��ȭ
    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateObject(obj); // ������Ʈ ��Ȱ��ȭ
    }

    // �� ������Ʈ�� �̹����� ����
    private void SetRecipeObjectImages(List<Recipe> recipes)
    {
        for (int i = 0; i < recipes.Count; i++)
        {
            Recipe recipe = recipes[i]; // ������ ������
            GameObject obj = allObjects[i]; // ������Ʈ
            Dictionary<string, string> imageNameMap = new Dictionary<string, string>
            {
                { "Recipe_Icon", recipe.recipe } // ������ ������ ����
            };

            // ��� ������ ����
            for (int j = 0; j < recipe.ingredient.Count(); j++)
            {
                imageNameMap.Add($"Ingredient_Icon_{j + 1}", recipe.ingredient[j]);
                
            }

            RecipeObject recipeObject = obj.GetComponent<RecipeObject>();
            recipeObject.SetImagesByName(imageNameMap); // �̹��� ����
        }
    }

    // ������ �����͸� �������� �޼��� (���� ������)

    private List<Recipe> GetRecipes()
    {
        // ���� ������ �������� �ڵ�� ��ü�ؾ� ��
        return new List<Recipe>
        {
            new Recipe { id = 1, stage = 1, recipe = "recipe1", ingredient = new List<string> { "ingredient1" } },
            new Recipe { id = 2, stage = 1, recipe = "recipe2", ingredient = new List<string> { "ingredient1", "ingredient2" } },
            new Recipe { id = 3, stage = 1, recipe = "recipe3", ingredient = new List<string> { "ingredient1", "ingredient2", "ingredient3" } }
        };
    }
}

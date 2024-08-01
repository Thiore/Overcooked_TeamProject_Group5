using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RecipePool : MonoBehaviour
{
    public GameObject oneIngredientPrefab; // 1���� ��Ḧ ������ ������ ������
    public GameObject twoIngredientsPrefab; // 2���� ��Ḧ ������ ������ ������
    public GameObject threeIngredientsPrefab; // 3���� ��Ḧ ������ ������ ������
    public GameObject twoIngredient_onetool_Prefab;
    public GameObject twoIngredient_twotool_Prefab;
    public GameObject threeIngredient_onetool_Prefab;
    public GameObject threeIngredient_twotool_Prefab;
    [SerializeField] private RectTransform recipePoolPanel; // RECIPE_POOL �г��� RectTransform
    [SerializeField] private float moveSpeed = 100f; // ������Ʈ �̵� �ӵ�
    [SerializeField] private int maxVisibleObjects = 5; // ȭ�鿡 ���� �ִ� ������Ʈ ��

    private List<GameObject> objectPool = new List<GameObject>(); // ������Ʈ Ǯ�� �����ϴ� ����Ʈ
    public List<GameObject> activeObjects = new List<GameObject>(); // Ȱ��ȭ�� ������Ʈ ���
    private List<GameObject> allObjects = new List<GameObject>(); // ������ ��� ������Ʈ ���

    private float spawnInterval = 8f; // ������Ʈ ���� ���� (��)
    private float elapsedTime = 0f; // ��� �ð�
    private List<Recipe> recipes; // ������ ������ ���
    public List<Recipe> Recipes { get { return recipes; } }

    private void Start()
    {
        GetRecipes(); // ������ �����͸� ������
        InitializeObjectPool(); // ������Ʈ Ǯ �ʱ�ȭ
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

    // ������Ʈ Ǯ �ʱ�ȭ
    private void InitializeObjectPool()
    {
        foreach (Recipe recipe in recipes)
        {
            GameObject prefab = GetPrefabForRecipe(recipe);
            for (int i = 0; i < 3; i++) // �� �����ǿ� ���� 3���� ������Ʈ ����
            {
                GameObject obj = Instantiate(prefab, recipePoolPanel);
                obj.name = recipe.recipe;
                obj.SetActive(false); // ������Ʈ�� ��Ȱ��ȭ
                objectPool.Add(obj); // Ǯ�� �߰�
                allObjects.Add(obj); // ��� ������Ʈ ��Ͽ� �߰�
            }
        }
    }

    // �����ǿ� �´� �������� ��ȯ
    private GameObject GetPrefabForRecipe(Recipe recipe)
    {
        RecipeObject recipeObject = new RecipeObject();
        
        switch (recipe.ingredient.Count)
        {
            case 1:
                return oneIngredientPrefab;
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
            GameObject obj = objectPool[0]; // ����Ʈ�� ù ��° ������Ʈ ��������
            objectPool.RemoveAt(0); // ����Ʈ���� ����
            Image[] childImages = obj.GetComponentsInChildren<Image>();
            foreach (Image img in childImages)
            {
                img.color = Color.white;
            }
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
        Debug.Log("������ Ȱ��ȭ");
    }

    // ������Ʈ ��Ȱ��ȭ
    private void DeactivateObject(GameObject obj)
    {
        activeObjects.Remove(obj); // Ȱ��ȭ�� ��Ͽ��� ����
        obj.SetActive(false); // ������Ʈ ��Ȱ��ȭ
        objectPool.Add(obj); // Ǯ�� �ٽ� �߰�
        Debug.Log("������ ��Ȱ��ȭ");
    }


    //Debug.Log�� ����� �޼��� �߰� ���� �ʿ�
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

                return; // ��Ī�Ǵ� ������Ʈ�� ã�����Ƿ� �޼��� ����
            }
        }
        StartCoroutine(Wrong());
    }

    private void AllCorrect(int index,int point)
    {
        // ��� �ڽ� Image ������Ʈ�� ������ ���� ����
        Image[] childImages = activeObjects[index].GetComponentsInChildren<Image>();
        foreach (Image img in childImages)
        {
            img.color = Color.green;
        }

        Debug.Log(activeObjects[index].name);
        StartCoroutine(DeactivateAfterDelay(activeObjects[index], 0.2f));
        GameManager.Instance.AllCorrect_Recipe(point);
        if (activeObjects.Count == 0)
        {
            ActivateObject();
        }
    }

    private void InCorrect(int index, int point)
    {
        // ��� �ڽ� Image ������Ʈ�� ������ ���� ����
        Image[] childImages = activeObjects[index].GetComponentsInChildren<Image>();
        foreach (Image img in childImages)
        {
            img.color = Color.green;
        }
        Debug.Log(activeObjects[index].name);
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


    // ���� �� ������Ʈ ��Ȱ��ȭ
    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateObject(obj); // ������Ʈ ��Ȱ��ȭ
    }

    // �� ������Ʈ�� �̹����� ����
    private void SetRecipeObjectImages(List<Recipe> recipes)
    {
        int objectsPerRecipe = allObjects.Count / recipes.Count;

        for (int i = 0; i < recipes.Count; i++)
        {
            for (int j = 0; j < objectsPerRecipe; j++)
            {
                int objectIndex = i * objectsPerRecipe + j;
                GameObject obj = allObjects[objectIndex];
                Recipe recipe = recipes[i];
                RecipeObject recipeObject = obj.GetComponent<RecipeObject>();
                obj.name = recipe.recipe;
                Dictionary<string, string> imageNameMap = new Dictionary<string, string>
            {
                { "Recipe_Icon", recipe.recipe }
            };

                for (int k = 0; k < recipe.ingredient.Count; k++)
                {
                    if (recipe.ingredient.Count == 1)
                    {
                        imageNameMap.Add($"Ingredient_Icon", recipe.ingredient[k]);
                    }
                    else
                    {
                        imageNameMap.Add($"Ingredient_Icon_{k + 1}", recipe.ingredient[k]);
                    }

                    if (recipe.ingredient[k].Contains("pot")|| recipe.ingredient[k].Contains("pan")||recipe.ingredient[k].Contains("fry"))
                    {
                        string tool = obj.GetComponent<Recipe>().ingredient[k].Substring(recipe.ingredient[0].Length - 3);
                        recipeObject.SetToolImage(k,tool);
                    }
                    
                }


                
                recipeObject.SetImagesByName(imageNameMap);
                
            }
        }
    }


    // ������ �����͸� �������� �޼���
    private void GetRecipes()
    {
        recipes = DataManager.Instance.StageRecipeData(GameManager.Instance.stage_index);
    }
}

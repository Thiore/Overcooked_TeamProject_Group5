using UnityEngine;
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

    private void Start()
    {
        // �ʱ� ������Ʈ Ǯ ����
        for (int i = 0; i < maxVisibleObjects * recipePrefabs.Length; i++)
        {
            GameObject obj = Instantiate(recipePrefabs[i % recipePrefabs.Length], recipePoolPanel);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    private void Update()
    {
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
                    if (rectTransform.anchoredPosition.x-rectTransform.rect.width-150 <= prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width)
                    {
                        rectTransform.anchoredPosition = new Vector2(prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width + 150, rectTransform.anchoredPosition.y);
                    }
                }
                else
                {
                    // ù ��° ������Ʈ�� ���� ������ ����
                    if (rectTransform.anchoredPosition.x-rectTransform.rect.width/2 <= -recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2)
                    {
                        rectTransform.anchoredPosition = new Vector2(-recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, rectTransform.anchoredPosition.y);
                    }
                }
            }
        }
    }

    public void ActivateObject(int index)
    {
        if (objectPool.Count > 0 && index >= 0 && index < recipePrefabs.Length)
        {
            // Ǯ���� ������Ʈ�� ������ Ȱ��ȭ
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);

            // ������Ʈ�� ���� ��ġ�� �г��� ���� ���������� ����
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, 0);

            // Ȱ��ȭ�� ������Ʈ ��Ͽ� �߰�
            activeObjects.Add(obj);

            // Ȱ��ȭ�� ������Ʈ ���� �ִ�ġ�� ������ ���� ������ ������Ʈ�� ��Ȱ��ȭ
            if (activeObjects.Count > maxVisibleObjects)
            {
                GameObject oldestObj = activeObjects[0];
                activeObjects.RemoveAt(0);
                oldestObj.SetActive(false);
                objectPool.Enqueue(oldestObj);
            }
        }
    }
}

using UnityEngine;
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

    private void Start()
    {
        // 초기 오브젝트 풀 생성
        for (int i = 0; i < maxVisibleObjects * recipePrefabs.Length; i++)
        {
            GameObject obj = Instantiate(recipePrefabs[i % recipePrefabs.Length], recipePoolPanel);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    private void Update()
    {
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
                    if (rectTransform.anchoredPosition.x-rectTransform.rect.width-150 <= prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width)
                    {
                        rectTransform.anchoredPosition = new Vector2(prevRectTransform.anchoredPosition.x + prevRectTransform.rect.width + rectTransform.rect.width + 150, rectTransform.anchoredPosition.y);
                    }
                }
                else
                {
                    // 첫 번째 오브젝트는 왼쪽 끝에서 멈춤
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
            // 풀에서 오브젝트를 가져와 활성화
            GameObject obj = objectPool.Dequeue();
            obj.SetActive(true);

            // 오브젝트의 시작 위치를 패널의 가장 오른쪽으로 설정
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(recipePoolPanel.rect.width / 2 + rectTransform.rect.width / 2, 0);

            // 활성화된 오브젝트 목록에 추가
            activeObjects.Add(obj);

            // 활성화된 오브젝트 수가 최대치를 넘으면 가장 오래된 오브젝트를 비활성화
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

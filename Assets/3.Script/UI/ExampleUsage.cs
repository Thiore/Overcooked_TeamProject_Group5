using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public RecipePool recipePool;
    private int currentPrefabIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 현재 인덱스의 프리팹 활성화
            recipePool.ActivateObject(currentPrefabIndex);

            // 다음 인덱스로 이동, 만약 마지막 인덱스라면 0으로 돌아감
            currentPrefabIndex = (currentPrefabIndex + 1) % recipePool.recipePrefabs.Length;
        }
    }
}

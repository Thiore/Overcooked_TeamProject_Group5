using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public RecipePool recipePool;
    private int currentPrefabIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ���� �ε����� ������ Ȱ��ȭ
            recipePool.ActivateObject(currentPrefabIndex);

            // ���� �ε����� �̵�, ���� ������ �ε������ 0���� ���ư�
            currentPrefabIndex = (currentPrefabIndex + 1) % recipePool.recipePrefabs.Length;
        }
    }
}

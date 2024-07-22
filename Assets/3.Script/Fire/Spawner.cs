using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;
    public int numberOfObjects = 15; // 한 번에 생성할 오브젝트 수
    public float spawnOffset = 0.1f; // 스폰 위치 오차 범위
    public float spawnInterval = 0.1f; // 오브젝트 생성 간격

    private bool isSpawning = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpawning = true;
            StartCoroutine(SpawnObjects());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpawning = false;
            StopCoroutine(SpawnObjects());
        }
    }

    private IEnumerator SpawnObjects()
    {
        while (isSpawning)
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-spawnOffset, spawnOffset), Random.Range(-spawnOffset, spawnOffset), 0);
                GameObject obj = Extingusher_Pool.Instance.GetObject();
                obj.transform.position = spawnPoint.position + randomOffset;
                obj.transform.rotation = spawnPoint.rotation;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}

using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxDistance = 20f;
    private Vector3 initialPosition;
    private Vector3 initialScale;

    private void OnEnable()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        StartCoroutine(Move());
    }

    private void OnDisable()
    {
        StopCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (Vector3.Distance(initialPosition, transform.position) < maxDistance)
        {
            float distanceTravelled = Vector3.Distance(initialPosition, transform.position);
            float scaleMultiplier = 1f;

            if (distanceTravelled < maxDistance / 2)
            {
                scaleMultiplier = 1 + (distanceTravelled / (maxDistance / 2)); // 크기 증가
            }
            else
            {
                scaleMultiplier = 2 - ((distanceTravelled - (maxDistance / 2)) / (maxDistance / 2)); // 크기 감소
            }

            transform.localScale = initialScale * scaleMultiplier;

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // Raycast를 사용하여 벽 충돌 감지
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    // 벽에 부딪혔을 때 퍼지는 로직
                    Burst();
                    yield break;
                }
            }

            yield return null;
        }

        // 일정 거리 이상 이동했을 때 반환
        Extingusher_Pool.Instance.ReturnObject(gameObject);
    }

    private void Burst()
    {
        // 퍼지는 효과를 여기에 추가
        // 예: Particle System 등을 활성화

        // 오브젝트 풀로 반환
        Extingusher_Pool.Instance.ReturnObject(gameObject);
    }
}

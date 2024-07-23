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
                scaleMultiplier = 1 + (distanceTravelled / (maxDistance / 2)); // ũ�� ����
            }
            else
            {
                scaleMultiplier = 2 - ((distanceTravelled - (maxDistance / 2)) / (maxDistance / 2)); // ũ�� ����
            }

            transform.localScale = initialScale * scaleMultiplier;

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // Raycast�� ����Ͽ� �� �浹 ����
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    // ���� �ε����� �� ������ ����
                    Burst();
                    yield break;
                }
            }

            yield return null;
        }

        // ���� �Ÿ� �̻� �̵����� �� ��ȯ
        Extingusher_Pool.Instance.ReturnObject(gameObject);
    }

    private void Burst()
    {
        // ������ ȿ���� ���⿡ �߰�
        // ��: Particle System ���� Ȱ��ȭ

        // ������Ʈ Ǯ�� ��ȯ
        Extingusher_Pool.Instance.ReturnObject(gameObject);
    }
}

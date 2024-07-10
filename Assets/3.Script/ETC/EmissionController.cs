using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    private bool isPick = false;
    private Queue<GameObject> pickQue;

    private Material originalMaterial;
    private Material instanceMaterial;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("TEST"))
        {
            if(!isPick)
            {
                isPick = true;
                if (pickQue.Count > 0)
                {
                    ByeObeject(pickQue.Peek());
                    pickQue.Clear();
                    pickQue.Enqueue(collision.gameObject);
                    PickObject(pickQue.Peek());
                }

                if (pickQue.Count.Equals(0))
                {
                    pickQue.Enqueue(collision.gameObject);
                    PickObject(pickQue.Peek());
                }

                Debug.Log("ȣ��");
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(pickQue.Count > 0)
        {
           ByeObeject(pickQue.Peek());
           pickQue.Clear();
        }

        isPick = false;
    }

    void PickObject(GameObject gameObject)
    {

        // �浹�� ������Ʈ�� ������ ��������
        Renderer renderer = gameObject.gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            // ������ ���׸��� ����
            originalMaterial = renderer.material;

            // ���׸��� ����
            instanceMaterial = new Material(originalMaterial);

            // ������ ���׸���� ����
            renderer.material = instanceMaterial;

            // Emission Ȱ��ȭ �� HDR �� ����
            instanceMaterial.EnableKeyword("_EMISSION");
            instanceMaterial.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f)/* * Mathf.LinearToGammaSpace(2.0f)*/);
        }
    }

    void ByeObeject(GameObject gameObject)
    {
        // �浹�� ������ �� ���� ���׸���� ����
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = originalMaterial;
        }
    }
}



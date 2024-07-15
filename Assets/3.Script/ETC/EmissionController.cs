using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    private Queue<GameObject> pickQue;
    private List<GameObject> pickList;

    private Material originalMaterial;
    private Material instanceMaterial;

    //private Player_StateController playerStateController;
    private Player_StateController1 playerStateController;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
        pickList = new List<GameObject>();

        playerStateController = GetComponent<Player_StateController1>();
    }

    //ī���ͳ� ȭ�� ��� ��ġ�Ȱ� �˺� ���߿� if || �߰��ϱ� (�±׸� ������ �Ȱ��� queue�� �˻��ϱ� ����)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Counter")/*||ȭ��||��������*/)
        {
            pickList.Add(other.gameObject);

            for (int i = 0; i < pickList.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, pickList[i].transform.position);

                if (pickQue.Count > 0)
                {
                    float pickDistance = Vector3.Distance(transform.position, pickQue.Peek().transform.position);
                    if (distance < pickDistance)
                    {
                        ByeObeject(pickQue.Peek());
                        pickQue.Clear();
                        pickQue.Enqueue(pickList[i]);
                        PickObject(pickQue.Peek());
                    }
                }

                if (pickQue.Count.Equals(0))
                {
                    pickQue.Enqueue(pickList[i]);
                    PickObject(pickQue.Peek());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickQue.Count > 0)
        {
            ByeObeject(pickQue.Peek());
            pickQue.Clear();
        }
    }

    void PickObject(GameObject gameObject)
    {

        // �浹�� ������Ʈ�� ������ ��������
        Renderer renderer = gameObject.GetComponent<Renderer>();

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

        playerStateController.IsEmission = true;
    }

    void ByeObeject(GameObject gameObject)
    {
        // �浹�� ������ �� ���� ���׸���� ����
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = originalMaterial;
        }
        playerStateController.IsEmission = false;
    }
}



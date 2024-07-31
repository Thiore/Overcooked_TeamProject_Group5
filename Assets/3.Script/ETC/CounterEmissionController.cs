using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterEmissionController : MonoBehaviour
{
    private Queue<GameObject> pickQue;
    private List<GameObject> pickList;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
        pickList = new List<GameObject>();
    }

    //ī���ͳ� ȭ�� ��� ��ġ�Ȱ� �˺� ���߿� if || �߰��ϱ� (�±׸� ������ �Ȱ��� queue�� �˻��ϱ� ����)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Counter") || other.gameObject.CompareTag("Crate") || other.gameObject.CompareTag("Pass") ||
            other.gameObject.CompareTag("TrashCan") || other.gameObject.transform.CompareTag("Plate_Return") || other.gameObject.CompareTag("GasRange"))
        {
            pickList.Add(other.gameObject);
            if (pickQue.Count.Equals(0) && pickList.Count.Equals(1))
            {
                pickQue.Enqueue(other.gameObject);
                ChangeEmission(pickQue.Peek());
                pickList.Clear();
                return;
            }

            for (int i = 0; i < pickList.Count; i++)
            {
                float queDistance = Vector3.Distance(transform.position, pickQue.Peek().transform.position);
                float distance = Vector3.Distance(transform.position, pickList[i].transform.position);
                
                if(distance < queDistance)
                {
                    if(pickQue.Count != 0)
                    ChangeOriginEmission(pickQue.Dequeue());

                    pickQue.Enqueue(pickList[i]);
                    ChangeEmission(pickQue.Peek());
                    pickList.Clear();
                    return;
                }              
            }

            pickList.Clear();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickQue.Count > 0)
        {
            ChangeOriginEmission(pickQue.Dequeue());
            pickQue.Clear();
        }
    }


    void ChangeEmission(GameObject gameObject)
    {
        // �浹�� ������Ʈ�� ������ ��������
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            // Emission Ȱ��ȭ �� HDR �� ����
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", new Color(0.3f, 0.3f, 0.3f)/* * Mathf.LinearToGammaSpace(2.0f)*/);
        }
    }

    void ChangeOriginEmission(GameObject gameObject)
    {
        // �浹�� ������ �� ���� ���׸���� ����
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
        }
    }

    public GameObject GetNearCounter()
    {
        if (pickQue.Count > 0)
        {
            return pickQue.Peek();
        }
        return null;
    }
}



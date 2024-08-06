using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterEmissionController : MonoBehaviour
{
    private Queue<GameObject> pickQue;
    private List<GameObject> pickList;

    //private GameObject SelectObj = null;
    private GameObject NextObj = null;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
        pickList = new List<GameObject>();
    }

    //ī���ͳ� ȭ�� ��� ��ġ�Ȱ� �˺� ���߿� if || �߰��ϱ� (�±׸� ������ �Ȱ��� queue�� �˻��ϱ� ����)
    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Counter") || other.gameObject.CompareTag("Crate") || other.gameObject.CompareTag("Pass") ||
            other.gameObject.CompareTag("TrashCan") || other.gameObject.transform.CompareTag("Plate_Return") || other.gameObject.CompareTag("GasRange")
            || other.gameObject.CompareTag("Sink") || other.gameObject.CompareTag("GasStation"))
        {
            if(pickQue.Count.Equals(0))
            {
                pickQue.Enqueue(other.gameObject);
                ChangeEmission(pickQue.Peek());
                return;
            }
            else
            {
                ChangeEmission(pickQue.Peek());
                if (other.gameObject == pickQue.Peek())
                {
                    return;
                }
                else
                {
                    NextObj = other.gameObject;
                }
            }
            
           
                float queDistance = Vector3.Distance(transform.position, pickQue.Peek().transform.position);
                float distance = Vector3.Distance(transform.position, NextObj.transform.position);

            if (distance < queDistance)
            {
                ChangeOriginEmission(pickQue.Dequeue());

                pickQue.Enqueue(NextObj);
                ChangeEmission(pickQue.Peek());

                return;
            }   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (pickQue.Count > 0)
        {
            ChangeOriginEmission(pickQue.Dequeue());
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
        gameObject.TryGetComponent(out CounterController counter);
        Array.Clear(counter.playerAnim, 0, counter.playerAnim.Length);
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



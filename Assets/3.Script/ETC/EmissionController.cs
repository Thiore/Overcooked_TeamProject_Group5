using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1f;
    private bool isSelect = false;
    private Queue<GameObject> pickQue;

    private Material originalMaterial;
    [SerializeField] private Material instanceMaterial;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();

        instanceMaterial.EnableKeyword("_EMISSION");
        instanceMaterial.SetColor("_EmissionColor", new Color(60, 60, 60)/* * Mathf.LinearToGammaSpace(2.0f)*/);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TEST"))
        {
            if(pickQue.Count.Equals(0))
            {
               pickQue.Enqueue(collision.gameObject);
            }
        }

        //if (collision.transform.CompareTag("TEST"))
        //{
        //    // �浹�� ������Ʈ�� ������ ��������
        //    Renderer renderer = collision.gameObject.GetComponent<Renderer>();

        //    if (renderer != null)
        //    {
        //        // ������ ���׸��� ����
        //        originalMaterial = renderer.material;

        //        // ������ ���׸���� ����
        //        renderer.material = instanceMaterial;
        //        Debug.Log("�浹");
        //    }
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        pickQue.Dequeue();
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    // �浹�� ������ �� ���� ���׸���� ����
    //    Renderer renderer = collision.gameObject.GetComponent<Renderer>();

    //    if (renderer != null && renderer.material == instanceMaterial)
    //    {
    //        renderer.material = originalMaterial;
    //    }
    //}
}

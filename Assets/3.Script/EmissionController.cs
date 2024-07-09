using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionController : MonoBehaviour
{
    private Material originalMaterial;
    private Material instanceMaterial;

    void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� ������ ��������
        Renderer renderer = collision.gameObject.GetComponent<Renderer>();

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
            instanceMaterial.SetColor("_EmissionColor", new Color(60,60,60)/* * Mathf.LinearToGammaSpace(2.0f)*/);

            Debug.Log("�浹");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // �浹�� ������ �� ���� ���׸���� ����
        Renderer renderer = collision.gameObject.GetComponent<Renderer>();

        if (renderer != null && renderer.material == instanceMaterial)
        {
            renderer.material = originalMaterial;
        }
    }
}

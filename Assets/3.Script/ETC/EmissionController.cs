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
        //    // 충돌한 오브젝트의 렌더러 가져오기
        //    Renderer renderer = collision.gameObject.GetComponent<Renderer>();

        //    if (renderer != null)
        //    {
        //        // 원래의 머테리얼 저장
        //        originalMaterial = renderer.material;

        //        // 복사한 머테리얼로 변경
        //        renderer.material = instanceMaterial;
        //        Debug.Log("충돌");
        //    }
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        pickQue.Dequeue();
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    // 충돌이 끝났을 때 원래 머테리얼로 복구
    //    Renderer renderer = collision.gameObject.GetComponent<Renderer>();

    //    if (renderer != null && renderer.material == instanceMaterial)
    //    {
    //        renderer.material = originalMaterial;
    //    }
    //}
}

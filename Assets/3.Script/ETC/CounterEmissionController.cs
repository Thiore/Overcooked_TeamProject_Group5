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

    //카운터나 화구 등등 설치된거 검별 나중에 if || 추가하기 (태그를 나눠도 똑같이 queue로 검사하기 위해)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Counter") || other.gameObject.CompareTag("Crate")/*||화구||쓰레기통*/)
        {
            pickList.Add(other.gameObject);
            if (pickQue.Count.Equals(0))
            {
                pickQue.Enqueue(other.gameObject);
                ChangeEmission(pickQue.Peek());
            }

            for (int i = 0; i < pickList.Count; i++)
            {
                float queDistance = Vector3.Distance(transform.position, pickQue.Peek().transform.position);
                float distance = Vector3.Distance(transform.position, pickList[i].transform.position);
                
                if(distance < queDistance)
                {
                    ChangeOriginEmission(pickQue.Dequeue());
                    pickQue.Enqueue(pickList[i]);
                    ChangeEmission(pickQue.Peek());
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
        // 충돌한 오브젝트의 렌더러 가져오기
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            // Emission 활성화 및 HDR 값 변경
            renderer.material.EnableKeyword("_EMISSION");
            renderer.material.SetColor("_EmissionColor", new Color(0.3f, 0.3f, 0.3f)/* * Mathf.LinearToGammaSpace(2.0f)*/);
        }
    }

    void ChangeOriginEmission(GameObject gameObject)
    {
        // 충돌이 끝났을 때 원래 머테리얼로 복구
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



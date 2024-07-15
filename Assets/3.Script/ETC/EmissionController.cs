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

    //카운터나 화구 등등 설치된거 검별 나중에 if || 추가하기 (태그를 나눠도 똑같이 queue로 검사하기 위해)
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Counter")/*||화구||쓰레기통*/)
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

        // 충돌한 오브젝트의 렌더러 가져오기
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            // 원래의 머테리얼 저장
            originalMaterial = renderer.material;

            // 머테리얼 복사
            instanceMaterial = new Material(originalMaterial);

            // 복사한 머테리얼로 변경
            renderer.material = instanceMaterial;

            // Emission 활성화 및 HDR 값 변경
            instanceMaterial.EnableKeyword("_EMISSION");
            instanceMaterial.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f)/* * Mathf.LinearToGammaSpace(2.0f)*/);
        }

        playerStateController.IsEmission = true;
    }

    void ByeObeject(GameObject gameObject)
    {
        // 충돌이 끝났을 때 원래 머테리얼로 복구
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = originalMaterial;
        }
        playerStateController.IsEmission = false;
    }
}



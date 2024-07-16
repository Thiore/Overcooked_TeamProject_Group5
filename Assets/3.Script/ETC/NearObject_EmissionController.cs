using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearObject_EmissionController : MonoBehaviour
{
    private Queue<GameObject> pickQue;
    private List<GameObject> pickList;

    private Player_StateController1 playerStateController1;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
        pickList = new List<GameObject>();
        playerStateController1 = GetComponent<Player_StateController1>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredients") || other.gameObject.CompareTag("Cooker") 
            || other.gameObject.CompareTag("Plate"))
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

                if (distance < queDistance)
                {
                    ChangeOriginEmission(pickQue.Dequeue());
                    pickQue.Enqueue(pickList[i]);
                    ChangeEmission(pickQue.Peek());
                }


                if (pickQue.Count.Equals(0))
                {
                    pickQue.Enqueue(pickList[i]);
                    ChangeEmission(pickQue.Peek());
                }

            }

            GetNearObject();
            pickList.Clear();
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (pickQue.Count > 0)
        {
            ChangeOriginEmission(pickQue.Peek());
            pickQue.Clear();
        }

        GetNearObject();
    }


    public void ChangeEmission(GameObject gameObject)
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

    public void ChangeOriginEmission(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
        }
    }

    public GameObject GetNearObject()
    {
        if(pickQue.Count >0)
        {
//            ChangeOriginEmission(pickQue.Peek());
            return pickQue.Peek();
        }
        return null;
    }

}

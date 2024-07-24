using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearObject_EmissionController : MonoBehaviour
{
    private Queue<GameObject> pickQue;
    private List<GameObject> pickList;

    private Player_StateController playerStateController;

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
        pickList = new List<GameObject>();
        playerStateController = GetComponent<Player_StateController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Plate"))
        {
            NearObCheck(other);           
        }
        else if (other.gameObject.CompareTag("Cooker"))
        {
            NearObCheck(other);
        }
        else if (other.gameObject.CompareTag("Ingredients"))
        {
            NearObCheck(other);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if (pickQue.Count > 0)
        {
            ChangeOriginEmission(pickQue.Peek());
            pickQue.Clear();
        }

    }


    private void NearObCheck(Collider other)
    {
        pickList.Add(other.gameObject);
        if (pickQue.Count.Equals(0))
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

            if (distance < queDistance)
            {
                if (pickQue.Count != 0)
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
        pickList.Clear();
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

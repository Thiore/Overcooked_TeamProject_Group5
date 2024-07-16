using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients_EmissionController : MonoBehaviour
{
    private Queue<GameObject> pickQue;
    private List<GameObject> pickList;

    private Player_StateController1 playerStateController;

    private bool isBellowIngre = false;
    public bool IsBellowIngre { get => isBellowIngre; set => isBellowIngre = value; }

    private void Awake()
    {
        pickQue = new Queue<GameObject>();
        pickList = new List<GameObject>();

        playerStateController = GetComponent<Player_StateController1>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ingredients") || other.gameObject.CompareTag("Cooker"))
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

            pickList.Clear();
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


    public void PickObject(GameObject gameObject)
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

    public void ByeObeject(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
        }
    }


}

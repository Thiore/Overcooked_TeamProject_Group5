using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전반적 플레이어 상호작용 (재료 / 요리 도구 등등 상호작용) 
public class Player_StateController : MonoBehaviour
{
    private Animator animator;
    private GameObject PickOB;
    [SerializeField] private Transform Attachtransform;

    private bool isBellow = false;
    private bool isCoroutineRunning = false;

    private bool isEmission = false;
    public bool IsEmission { get => isEmission; set => isEmission = value; }

    private Ingredients_EmissionController ingre_controller;

    private void Awake()
    {
        ingre_controller = GetComponent<Ingredients_EmissionController>();
        animator = GetComponent<Animator>();
    }

    //온스테이 안에서 태그로 전반적인 처리
    private void OnTriggerStay(Collider other)
    {
        if(!isCoroutineRunning) 
        { 
            StartCoroutine(PlayerStateChange(other.gameObject));
        }
    }


    private IEnumerator PlayerStateChange(GameObject gameObject)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isCoroutineRunning = true;

            //재료 상자 앞에서 
            if (gameObject.CompareTag("Crate") && !isBellow)
            {
                var ani = gameObject.transform.GetComponent<Animator>();
                if (ani != null)
                {
                    ani.SetTrigger("Pick");
                }
                yield return new WaitForSeconds(0.5f);
            }

            // 재료를 내려놓을때
            if (isBellow)
            {
                DropIngredients();
                yield return new WaitForSeconds(0.5f);
            }

            // 재료를 들때
            if (!isBellow && gameObject.CompareTag("Ingredients"))
            {
                TakeIngredients(gameObject);
                yield return new WaitForSeconds(0.5f);
            }

            isCoroutineRunning = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {

        }


    }





    private void DropIngredients()
    {
        if(PickOB != null)
        {
            animator.SetBool("IsTake", false);
            isBellow = false;
            PickOB.gameObject.AddComponent<Rigidbody>();
            PickOB.transform.SetParent(null);
            PickOB = null;
        }
    }

    private void TakeIngredients(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        isBellow = true;
        PickOB = gameObject;
        Destroy(PickOB.transform.GetComponent<Rigidbody>());
        ingre_controller.ByeObeject(PickOB);
        PickOB.transform.SetParent(Attachtransform);
        PickOB.transform.rotation = Attachtransform.rotation;
        PickOB.transform.position = Attachtransform.position;
    }

}

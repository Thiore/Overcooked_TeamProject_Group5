using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전반적 플레이어 상호작용 (재료 / 요리 도구 등등 상호작용) 
public class Player_StateController : MonoBehaviour
{
    private Animator animator;
    private GameObject PickOB;
    [SerializeField] private Transform Attachtransform;

    public bool isBellow = false;
    private bool isCoroutineRunning = false;

    private bool isEmission = false;
    public bool IsEmission { get => isEmission; set => isEmission = value; }

    private NearObject_EmissionController ingre_controller;

    private void Awake()
    {
        ingre_controller = GetComponent<NearObject_EmissionController>();
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
        // 스페이스바는 집을수 있는 사물들은 집어 올림(재료, 요리도구 등등)
        if (Input.GetKey(KeyCode.Space))
        {
            isCoroutineRunning = true;

            //재료 상자 앞에서 
            if (gameObject.CompareTag("Crate") && !isBellow)
            {
                //var ani = gameObject.transform.GetComponent<Animator>();
                //if (ani != null)
                //{
                //    ani.SetTrigger("Pick");
                //}
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


        //요리도구 상호작용 
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            //프라이팬 



        }


    }


    //private IEnumerator PlayerStateChange(GameObject gameObject)
    //{
    //    // 스페이스바는 집을수 있는 사물들은 집어 올림(재료, 요리도구, 접시
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        // 재료를 내려놓을때
    //        if (isBellow)
    //        {
    //            DropObject();
    //            isBellow = false;
    //            yield return new WaitForSeconds(0.5f);
    //        }
    //        else   // 집지 않은 상태 
    //        {
    //            //재료 상자 앞에서 
    //            if (gameObject.CompareTag("Crate"))
    //            {
    //                var ani = gameObject.transform.GetComponent<Animator>();
    //                if (ani != null)
    //                {
    //                    ani.SetTrigger("Pick");
    //                    // 생성된 재료 오브젝트 바로 집는 메소드 추가 필요 
    //                }
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //            else if (gameObject.CompareTag("Cooker") || gameObject.CompareTag("Ingredients"))
    //            {
    //                TakeHandObject(gameObject);
    //                isBellow = true;
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //        }

    //        coroutine = null;
    //    }


    //    //요리도구 상호작용 
    //    if (Input.GetKeyDown(KeyCode.LeftControl))
    //    {
    //        isCoroutineRunning = true;
    //    }


    //}



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

    //private void DropObject()
    //{
    //    if (PickOB != null)
    //    {
    //        animator.SetBool("IsTake", false);
    //        if (emissionController.IsPutOn)
    //        {
    //            Debug.Log("풋온");
    //            var counter = emissionController.ReadyPutCounter();
    //            if (counter != null && emissionController.PutOnReady())
    //            {
    //                PickOB.transform.SetParent(counter.transform);
    //                PickOB.transform.position = counter.transform.position;
    //                PickOB.transform.rotation = Quaternion.identity;
    //                emissionController.IsPutOn = false;
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("풋아님");
    //            PickOB.transform.SetParent(null);
    //            var rb = PickOB.gameObject.AddComponent<Rigidbody>();
    //            rb.mass = 10;

    //        }
    //        PickOB = null;
    //    }
    //}

    private void TakeIngredients(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        isBellow = true;
        PickOB = gameObject;
        Destroy(PickOB.transform.GetComponent<Rigidbody>());
        ingre_controller.ChangeOriginEmission(PickOB);
        PickOB.transform.SetParent(Attachtransform);
        PickOB.transform.rotation = Attachtransform.rotation;
        PickOB.transform.position = Attachtransform.position;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//전반적 플레이어 상호작용 (재료 / 요리 도구 등등 상호작용) 
public class Player_StateController1 : MonoBehaviour
{
    private Animator animator;

    //내가 보는 오브젝트
    private GameObject nearOb;
    //내가 집은 오브젝트 
    private GameObject HandsOnOb;
    //이건 인스펙터에서 셰프 밑에 스켈레톤 Attach 넣어 사용하기
    [SerializeField] private Transform Attachtransform;

    //내가 들고 있는지 
    private bool isHolding = false;

    //코루틴 배열도 가능한데 
    // Start랑 할떄 어떤건지 배열로 관리
    // 각 오브젝트마다 하나하나 들어갈수잇고 플레이어가 좀 더 관리?
    private Coroutine coroutine;

    private CounterEmissionController emissionController;
    private NearObject_EmissionController nearcontroller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        emissionController = GetComponent<CounterEmissionController>();
        nearcontroller = GetComponent<NearObject_EmissionController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(PlayerStateChange(other.gameObject));
        }
    }

    //온스테이로 플레이어 범위 내에서 전반적인 처리
    //private void OnTriggerStay(Collider other)
    //{
    //    if (coroutine == null)
    //    {
    //        coroutine = StartCoroutine(PlayerStateChange(other.gameObject));
    //    }
    //}


    private IEnumerator PlayerStateChange(GameObject gameObject)
    {
        if (gameObject != null)
        {
            // 스페이스바는 집을수 있는 사물들은 집어 올림(재료, 요리도구, 접시
            if (Input.GetKey(KeyCode.Space))
            {
                // 재료를 내려놓을때
                if (isHolding)
                {
                    DropObject();
                    yield return new WaitForSeconds(0.5f);
                }
                else   // 집지 않은 상태 
                {
                    //재료 상자 앞에서 
                    if (gameObject.CompareTag("Crate"))
                    {
                        var ani = gameObject.transform.GetComponent<Animator>();
                        if (ani != null)
                        {
                            ani.SetTrigger("Pick");
                            // 생성된 재료 오브젝트 바로 집는 메소드 추가 필요 
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                    else if (gameObject.CompareTag("Cooker") || gameObject.CompareTag("Ingredients"))
                    {
                        if (nearcontroller.GetNearObject() != null)
                        {
                            TakeHandObject(nearcontroller.GetNearObject());
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                coroutine = null;
            }


            //요리도구 상호작용 
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {

            }
        }

    }



    private void DropObject()
    {
        if (HandsOnOb != null)
        {
            animator.SetBool("IsTake", false);
            if (emissionController.IsPutOn)
            {
                //var counter = emissionController.ReadyPutCounter();
                //if (counter != null)
                //{
                //    HandsOnOb.transform.SetParent(counter.transform);
                //    HandsOnOb.transform.position = counter.transform.position;
                //    HandsOnOb.transform.rotation = Quaternion.identity;
                //    emissionController.IsPutOn = false;
                //}
            }
            else
            {
                HandsOnOb.transform.SetParent(null);
                var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
                rb.mass = 10;

            }
            HandsOnOb = null;
            isHolding = false;
        }
    }

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        HandsOnOb = gameObject;
        Destroy(HandsOnOb.transform.GetComponent<Rigidbody>());
        HandsOnOb.transform.SetParent(Attachtransform);
        HandsOnOb.transform.rotation = Attachtransform.rotation;
        HandsOnOb.transform.position = Attachtransform.position;
        isHolding = true;
    }


}

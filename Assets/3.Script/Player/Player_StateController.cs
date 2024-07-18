using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//전반적 플레이어 상호작용 (재료 / 요리 도구 등등 상호작용) 
public class Player_StateController : MonoBehaviour
{
    private Animator animator;

    //내가 보는 오브젝트
    private GameObject nearOb;
    //내 근처 카운터
    private GameObject nearcounter;
    //내가 집은 오브젝트 
    private GameObject HandsOnOb;
    //이건 인스펙터에서 셰프 밑에 스켈레톤 Attach 넣어 사용하기
    [SerializeField] private Transform Attachtransform;

    //내가 들고 있는지 
    private bool isHolding = false;
    private bool isChop = false;

    public bool IsChop { get => isChop; private set => isChop = value; }

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

    //온스테이로 플레이어 범위 내에서 전반적인 처리
    private void Update()
    {
        nearcounter = emissionController.GetNearCounter();
        nearOb = nearcontroller.GetNearObject();

        // 스페이스바는 집을수 있는 사물들은 집어 올림(재료, 요리도구, 접시
        if (Input.GetKey(KeyCode.Space))
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(PlayerHodingChange());
            }
        }

        //요리도구 상호작용
        // 굽고 썰고 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(PlayerCookedChage());
        }
    }

    private IEnumerator PlayerCookedChage()
    {
        if (nearcounter != null)
        {
            // 동작만하고 실질적인 처리는 재료가?

            animator.SetTrigger("Chop");
            if(nearcounter.transform.childCount.Equals(2))
            {

            }
            Debug.Log(nearcounter.transform.childCount);


        }

        yield return new WaitForSeconds(0.5f);
    }


    private IEnumerator PlayerHodingChange()
    {

        // 재료를 내려놓을때
        if (isHolding)
        {
            DropObject();
        }
        else   // 집지 않은 상태 
        {
            if (nearcounter != null) // 카운터 옆인지 확인 
            {
                var counter = nearcounter.transform.GetComponent<CounterController>();

                // 박스 위에 아무것도 없다면 
                if (!counter.IsPutOn)
                {
                    if (nearcounter.CompareTag("Crate"))
                    {
                        var ani = nearcounter.transform.GetComponent<Animator>();
                        if (ani != null)
                        {
                            ani.SetTrigger("Pick");
                            Debug.Log("열기");
                            // 생성된 재료 오브젝트 바로 집는 메소드 추가 필요 
                            yield return new WaitForSeconds(0.2f);
                        }
                    }

                    //박스위에 없고 근처 오브젝트가 있다면 
                    if (nearOb != null)
                    {
                        TakeHandObject(nearOb);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else // 박스 위에 있으면? 
                {
                    if(counter.PutOnOb != null)
                    {
                        TakeHandObject(nearOb);
                        counter.ChangePuton();
                        counter.PutOnOb = null;
                        yield return new WaitForSeconds(0.2f);
                    }
                }

            }
            else
            {
                if (nearOb != null)
                {
                    TakeHandObject(nearOb);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        coroutine = null;
    }



    private void DropObject()
    {
        if (HandsOnOb != null)
        {
            if (nearcounter != null)
            {
                var counter = nearcounter.transform.GetComponent<CounterController>();
                if (!counter.IsPutOn && !counter.PutOnOb)
                {
                    HandsOnOb.transform.SetParent(nearcounter.transform);
                    HandsOnOb.transform.position = nearcounter.transform.position;
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChangePuton();
                    counter.PutOnOb = HandsOnOb;

                    animator.SetBool("IsTake", false);
                    HandsOnOb = null;
                    isHolding = false;
                }
                else
                {
                    Debug.Log("이미올라가있음");
                }
            }
            else
            {
                HandsOnOb.transform.SetParent(null);
                var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
                rb.mass = 0.05f;
                rb.angularDrag = 0;
                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
        }
    }

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        HandsOnOb = gameObject;
        nearcontroller.ChangeOriginEmission(HandsOnOb);
        Destroy(HandsOnOb.transform.GetComponent<Rigidbody>());
        HandsOnOb.transform.SetParent(Attachtransform);
        HandsOnOb.transform.rotation = Attachtransform.rotation;
        HandsOnOb.transform.position = Attachtransform.position;
        isHolding = true;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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

    public GameObject HandsOnObject {  get => HandsOnOb; }

    //이건 인스펙터에서 셰프 밑에 스켈레톤 Attach 넣어 사용하기
    [SerializeField] private Transform Attachtransform;

    //내가 들고 있는지 
    private bool isHolding = false;
    private bool isChop = false;

    public bool IsHolding { get => isHolding; private set => isHolding = value; }
    public bool IsChop { get => isChop; private set => isChop = value; }

    //코루틴 배열도 가능한데 
    // Start랑 할떄 어떤건지 배열로 관리
    // 각 오브젝트마다 하나하나 들어갈수잇고 플레이어가 좀 더 관리?
    private Coroutine coroutine = null;

    private CounterEmissionController emissionController;
    private NearObject_EmissionController nearcontroller;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        emissionController = GetComponent<CounterEmissionController>();
        nearcontroller = GetComponent<NearObject_EmissionController>();
    }

    private void Update()
    {
        nearcounter = emissionController.GetNearCounter();
        nearOb = nearcontroller.GetNearObject();

        // 스페이스바는 집을수 있는 사물들은 집어 올림(재료, 요리도구, 접시
        if (coroutine == null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                coroutine = StartCoroutine(PlayerHodingChange(nearcounter, nearOb));
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                //던지기 조준 
                if (isHolding && HandsOnOb.CompareTag("Ingredients"))
                {
                    //회전하는건 Movement에. 던지는건 여기에
                    Debug.Log("던지기");
                    ThrowIngredients();
                }
            }


            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                //요리도구 상호작용
                // 굽고 썰고 
                if (!IsHolding)
                {
                    StartCoroutine(PlayerCookedChage());
                }
            }
            

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("ChoppingBoard") && animator.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop"))
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsWalking", false);
        }
    }

    private void ThrowIngredients()
    {
        HandsOnOb.transform.SetParent(null);
        var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        rb.angularDrag = 0;
        rb.AddForce(transform.forward * 100f);
        animator.SetBool("IsTake", false);
        HandsOnOb = null;
        isHolding = false;
    }


    private IEnumerator PlayerHodingChange(GameObject nearCount, GameObject nearob)
    {
        if (nearCount == null && nearob == null)
        {
            coroutine = null;
            yield break;          
        }

        // 근처 카운터가 있고 내가 집은 상태가 아니라면 
        if(isHolding)
        {
            DropObject(nearCount, nearob);
            yield return new WaitForSeconds(0.3f);
        }
        else 
        {
            if (nearCount != null)
            {
                //카운터 위에 오브젝트가 있는지 없는지 확인 
                var counter = nearCount.transform.GetComponent<CounterController>();

                if (!counter.IsPutOn) //올라가 있지 않다면 근처 오브젝트겠지
                {
                    if (counter.gameObject.CompareTag("Crate"))
                    {
                        var spawn = counter.transform.GetComponent<spawn_Ingredient>();
                        if (spawn != null)
                        {
                            TakeHandObject(spawn.PickAnim());
                            Debug.Log("열기");
                            // 생성된 재료 오브젝트 바로 집는 메소드 추가 필요 
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else
                    {
                        if (nearOb != null)
                        {
                            TakeHandObject(nearob);
                        }
                        yield return new WaitForSeconds(0.3f);
                    }

                }
                else
                {
                    //카운터 집을 수 있는 오브젝트가 있고, 도마가 없을때 
                    if (counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Cooker") /* 소화기 태그 추가 필요 */)
                    {
                        TakeHandObject(counter.PutOnOb);
                    }
                    //집을 수 있는 오브젝트가 도마 위에 있을때 
                    else if (counter.ChoppingBoard != null)
                    {
                        TakeHandObject(counter.ChoppingBoard.transform.GetChild(1).gameObject);
                        counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        TakeHandObject(counter.PutOnOb);
                    }

                    counter.PutOnOb = null;
                    counter.ChangePuton();
                    yield return new WaitForSeconds(0.5f);
                }

                yield return new WaitForSeconds(0.5f);
            }
            // 근처 카운터가 없다면(땅바닥이겟지)
            else
            {
                if (nearOb != null)
                {
                    if (nearob.CompareTag("Plate") || nearob.CompareTag("Cooker"))
                    {
                        TakeHandObject(nearob);
                    }
                    else
                    {
                        TakeHandObject(nearob);
                    }

                    yield return new WaitForSeconds(0.3f);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        coroutine = null;
        yield return new WaitForSeconds(0.3f);
    }

    private void DropObject(GameObject nearcounter, GameObject nearob)
    {
        // 근처에 카운터가 있다면 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // 카운터에 올라간게 없다면  
            {
                if (counter.ChoppingBoard == null) // 카운터에 도마가 없는 곳이라면 
                {
                    if (counter.gameObject.CompareTag("Crate")) // 계속 박스 위가 아니라 가운데로 들어감 
                    {
                        HandsOnOb.transform.SetParent(counter.transform);
                        var boxcol = counter.transform.GetComponent<BoxCollider>();
                        Vector3 boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
                        HandsOnOb.transform.position = boxtop;
                    }
                    else
                    {
                        HandsOnOb.transform.SetParent(counter.transform);
                        HandsOnOb.transform.position = counter.transform.position + new Vector3(0, 0.1f, 0);
                    }

                }
                else // 도마가 있다면 
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position + new Vector3(0, 0.1f, 0);
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                }

                HandsOnOb.transform.rotation = Quaternion.identity;
                counter.ChangePuton();
                counter.PutOnOb = HandsOnOb;
                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
            else if (counter.transform.CompareTag("Plate_Return"))
            {
                Debug.Log("드랍못함");
            }
            else // 카운터에 올라가 있는데 드랍하려고 하면 
            {
                Debug.Log("이미올라가있음");
            }
        }
        else // 근처에 카운터 없으면 땅에 떨군다는 
        {
            Debug.Log("emfdjdhsk");
            HandsOnOb.transform.SetParent(null);
            var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.05f;
            rb.angularDrag = 0;
            animator.SetBool("IsTake", false);
            HandsOnOb = null;
            isHolding = false;
        }
    }





    private IEnumerator PlayerCookedChage()
    {
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();

            // 동작만하고 실질적인 처리는 재료가?
            // 도마가 있는지, 도마 자식에 태그가 재료인 오브젝트가 있는지 + 재료 가 썰 수있는 boolean인지 
            if (counter.ChoppingBoard != null)
            {
                if (counter.ChoppingBoard.transform.childCount.Equals(2))
                {
                    if (counter.ChoppingBoard.transform.GetChild(1).gameObject.CompareTag("Ingredients") /* 재료가 썰수있는지  */)
                    {
                        counter.ChoppingBoard.transform.GetChild(1).gameObject.transform.TryGetComponent(out Ingredient ingre);
                        if (ingre != null && ingre.OnChopping)
                        {
                            animator.SetTrigger("Chop");
                        }
                        // 재료 eCooked enum에서 받고 노말일때만 }
                    }
                }
            }
        }

        yield return null;
    }


    private void DropObject()
    {
        // 근처에 카운터가 있다면 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // 카운터에 올라가있지 않다면  
            {
                if (counter.ChoppingBoard == null) // 카운터에 도마가 없는 곳이라면 
                {
                    HandsOnOb.transform.SetParent(nearcounter.transform);
                    HandsOnOb.transform.position = nearcounter.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChangePuton();
                    counter.PutOnOb = HandsOnOb;
                }
                else // 도마가 있다면 
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                    //도마도 끄기 
                    counter.ChangePuton();
                }

                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
            else // 카운터에 올라가 있는데 드랍하려고 하면 
            {
                Debug.Log("이미올라가있음");
            }
        }
        else // 근처에 카운터 없으면 땅에 떨군다는 
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

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        HandsOnOb = gameObject;
        nearcontroller.ChangeOriginEmission(HandsOnOb);
        if (HandsOnOb.transform.TryGetComponent(out Rigidbody rigi))
        {
            Destroy(rigi);
        }
        HandsOnOb.transform.SetParent(Attachtransform);
        HandsOnOb.transform.rotation = Attachtransform.rotation;
        HandsOnOb.transform.position = Attachtransform.position;
        isHolding = true;
    }


}

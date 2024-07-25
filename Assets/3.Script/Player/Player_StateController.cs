using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject HandsOnObject { get => HandsOnOb; }

    //이건 인스펙터에서 셰프 밑에 스켈레톤 Attach 넣어 사용하기
    [SerializeField] private Transform Attachtransform;
    [SerializeField] private GameObject Cleaver;
    public GameObject CleaverOb { get => Cleaver; }

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


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //요리도구 상호작용
                // 굽고 썰고 
                if (!IsHolding)
                {
                    Debug.Log("처음 컨트롤 들어오냐");
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
            Cleaver.SetActive(false);
        }
    }

    private void ThrowIngredients()
    {
        HandsOnOb.transform.SetParent(null);
        var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        rb.angularDrag = 0;
        rb.AddForce(transform.forward * 100f);
        if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
        {
            mesh.enabled = true;
        }
        if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
        {
            col.enabled = true;
        }
        animator.SetBool("IsTake", false);
        HandsOnOb = null;
        isHolding = false;
    }


    private IEnumerator PlayerHodingChange(GameObject nearCount, GameObject nearob)
    {

        // 근처 카운터가 있고 내가 집은 상태가 아니라면 
        if (isHolding)
        {
            DropObject(nearCount, nearob);
            yield break;
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
                            // 생성된 재료 오브젝트 바로 집는 메소드 추가 필요 
                            yield break;
                        }
                    }
                    else // 올라가 있지 않고 근처 오브젝트가 아닐때 근처 주워오기
                    {
                        if (nearOb != null)
                        {
                            TakeHandObject(nearob);
                        }
                        yield break;
                    }

                }
                else
                {
                    //카운터 집을 수 있는 오브젝트가 있고, 도마가 없을때 
                    //if (counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Cooker") /* 소화기 태그 추가 필요 */)
                    //{
                    //    TakeHandObject(counter.PutOnOb);
                    //}
                    ////집을 수 있는 오브젝트가 도마 위에 있을때 
                    //else 
                    if (counter.ChoppingBoard != null)
                    {
                        Debug.Log("들어오긴해?");
                        TakeHandObject(counter.ChoppingBoard.transform.GetChild(1).gameObject);
                        counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else // 아예 나머지들?? 
                    {
                        if(counter.CompareTag("Plate_Return"))
                        {
                            if(counter.TryGetComponent(out Plate_Return platereturn))
                            {
                                TakeHandObject(platereturn.GetPlate());
                                yield break;
                            }          
                            
                        }
                        else
                        {
                            TakeHandObject(counter.PutOnOb);
                        }
                    }

                    counter.PutOnOb = null;
                    counter.ChangePuton();
                    yield break;
                }

                yield break;
            }
            // 근처 카운터가 없다면(땅바닥이겟지)
            else
            {
                if (nearOb != null)
                {
                    TakeHandObject(nearob);
                    yield break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        if (nearCount == null && nearob == null)
        {
            coroutine = null;
            yield break;
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
                    if (counter.gameObject.CompareTag("Crate")) // 들고 있기 때문에 도마가 없고 재료 박스 앞이라면 박스 위에 올린다 
                    {
                        HandsOnOb.transform.SetParent(counter.transform);
                        var boxcol = counter.transform.GetComponent<BoxCollider>();
                        Vector3 boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
                        HandsOnOb.transform.position = boxtop;
                    }
                    else if(counter.gameObject.CompareTag("Pass"))
                    {
                        
                        if(HandsOnOb.TryGetComponent(out Plate plate))
                        {
                            counter.transform.TryGetComponent(out Plate_Spawn plate_spawn);
                            if (plate_spawn.PassPlate(plate))
                            {
                                animator.SetBool("IsTake", false);
                                HandsOnOb = null;
                                isHolding = false;                                
                            }
                            return;                         
                        }
                        else
                        {
                            //접시가 아니면 뭐 패스 
                            return;
                        }
                        

                    }
                    else if (counter.transform.CompareTag("Plate_Return")) //플레이트 쌓이는데는 집는것만 됨 
                    {
                        Debug.Log("드랍못함");
                        return;
                    }
                    else if (counter.transform.CompareTag("TrashCan")) // 쓰레기통은 재료만  
                    {
                        if(HandsOnOb.CompareTag("Plate") /*조리기구 추가*/)
                        {
                            if(HandsOnOb.transform.childCount.Equals(1) && HandsOnOb.name.Contains("_"))
                            {
                                Destroy(HandsOnOb.transform.GetChild(0));
                                return;
                            }
                            else if(HandsOnOb.transform.childCount.Equals(1))
                            {
                                if(HandsOnOb.transform.GetChild(0).TryGetComponent(out Ingredient ingre))
                                {
                                    ingre.Die();
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            HandsOnOb.transform.SetParent(counter.transform);
                        }
                    }
                    else // 들고 있고 도마가 없다면 일반 카운터이기때문에 그냥 둔다, 재료들은 살짝 올려준다 
                    {
                        HandsOnOb.transform.SetParent(counter.transform);
                        if (HandsOnOb.CompareTag("Ingredients"))
                        {
                            HandsOnOb.transform.position = counter.transform.position + new Vector3(0, 0.1f, 0);
                        }
                        else
                        {
                            HandsOnOb.transform.position = counter.transform.position;
                        }
                    }

                }
                else // 도마가 있다면 
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position + new Vector3(0, 0.1f, 0);
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                    if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
                    {
                        mesh.enabled = true;
                    }
                    if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
                    {
                        col.enabled = true;
                    }
                }

                HandsOnOb.transform.rotation = Quaternion.identity;
                counter.ChangePuton();
                counter.PutOnOb = HandsOnOb;
                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
            else // 카운터에 올라가 있는데 드랍하려고 하면 
            {
                //플레이트에 넣을때 
                // 접시만(재료 X) / 접시(재료 O) / 재료 + 재료
                if ((counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Ingredients")) && HandsOnOb.CompareTag("Ingredients"))
                {
                    HandsOnOb.TryGetComponent(out Ingredient ingre);
                    HandsOnOb.TryGetComponent(out AddIngredientSpawn addingre);
                    HandsOnOb.TryGetComponent(out SphereCollider col);
                    if (ingre.OnPlate)
                    {
                        //손에 재료들고 접시만 
                        if (counter.PutOnOb.CompareTag("Plate"))
                        {

                            //접시 재료 ㅇ 
                            if(counter.PutOnOb.transform.childCount > 0)
                            {
                                if (addingre.SetAddIngredient(counter.PutOnOb.transform.GetChild(0).gameObject))
                                {
                                    ingre.Die();
                                    animator.SetBool("IsTake", false);
                                    HandsOnOb = null;
                                    isHolding = false;
                                }
                             
                            }
                            else // 접시 재료 X 
                            {
                                HandsOnOb.transform.SetParent(counter.PutOnOb.transform);
                                HandsOnOb.transform.position = counter.PutOnOb.transform.position;
                                HandsOnOb.transform.rotation = counter.PutOnOb.transform.rotation;
                                col.enabled = false;
                                animator.SetBool("IsTake", false);
                                HandsOnOb = null;
                                isHolding = false;
                            }
                        }
                        else // 재료재료
                        {
                            if (addingre.SetAddIngredient(counter.PutOnOb))
                            {
                                ingre.Die();
                                animator.SetBool("IsTake", false);
                                HandsOnOb = null;
                                isHolding = false;
                            }
                            else
                            {
                                Debug.Log("못내림");
                            }
                        }
                    }
                }


            }
        }
        else // 근처에 카운터 없으면 땅에 떨군다는 
        {
            HandsOnOb.transform.SetParent(null);
            var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.05f;
            rb.angularDrag = 0;
            if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
            {
                mesh.enabled = true;
            }
            if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
            {
                col.enabled = true;
            }
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
                            Cleaver.SetActive(true);
                        }
                        // 재료 eCooked enum에서 받고 노말일때만 }
                    }
                }
            }
        }

        yield return null;
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
        if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
        {
            mesh.enabled = false;
        }
        if(HandsOnOb.transform.TryGetComponent(out SphereCollider col))
        {
            col.enabled = false;
        }
        HandsOnOb.transform.SetParent(Attachtransform);
        HandsOnOb.transform.rotation = Attachtransform.rotation;
        HandsOnOb.transform.position = Attachtransform.position;
        isHolding = true;
    }


}

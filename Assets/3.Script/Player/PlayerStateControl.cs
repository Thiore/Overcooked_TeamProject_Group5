using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateControl : MonoBehaviour
{
    private Animator animator;

    private GameObject nearOb = null;
    private GameObject nearCounter = null;
    private GameObject HandsOnOb = null;

    [SerializeField] private Transform AttachTransform;
    [SerializeField] private GameObject cleaver;
    public GameObject Cleaver { get => cleaver; }

    private CounterEmissionController counterEmissionController;
    private NearObject_EmissionController nearObjectEmissionController;
    [SerializeField] private Player_SwapManager playerSwapManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        counterEmissionController = GetComponent<CounterEmissionController>();
        nearObjectEmissionController = GetComponent<NearObject_EmissionController>();
        playerSwapManager = GetComponentInParent<Player_SwapManager>();
    }

    private void Update()
    {
        nearCounter = counterEmissionController.GetNearCounter();
        nearOb = nearObjectEmissionController.GetNearObject();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (HandsOnOb == null)
            {
                StartCoroutine(PickUpDbjectCheck(nearCounter, nearOb));
            }
            else
            {
                StartCoroutine(DropDownObjectCheck(nearCounter, nearOb));
            }
        }


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            if (HandsOnOb == null && nearCounter != null)
            {
                if (nearCounter.TryGetComponent(out CounterController counter))
                {
                    if (counter.ChoppingBoard != null && counter.PutOnOb.CompareTag("Ingredients"))
                    {
                        Debug.Log("잘라좀");

                        StartCoroutine(PlayerCookedChage());
                    }
                }
            }

            if (HandsOnOb == null && nearCounter != null)
            {
                if (nearCounter.CompareTag("Sink"))
                {
                    if (nearCounter.TryGetComponent(out Sink sink))
                    {
                        StartCoroutine(PlayerWashPlate());
                    }
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

    private IEnumerator DropDownObjectCheck(GameObject nearCounter, GameObject nearOb)
    {
        // 놓을때 > 이미 Handsob가 있을때로 가정해서 들어오는거임 
        if (nearCounter != null)
        {
            // 카운터가 널이 아닐때 카운터 위에 체크 
            var counter = nearCounter.GetComponent<CounterController>();

            //접시 줍는곳은 드랍은 일절 X 
            if (counter.CompareTag("Plate_Return"))
            {
                yield break;
            }
            else if (counter.CompareTag("Sink"))
            {
                if (counter.TryGetComponent(out Sink sink))
                {
                    if (!sink.CheckInWaterPlate(HandsOnOb))
                    {
                        yield break;
                    }
                    else
                    {
                        animator.SetBool("IsTake", false);
                        HandsOnOb = null;
                        yield break;
                    }
                }
            }
            //여기 제출하는 부분
            else if(counter.CompareTag("Pass"))
            {
                if (HandsOnOb.TryGetComponent(out Plate plate))
                {
                    counter.transform.TryGetComponent(out Plate_Spawn plate_spawn);
                    if (plate_spawn.PassPlate(plate))
                    {
                        animator.SetBool("IsTake", false);
                        HandsOnOb = null;
                    }
                    yield break;
                }
                else
                {
                    //접시가 아니면 뭐 패스 
                    yield break;
                }
            }

            

            // 카운터에 올라간게 null 이 아니면 올라간거 쿠킹툴인지, 손에 쥔걸 내릴수있는지 판단
            if (counter.PutOnOb != null && counter.PutOnOb.TryGetComponent(out Cookingtool tool))
            {
                if (tool is FryingPan)
                {
                    //쿡체크메소드(HandsOnb);
                    tool.CookedCheck(HandsOnOb);
                }
                else if (tool is Pot)
                {
                    tool.CookedCheck(HandsOnOb);
                } // 추후 else if로 다른 요리도구 추가 가능 
                else
                {
                    // 아무것도 안걸렸으면 손에 다른걸 들었거나 상호작용이 안되는거겠지
                    yield break;
                }
            }
            else if (counter.PutOnOb != null && counter.PutOnOb.TryGetComponent(out Plate plate))
            {
                if(HandsOnOb.TryGetComponent(out Ingredient Ingre))
                {
                    if(plate.OnPlate(Ingre))
                    {
                        Debug.Log("재료 넣기");
                        animator.SetBool("IsTake", false);
                        HandsOnOb = null;
                        yield break;
                    }
                    else
                    {
                        yield break;
                    }
                }
               
            }
            else if (counter.PutOnOb == null)
            //카운터가 근처에 있고, 카운터가 쿠킹툴이 아닐때(일반이겠지, 싱크대도 고려해야하나)
            {
                //드랍메소드
                if (counter.CompareTag("TrashCan"))
                {
                    //쓰레기통
                    if (counter.TryGetComponent(out TrashCanController trash))
                    {
                        if (HandsOnOb.transform.childCount > 0)
                        {
                            Destroy(HandsOnOb.transform.GetChild(0));
                            yield return new WaitForSeconds(0.3f);
                        }

                        if (HandsOnOb.CompareTag("Ingredients"))
                        {
                            trash.PutOnOb = HandsOnOb;
                            HandsOnOb.transform.SetParent(counter.transform);
                            trash.IsPutOn = true;
                            yield return StartCoroutine(trash.DropTrash_co());
                            trash.PutOnOb = null;
                            trash.IsPutOn = false;
                        }
                    }
                }
                else if (counter.ChoppingBoard != null)
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position;
                    HandsOnOb.transform.rotation = counter.ChoppingBoard.transform.rotation;
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.3f);
                }
                else
                {
                    HandsOnOb.transform.SetParent(counter.transform);

                    if (counter.CompareTag("Crate"))
                    {
                        var boxcol = counter.transform.GetComponent<BoxCollider>();
                        Vector3 boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
                        HandsOnOb.transform.position = boxtop;
                    }
                    else
                    {
                        HandsOnOb.transform.position = counter.transform.position;
                    }
                    HandsOnOb.transform.rotation = counter.transform.rotation;
                    yield return new WaitForSeconds(0.3f);
                }

                if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
                {
                    mesh.enabled = true;
                }
                if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
                {
                    col.enabled = true;
                }

                animator.SetBool("IsTake", false);
                counter.PutOnOb = HandsOnOb;
                counter.ChangePuton();
                HandsOnOb = null;
            }
        }
        else
        {
            animator.SetBool("IsTake", false);
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
            HandsOnOb = null;
            yield return new WaitForSeconds(0.3f);

        }

        yield return new WaitForSeconds(0.3f);
    }


    private IEnumerator PickUpDbjectCheck(GameObject nearCounter, GameObject nearOb)
    {
        if (nearCounter != null)
        {
            if (nearCounter.TryGetComponent(out CounterController counterController))
            {
                if (counterController.PutOnOb == null)
                {
                    if (counterController.gameObject.CompareTag("Crate"))
                    {
                        if (counterController.transform.TryGetComponent(out spawn_Ingredient spawn))
                        {
                            PickUpDbject(spawn.PickAnim());
                            yield break;
                        }
                    }
                    else if (nearOb != null)
                    {
                        PickUpDbject(nearOb);
                        yield break;
                    }
                }
                else
                {
                    if (counterController.ChoppingBoard != null)
                    {
                        counterController.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                    }

                    if (counterController.CompareTag("Plate_Return"))
                    {
                        if (counterController.TryGetComponent(out Plate_Return platereturn))
                        {
                            PickUpDbject(platereturn.GetPlate());
                            yield break;
                        }
                    }

                    PickUpDbject(counterController.PutOnOb);
                    counterController.PutOnOb = null;
                    counterController.ChangePuton();
                    yield break;
                }
            }
        }
        else
        {
            if (nearOb != null)
                PickUpDbject(nearOb);
            yield break;
        }
    }



    private void PickUpDbject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        HandsOnOb = gameObject;
        nearObjectEmissionController.ChangeOriginEmission(HandsOnOb);
        if (HandsOnOb.transform.TryGetComponent(out Rigidbody rigi))
        {
            Destroy(rigi);
        }
        if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
        {
            mesh.enabled = false;
        }
        if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
        {
            col.enabled = false;
        }
        HandsOnOb.transform.SetParent(AttachTransform);
        HandsOnOb.transform.rotation = AttachTransform.rotation;
        HandsOnOb.transform.position = AttachTransform.position;
    }


    private IEnumerator PlayerCookedChage()
    {
        if (nearCounter != null)
        {
            var counter = nearCounter.transform.GetComponent<CounterController>();

            // 동작만하고 실질적인 처리는 재료가?
            // 도마가 있는지, 도마 자식에 태그가 재료인 오브젝트가 있는지 + 재료 가 썰 수있는 boolean인지 
            if (counter.ChoppingBoard != null)
            {
                if (counter.ChoppingBoard.transform.childCount.Equals(2))
                {
                    if (counter.ChoppingBoard.transform.GetChild(1).gameObject.CompareTag("Ingredients") /* 재료가 썰수있는지  */)
                    {
                        counter.ChoppingBoard.transform.GetChild(1).gameObject.transform.TryGetComponent(out Ingredient ingre);

                        if (ingre.Chopable())
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

    public IEnumerator PlayerWashPlate()
    {
        //이미 싱크대인거 검수하고 들어옴 
        if (nearCounter != null)
        {
            if (nearCounter.TryGetComponent(out Sink sink))
            {
                if (sink.transform.GetChild(0).childCount > 0)
                {
                    animator.SetTrigger("Wash");
                }
            }
        }

        yield return null;
    }



}

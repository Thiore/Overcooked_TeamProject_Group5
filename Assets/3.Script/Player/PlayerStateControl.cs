using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateControl : MonoBehaviour
{
    private Animator animator;

    private GameObject nearOb = null;
    private GameObject nearCounter = null;
    private GameObject HandsOnOb = null;

    public GameObject HandsOnObGet {  get { return HandsOnOb; } }

    [SerializeField] private Transform AttachTransform;
    [SerializeField] private GameObject cleaver;
    public GameObject Cleaver { get => cleaver; }

    private CounterEmissionController counterEmissionController;
    private NearObject_EmissionController nearObjectEmissionController;
    [SerializeField] private Player_SwapManager playerSwapManager;

    [SerializeField] private GameObject[] playerFaces;

    private Coroutine stateCo = null;

    private WaitForSeconds AnimTime = new WaitForSeconds(0.2f);

    private CounterController resetChoppingBoard = null;

    private void Awake()
    {
        var facenum = GameManager.Instance.Faceindex;
        for (int i = 0; i < playerFaces.Length; i++)
        {
            playerFaces[i].SetActive(false);
        }

        playerFaces[facenum].SetActive(true);

        animator = GetComponent<Animator>();
        counterEmissionController = GetComponent<CounterEmissionController>();
        nearObjectEmissionController = GetComponent<NearObject_EmissionController>();
        playerSwapManager = GetComponentInParent<Player_SwapManager>();
    }

    private void Update()
    {
        nearCounter = counterEmissionController.GetNearCounter();
        if (HandsOnOb == null)
            nearOb = nearObjectEmissionController.GetNearObject();

        if(stateCo == null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (HandsOnOb == null)
                {
                    if(nearCounter != null)
                    {
                        stateCo = StartCoroutine(PickUpObjectCheck(nearCounter, nearOb));
                    }
                    else
                    {
                        if(nearOb!=null)
                            PickUpObject(nearOb);
                    }
                }
                else
                {
                    stateCo = StartCoroutine(DropDownObjectCheck(nearCounter));
                }
            }
        }
        if (stateCo == null)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {

                if (HandsOnOb == null && nearCounter != null)
                {
                    if (nearCounter.TryGetComponent(out CounterController counter))
                    {
                        if (counter.Counter.Equals(eCounter.ChoppingBoard) && counter.PutOnOb.CompareTag("Ingredients"))
                        {
                            Debug.Log("잘라좀");

                            stateCo = StartCoroutine(PlayerCookedChage());
                        }
                    }
                }
            }
        }
        if (stateCo == null)
        {
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                if (HandsOnOb != null && HandsOnOb.CompareTag("Ingredients"))
                {
                    Debug.Log("던지기");
                    ThrowIngredients();
                }
            }
        }
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop")&& Cleaver.activeSelf)
        {
            Cleaver.SetActive(false);
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop") && !Cleaver.activeSelf)
        {
            Cleaver.SetActive(true);
            resetChoppingBoard = nearCounter.GetComponent<CounterController>();
            for(int i = 0; i < resetChoppingBoard.playerAnim.Length;i++)
            {
                if (resetChoppingBoard.playerAnim[i] != null && resetChoppingBoard.playerAnim[i] == animator)
                    break;
                else
                {
                    resetChoppingBoard.playerAnim[i] = animator;
                }
            }
            
        }
        
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if ((other.transform.CompareTag("ChoppingBoard") && animator.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Chop"))||
    //        (other.transform.CompareTag("Sink")&& animator.GetCurrentAnimatorStateInfo(0).IsName("New_Chef@Wash")))
    //    {
    //        animator.SetTrigger("Finish");
    //        //animator.SetBool("IsWalking", false);
    //        Cleaver.SetActive(false);
    //    }
    //}

    private IEnumerator DropDownObjectCheck(GameObject nearCounter)
    {
        // 놓을때 > 이미 Handsob가 있을때로 가정해서 들어오는거임 
        if (nearCounter != null)
        {
            // 카운터가 널이 아닐때 카운터 위에 체크 
            var counter = nearCounter.GetComponent<CounterController>();

            switch (counter.Counter)
            {
                case eCounter.counter:
                    if (counter.IsPutOn)
                    {
                        if (counter.PutOnOb.CompareTag(("Plate")))
                        {
                            if (IngreOnPlate(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        else if(counter.PutOnOb.CompareTag("Cooker"))
                        {
                            if(IngreOnCooker(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        break;
                    }
                    else
                    {
                        HandsOnOb.transform.SetParent(counter.transform);
                        HandsOnOb.transform.position = counter.transform.position;
                        HandsOnOb.transform.rotation = counter.transform.rotation;
                       
                        animator.SetBool("IsTake", false);
                        counter.PutOnOb = HandsOnOb;
                        counter.ChangePuton();
                        HandsOnOb = null;
                        yield return AnimTime;
                    }
                    break;
                case eCounter.GasRange:
                    if (counter.IsPutOn)
                    {
                        if (HandsOnOb.TryGetComponent(out Ingredient ingre))
                        {
                            if (!ingre.isCook)
                            {
                                if (ingre.Cookable())
                                {
                                    if (counter.PutOnOb.TryGetComponent(out Cookingtool tool))
                                    {
                                        if (tool is Pot && ingre.utensils.Equals(eCookutensils.Pot))
                                        {
                                            tool.ResetCook(ingre);
                                            tool.StartCook();
                                            animator.SetBool("IsTake", false);
                                            HandsOnOb = null;
                                            yield return AnimTime;
                                        }
                                        else if (tool is FryingPan && ingre.utensils.Equals(eCookutensils.Pan))
                                        {
                                            tool.ResetCook(ingre);
                                            tool.StartCook();
                                            animator.SetBool("IsTake", false);
                                            HandsOnOb = null;
                                            yield return AnimTime;
                                        }

                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (HandsOnOb.CompareTag("Cooker"))
                        {
                            
                            HandsOnOb.transform.SetParent(counter.transform);
                            HandsOnOb.GetComponent<Cookingtool>().StartCook();
                            HandsOnOb.transform.position = counter.transform.position;
                            HandsOnOb.transform.rotation = counter.transform.rotation;
                            animator.SetBool("IsTake", false);
                            counter.PutOnOb = HandsOnOb;
                            counter.ChangePuton();
                            HandsOnOb = null;
                            yield return AnimTime;
                        }
                    }
                    break;
                case eCounter.GasStation:
                    if (counter.IsPutOn)
                    {
                        if (HandsOnOb.TryGetComponent(out Ingredient ingre))
                        {
                            if (!ingre.isCook)
                            {
                                if (ingre.Cookable())
                                {
                                    if (counter.PutOnOb.TryGetComponent(out Cookingtool tool))
                                    {
                                        if (tool is Pot && ingre.utensils.Equals(eCookutensils.Fry))
                                        {
                                            tool.ResetCook(ingre);
                                            tool.StartCook();
                                            animator.SetBool("IsTake", false);
                                            HandsOnOb = null;
                                            yield return AnimTime;
                                        }

                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        if (HandsOnOb.CompareTag("Fryer"))
                        {
                            HandsOnOb.transform.SetParent(counter.transform);
                            HandsOnOb.GetComponent<Cookingtool>().StartCook();
                            HandsOnOb.transform.position = counter.transform.position;
                            HandsOnOb.transform.rotation = counter.transform.rotation;
                            animator.SetBool("IsTake", false);
                            counter.PutOnOb = HandsOnOb;
                            counter.ChangePuton();
                            HandsOnOb = null;
                            yield return AnimTime;
                        }

                    }
                    break;
                case eCounter.TrashCan:
                    //쓰레기통
                    if (counter.TryGetComponent(out TrashCanController trash))
                    {
                        if (HandsOnOb.CompareTag("Plate"))
                        {
                            for (int i = 0; i < HandsOnOb.transform.childCount; i++)
                            {
                                if (HandsOnOb.transform.GetChild(i).gameObject.activeSelf)
                                {
                                    HandsOnOb.transform.GetChild(i).gameObject.SetActive(false);
                                }
                            }
                            HandsOnOb = null;
                            animator.SetBool("IsTake", false);
                            yield return AnimTime;
                        }

                        if (HandsOnOb.CompareTag("Ingredients"))
                        {
                            trash.PutOnOb = HandsOnOb;
                            animator.SetBool("IsTake", false);
                            HandsOnOb.transform.SetParent(counter.transform);
                            HandsOnOb.transform.position = counter.transform.position;
                            HandsOnOb.transform.rotation = counter.transform.rotation;
                            trash.IsPutOn = true;
                            StartCoroutine(trash.DropTrash_co());
                            yield return AnimTime;
                        }
                    }
                    break;
                case eCounter.Pass:
                    if (HandsOnOb.TryGetComponent(out Plate plate))
                    {
                        counter.transform.TryGetComponent(out Plate_Spawn plate_spawn);
                        if (plate_spawn.PassPlate(plate))
                        {

                            HandsOnOb = null;
                            animator.SetBool("IsTake", false);
                            yield return AnimTime;
                        }
                    }
                    break;
                case eCounter.ChoppingBoard:
                    if(counter.IsPutOn)
                    {
                        if (counter.PutOnOb.CompareTag(("Plate")))
                        {
                            if (IngreOnPlate(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        else if (counter.PutOnOb.CompareTag("Cooker"))
                        {
                            if (IngreOnCooker(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        break;
                    }
                    else
                    {
                        HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                        HandsOnOb.transform.position = counter.ChoppingBoard.transform.position;
                        HandsOnOb.transform.rotation = counter.ChoppingBoard.transform.rotation;
                        counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                        counter.PutOnOb = HandsOnOb;
                        counter.ChangePuton();
                        HandsOnOb = null;
                        
                        animator.SetBool("IsTake", false);
                        yield return AnimTime;
                    }
                    break;
                case eCounter.Crate:
                    if (counter.IsPutOn)
                    {
                        if (counter.PutOnOb.CompareTag(("Plate")))
                        {
                            if (IngreOnPlate(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        else if (counter.PutOnOb.CompareTag("Cooker"))
                        {
                            if (IngreOnCooker(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        break;
                    }
                    else if (!counter.IsPutOn)
                    {
                        HandsOnOb.transform.SetParent(counter.transform);
                        var boxcol = counter.transform.GetComponent<BoxCollider>();
                        Vector3 boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
                        HandsOnOb.transform.position = boxtop;
                        HandsOnOb.transform.rotation = counter.transform.rotation;
                        counter.PutOnOb = HandsOnOb;
                        counter.ChangePuton();
                        HandsOnOb = null;
                        animator.SetBool("IsTake", false);
                        
                        yield return AnimTime;
                    }
                    break;
                case eCounter.Sink:
                    if (HandsOnOb.CompareTag("Plate"))
                    {
                        if (counter.TryGetComponent(out Sink sink))
                        {
                            if (sink.CheckInWaterPlate(HandsOnOb))
                            {
                                HandsOnOb = null;
                                animator.SetBool("IsTake", false);
                                yield return AnimTime;
                            }
                        }
                    }
                    break;
                case eCounter.Plate_Return:
                    //접시 줍는곳은 드랍은 일절 X 
                    break;
            }
            stateCo = null;
            yield break;
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
            yield return AnimTime;
            stateCo = null;
            yield break;
        }

    }


    private IEnumerator PickUpObjectCheck(GameObject nearCounter,GameObject nearobj)
    {
        if (nearCounter.TryGetComponent(out CounterController counterController))
        {
            switch (counterController.Counter)
            {
                case eCounter.counter:
                    if(counterController.IsPutOn)
                    {
                        PickUpObject(counterController.PutOnOb);
                        counterController.PutOnOb = null;
                        counterController.ChangePuton();
                        yield return AnimTime;
                    }
                    break;
                case eCounter.GasRange:
                    if(counterController.IsPutOn)
                    {
                        if (counterController.transform.GetChild(0).gameObject.activeSelf)
                            counterController.transform.GetChild(0).gameObject.SetActive(false);
                        PickUpObject(counterController.transform.GetChild(1).gameObject);
                        //counterController.transform.GetChild(1).transform.SetParent(null);
                        counterController.PutOnOb = null;
                        counterController.ChangePuton();
                        yield return AnimTime;
                    }
                    break;
                case eCounter.GasStation:
                    if (counterController.IsPutOn)
                    {
                        if (counterController.transform.GetChild(0).gameObject.activeSelf)
                            counterController.transform.GetChild(0).gameObject.SetActive(false);
                        
                        PickUpObject(counterController.transform.GetChild(1).gameObject);
                        //counterController.transform.GetChild(1).transform.SetParent(null);
                        counterController.PutOnOb = null;
                        counterController.ChangePuton();
                        yield return AnimTime;
                    }
                    break;
                case eCounter.ChoppingBoard:
                    if (counterController.IsPutOn)
                    {
                        counterController.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                        counterController.ChoppingBoard.transform.GetChild(1).gameObject.SetActive(true);
                        PickUpObject(counterController.PutOnOb);
                        counterController.PutOnOb = null;
                        counterController.ChangePuton();
                        yield return AnimTime;
                    }
                    break;
                case eCounter.Crate:
                    if (!counterController.IsPutOn)
                    {
                        if (counterController.transform.TryGetComponent(out spawn_Ingredient spawn))
                        {
                            PickUpObject(spawn.PickAnim());
                            yield return AnimTime;
                        }
                    }
                    else
                    {
                        PickUpObject(counterController.PutOnOb);
                        counterController.PutOnOb = null;
                        counterController.ChangePuton();
                        yield return AnimTime;
                    }
                    break;
                case eCounter.Sink:
                    if (!PlayerPlate())
                    {
                        yield return AnimTime;
                    }
                    else
                    {
                        counterController.PutOnOb = null;
                        counterController.ChangePuton();
                        yield return AnimTime;                        
                    }
                    break;
                case eCounter.Plate_Return:
                    if (counterController.TryGetComponent(out Plate_Return platereturn))
                    {
                        PickUpObject(platereturn.GetPlate());
                        yield return AnimTime;
                    }
                    break;
            }
        }
        if(HandsOnOb==null)
        {
            if(nearobj !=null)
            {
                PickUpObject(nearobj);
                 yield return AnimTime;
            }
        }
        stateCo = null;
        yield break;
    }



    private void PickUpObject(GameObject gameObject)
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

            if (counter.ChoppingBoard.transform.childCount.Equals(3))
            {
                counter.ChoppingBoard.transform.GetChild(2).gameObject.transform.TryGetComponent(out Ingredient ingre);

                if (ingre.Chopable())
                {
                    AnimatorStateInfo AnimInfo = animator.GetCurrentAnimatorStateInfo(0);
                    if (!AnimInfo.IsName("New_Chef@Chop"))
                    {
                        animator.SetTrigger("Chop");
                        yield return new WaitForSeconds(0.1f);

                        for (int i = 0; i < counter.playerAnim.Length; i++)
                        {
                            if (counter.playerAnim[i] != null && counter.playerAnim[i] == animator)
                            {
                                stateCo = null;
                                yield break;

                            }
                            else
                            {
                                counter.playerAnim[i] = animator;
                                stateCo = null;
                                yield break;
                            }
                        }

                    }
                }
                // 재료 eCooked enum에서 받고 노말일때만 }
            }

        }
        stateCo = null;
        yield break;
    }

    public bool PlayerPlate()
    {
        //이미 싱크대인거 검수하고 들어옴 
        if (nearCounter != null)
        {
            if (nearCounter.TryGetComponent(out Sink sink))
            {
                if(sink.CheckPos(transform))
                {
                    if (sink.InPlate.Count > 0)
                    {
                        animator.SetTrigger("Wash");
                        return false;
                    }
                }
                else
                {
                    if(sink.outPlate.Count > 0)
                    {
                        
                        PickUpObject(sink.GetPlate());
                        return true;
                    }
                }
                
            }
        }
        return false;
    }

    private void ThrowIngredients()
    {
        
        var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        rb.angularDrag = 0;
        if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
        {
            mesh.enabled = true;
        }
        if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
        {
            col.enabled = true;
        }
        HandsOnOb.transform.SetParent(null);
        rb.AddForce(transform.forward * 100f);
       
        animator.SetBool("IsTake", false);
        HandsOnOb = null;
        stateCo = null;
    }

    private bool IngreOnPlate(CounterController counter)
    {
        if (counter.PutOnOb.TryGetComponent(out Plate plate))
        {
            if (HandsOnOb.TryGetComponent(out Ingredient Ingre))
            {
                if (plate.OnPlate(Ingre))
                {
                    Ingre.Die();
                    Debug.Log("재료 넣기");
                    animator.SetBool("IsTake", false);
                    HandsOnOb = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (HandsOnOb.CompareTag("Cooker"))
            {
                if(HandsOnOb.transform.GetChild(1).TryGetComponent(out Ingredient ingre))
                {
                    if (plate.OnPlate(ingre))
                    {
                        ingre.Die();
                        HandsOnOb.transform.GetChild(0).gameObject.SetActive(false);
                        Debug.Log("재료 넣기");
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
        }
        return false;
    }

    private bool IngreOnCooker(CounterController counter)
    {
        if(counter.PutOnOb.transform.childCount.Equals(1))
        {
            if (HandsOnOb.TryGetComponent(out Ingredient Ingre))
            {
                if (!Ingre.isCook)
                {
                    if (Ingre.Cookable())
                    {
                        if (counter.PutOnOb.TryGetComponent(out Cookingtool tool))
                        {
                            if (tool is Pot && Ingre.utensils.Equals(eCookutensils.Pot))
                            {
                                tool.ResetCook(Ingre);
                                return true;
                            }
                            else if (tool is FryingPan && Ingre.utensils.Equals(eCookutensils.Pan))
                            {
                                tool.ResetCook(Ingre);
                                return true;
                            }
                            else if (tool is FryingPan && Ingre.utensils.Equals(eCookutensils.Fry))
                            {
                                tool.ResetCook(Ingre);
                                return true;
                            }

                        }
                    }
                }

            }

        }
        
        return false;
    }
}

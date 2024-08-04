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
        nearOb = nearObjectEmissionController.GetNearObject();

        if(stateCo == null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (HandsOnOb == null)
                {
                    stateCo = StartCoroutine(PickUpDbjectCheck(nearCounter, nearOb));
                }
                else
                {
                    stateCo = StartCoroutine(DropDownObjectCheck(nearCounter));
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
                            Debug.Log("�߶���");

                            stateCo = StartCoroutine(PlayerCookedChage());
                        }
                    }
                }

                if (HandsOnOb == null && nearCounter != null)
                {
                    if (nearCounter.CompareTag("Sink"))
                    {
                        if (nearCounter.TryGetComponent(out Sink sink))
                        {
                            stateCo = StartCoroutine(PlayerWashPlate());
                        }
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                if (HandsOnOb != null && HandsOnOb.CompareTag("Ingredients"))
                {
                    Debug.Log("������");
                    ThrowIngredients();
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

    private IEnumerator DropDownObjectCheck(GameObject nearCounter)
    {
        // ������ > �̹� Handsob�� �������� �����ؼ� �����°��� 
        if (nearCounter != null)
        {
            // ī���Ͱ� ���� �ƴҶ� ī���� ���� üũ 
            var counter = nearCounter.GetComponent<CounterController>();

            switch (counter.Counter)
            {
                case eCounter.counter:
                    if (counter.PutOnOb != null)
                    {
                        if (counter.PutOnOb.CompareTag(("Plate")))
                        {
                            if (IngreOnPlate(counter))
                            {
                                yield return AnimTime;
                            }
                        }
                        break;
                    }
                    else
                        HandsOnOb.transform.SetParent(counter.transform);
                    HandsOnOb.transform.position = counter.transform.position;
                    HandsOnOb.transform.rotation = counter.transform.rotation;
                    break;
                case eCounter.GasRange:
                    if (counter.PutOnOb != null)
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
                                            break;
                                        }
                                        else if (tool is FryingPan && ingre.utensils.Equals(eCookutensils.Pan))
                                        {
                                            break;
                                        }

                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (HandsOnOb.CompareTag("Pot"))
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
                        else if (HandsOnOb.CompareTag("Pan"))
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
                    }
                    break;
                case eCounter.GasStation:
                    if (counter.PutOnOb != null)
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
                                            break;
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
                    //��������
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

                            yield return AnimTime;
                        }

                        if (HandsOnOb.CompareTag("Ingredients"))
                        {
                            trash.PutOnOb = HandsOnOb;
                            animator.SetBool("IsTake", false);
                            HandsOnOb.transform.SetParent(counter.transform);
                            trash.IsPutOn = true;
                            StartCoroutine(trash.DropTrash_co());
                            trash.PutOnOb = null;
                            trash.IsPutOn = false;
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
                            yield return AnimTime;
                        }
                    }
                    break;
                case eCounter.ChoppingBoard:
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position;
                    HandsOnOb.transform.rotation = counter.ChoppingBoard.transform.rotation;
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                    yield return AnimTime;
                    break;
                case eCounter.Crate:
                    HandsOnOb.transform.SetParent(counter.transform);
                    var boxcol = counter.transform.GetComponent<BoxCollider>();
                    Vector3 boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
                    HandsOnOb.transform.position = boxtop;
                    HandsOnOb.transform.rotation = counter.transform.rotation;
                    break;
                case eCounter.Sink:
                    if (counter.TryGetComponent(out Sink sink))
                    {
                        if (sink.CheckInWaterPlate(HandsOnOb))
                        {
                            HandsOnOb = null;
                            animator.SetBool("IsTake", false);
                            yield return AnimTime;
                        }
                    }
                    break;
                case eCounter.Plate_Return:
                    //���� �ݴ°��� ����� ���� X 
                    stateCo = null;
                    yield break;
            }
        }











        //if (counter.PutOnOb == null)
        ////ī���Ͱ� ��ó�� �ְ�, ī���Ͱ� ��ŷ���� �ƴҶ�(�Ϲ��̰���, ��ũ�뵵 ����ؾ��ϳ�)
        //{
        //    //����޼ҵ�
            
        //    else if (counter.ChoppingBoard != null)
        //    {
                
        //    }
        //    else
        //    {
        //        HandsOnOb.transform.SetParent(counter.transform);

        //        if (counter.CompareTag("Crate"))
        //        {
        //            var boxcol = counter.transform.GetComponent<BoxCollider>();
        //            Vector3 boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
        //            HandsOnOb.transform.position = boxtop;
        //        }
        //        else
        //        {
        //            HandsOnOb.transform.position = counter.transform.position;
        //        }
        //        HandsOnOb.transform.rotation = counter.transform.rotation;
        //        yield return new WaitForSeconds(0.3f);
        //    }

        //    if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
        //    {
        //        mesh.enabled = true;
        //    }
        //    if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
        //    {
        //        col.enabled = true;
        //    }

        //    animator.SetBool("IsTake", false);
        //    //counter.PutOnOb = HandsOnOb;
        //    //counter.ChangePuton();
        //    HandsOnOb = null;
        //    yield return new WaitForSeconds(0.2f);
        //}
        
        //else
        //{
        //    animator.SetBool("IsTake", false);
        //    HandsOnOb.transform.SetParent(null);
        //    var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
        //    rb.mass = 0.05f;
        //    rb.angularDrag = 0;
        //    if (HandsOnOb.transform.TryGetComponent(out MeshCollider mesh))
        //    {
        //        mesh.enabled = true;
        //    }
        //    if (HandsOnOb.transform.TryGetComponent(out SphereCollider col))
        //    {
        //        col.enabled = true;
        //    }
        //    HandsOnOb = null;
        //    yield return new WaitForSeconds(0.2f);

        //}
        //stateCo = null;
        //yield break;
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
                            stateCo = null;
                            yield break;
                        }
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
                            stateCo = null;
                            yield break;
                        }
                    }
                    else if(counterController.CompareTag("GasRange"))
                    {
                        if(counterController.transform.childCount.Equals(2))
                        {
                            counterController.transform.GetChild(0).gameObject.SetActive(false);
                            counterController.transform.GetChild(1).transform.SetParent(null);
                            PickUpDbject(counterController.transform.GetChild(1).gameObject);
                            counterController.PutOnOb = null;
                            counterController.ChangePuton();
                            stateCo = null;
                            yield break;
                        }
                    }

                    PickUpDbject(counterController.PutOnOb);
                    counterController.PutOnOb = null;
                    counterController.ChangePuton();
                    stateCo = null;
                    yield break;
                }
            }

        }

        if (nearOb != null)
            PickUpDbject(nearOb);
        stateCo = null;
        yield break;

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

            // ���۸��ϰ� �������� ó���� ��ᰡ?
            // ������ �ִ���, ���� �ڽĿ� �±װ� ����� ������Ʈ�� �ִ��� + ��� �� �� ���ִ� boolean���� 
            if (counter.ChoppingBoard != null)
            {
                if (counter.ChoppingBoard.transform.childCount.Equals(2))
                {
                    if (counter.ChoppingBoard.transform.GetChild(1).gameObject.CompareTag("Ingredients") /* ��ᰡ ����ִ���  */)
                    {
                        counter.ChoppingBoard.transform.GetChild(1).gameObject.transform.TryGetComponent(out Ingredient ingre);

                        if (ingre.Chopable())
                        {
                            animator.SetTrigger("Chop");
                            Cleaver.SetActive(true);
                        }
                        // ��� eCooked enum���� �ް� �븻�϶��� }
                    }
                }
            }
        }
        stateCo = null;
        yield break;
    }

    public IEnumerator PlayerWashPlate()
    {
        //�̹� ��ũ���ΰ� �˼��ϰ� ���� 
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
        stateCo = null;
        yield break;
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
    }

    private bool IngreOnPlate(CounterController counter)
    {
        if (counter.PutOnOb.TryGetComponent(out Plate plate))
        {
            if (HandsOnOb.TryGetComponent(out Ingredient Ingre))
            {
                if (plate.OnPlate(Ingre))
                {
                    Debug.Log("��� �ֱ�");
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
                        Debug.Log("��� �ֱ�");
                        animator.SetBool("IsTake", false);
                        HandsOnOb = null;
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

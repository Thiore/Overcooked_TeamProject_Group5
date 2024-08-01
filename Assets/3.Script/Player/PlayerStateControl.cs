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
                        Debug.Log("�߶���");

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
        // ������ > �̹� Handsob�� �������� �����ؼ� �����°��� 
        if (nearCounter != null)
        {
            // ī���Ͱ� ���� �ƴҶ� ī���� ���� üũ 
            var counter = nearCounter.GetComponent<CounterController>();

            //���� �ݴ°��� ����� ���� X 
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
            //���� �����ϴ� �κ�
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
                    //���ð� �ƴϸ� �� �н� 
                    yield break;
                }
            }

            

            // ī���Ϳ� �ö󰣰� null �� �ƴϸ� �ö󰣰� ��ŷ������, �տ� ��� �������ִ��� �Ǵ�
            if (counter.PutOnOb != null && counter.PutOnOb.TryGetComponent(out Cookingtool tool))
            {
                if (tool is FryingPan)
                {
                    //��üũ�޼ҵ�(HandsOnb);
                    tool.CookedCheck(HandsOnOb);
                }
                else if (tool is Pot)
                {
                    tool.CookedCheck(HandsOnOb);
                } // ���� else if�� �ٸ� �丮���� �߰� ���� 
                else
                {
                    // �ƹ��͵� �Ȱɷ����� �տ� �ٸ��� ����ų� ��ȣ�ۿ��� �ȵǴ°Ű���
                    yield break;
                }
            }
            else if (counter.PutOnOb != null && counter.PutOnOb.TryGetComponent(out Plate plate))
            {
                if(HandsOnOb.TryGetComponent(out Ingredient Ingre))
                {
                    if(plate.OnPlate(Ingre))
                    {
                        Debug.Log("��� �ֱ�");
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
            //ī���Ͱ� ��ó�� �ְ�, ī���Ͱ� ��ŷ���� �ƴҶ�(�Ϲ��̰���, ��ũ�뵵 ����ؾ��ϳ�)
            {
                //����޼ҵ�
                if (counter.CompareTag("TrashCan"))
                {
                    //��������
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

        yield return null;
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

        yield return null;
    }



}

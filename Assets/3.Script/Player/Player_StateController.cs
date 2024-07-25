using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �÷��̾� ��ȣ�ۿ� (��� / �丮 ���� ��� ��ȣ�ۿ�) 
public class Player_StateController : MonoBehaviour
{
    private Animator animator;

    //���� ���� ������Ʈ
    private GameObject nearOb;
    //�� ��ó ī����
    private GameObject nearcounter;
    //���� ���� ������Ʈ 
    private GameObject HandsOnOb;

    public GameObject HandsOnObject { get => HandsOnOb; }

    //�̰� �ν����Ϳ��� ���� �ؿ� ���̷��� Attach �־� ����ϱ�
    [SerializeField] private Transform Attachtransform;
    [SerializeField] private GameObject Cleaver;
    public GameObject CleaverOb { get => Cleaver; }

    //���� ��� �ִ��� 
    private bool isHolding = false;
    private bool isChop = false;

    public bool IsHolding { get => isHolding; private set => isHolding = value; }
    public bool IsChop { get => isChop; private set => isChop = value; }

    //�ڷ�ƾ �迭�� �����ѵ� 
    // Start�� �ҋ� ����� �迭�� ����
    // �� ������Ʈ���� �ϳ��ϳ� �����հ� �÷��̾ �� �� ����?
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

        // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
        if (coroutine == null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                coroutine = StartCoroutine(PlayerHodingChange(nearcounter, nearOb));
            }

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                //������ ���� 
                if (isHolding && HandsOnOb.CompareTag("Ingredients"))
                {
                    //ȸ���ϴ°� Movement��. �����°� ���⿡
                    Debug.Log("������");
                    ThrowIngredients();
                }
            }


            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                //�丮���� ��ȣ�ۿ�
                // ���� ��� 
                if (!IsHolding)
                {
                    Debug.Log("ó�� ��Ʈ�� ������");
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

        // ��ó ī���Ͱ� �ְ� ���� ���� ���°� �ƴ϶�� 
        if (isHolding)
        {
            DropObject(nearCount, nearob);
            yield break;
        }
        else
        {
            if (nearCount != null)
            {
                //ī���� ���� ������Ʈ�� �ִ��� ������ Ȯ�� 
                var counter = nearCount.transform.GetComponent<CounterController>();

                if (!counter.IsPutOn) //�ö� ���� �ʴٸ� ��ó ������Ʈ����
                {
                    if (counter.gameObject.CompareTag("Crate"))
                    {
                        var spawn = counter.transform.GetComponent<spawn_Ingredient>();
                        if (spawn != null)
                        {
                            TakeHandObject(spawn.PickAnim());
                            // ������ ��� ������Ʈ �ٷ� ���� �޼ҵ� �߰� �ʿ� 
                            yield break;
                        }
                    }
                    else // �ö� ���� �ʰ� ��ó ������Ʈ�� �ƴҶ� ��ó �ֿ�����
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
                    //ī���� ���� �� �ִ� ������Ʈ�� �ְ�, ������ ������ 
                    //if (counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Cooker") /* ��ȭ�� �±� �߰� �ʿ� */)
                    //{
                    //    TakeHandObject(counter.PutOnOb);
                    //}
                    ////���� �� �ִ� ������Ʈ�� ���� ���� ������ 
                    //else 
                    if (counter.ChoppingBoard != null)
                    {
                        Debug.Log("��������?");
                        TakeHandObject(counter.ChoppingBoard.transform.GetChild(1).gameObject);
                        counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else // �ƿ� ��������?? 
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
            // ��ó ī���Ͱ� ���ٸ�(���ٴ��̰���)
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
        // ��ó�� ī���Ͱ� �ִٸ� 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // ī���Ϳ� �ö󰣰� ���ٸ�  
            {
                if (counter.ChoppingBoard == null) // ī���Ϳ� ������ ���� ���̶�� 
                {
                    if (counter.gameObject.CompareTag("Crate")) // ��� �ֱ� ������ ������ ���� ��� �ڽ� ���̶�� �ڽ� ���� �ø��� 
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
                            //���ð� �ƴϸ� �� �н� 
                            return;
                        }
                        

                    }
                    else if (counter.transform.CompareTag("Plate_Return")) //�÷���Ʈ ���̴µ��� ���°͸� �� 
                    {
                        Debug.Log("�������");
                        return;
                    }
                    else if (counter.transform.CompareTag("TrashCan")) // ���������� ��Ḹ  
                    {
                        if(HandsOnOb.CompareTag("Plate") /*�����ⱸ �߰�*/)
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
                    else // ��� �ְ� ������ ���ٸ� �Ϲ� ī�����̱⶧���� �׳� �д�, ������ ��¦ �÷��ش� 
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
                else // ������ �ִٸ� 
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
            else // ī���Ϳ� �ö� �ִµ� ����Ϸ��� �ϸ� 
            {
                //�÷���Ʈ�� ������ 
                // ���ø�(��� X) / ����(��� O) / ��� + ���
                if ((counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Ingredients")) && HandsOnOb.CompareTag("Ingredients"))
                {
                    HandsOnOb.TryGetComponent(out Ingredient ingre);
                    HandsOnOb.TryGetComponent(out AddIngredientSpawn addingre);
                    HandsOnOb.TryGetComponent(out SphereCollider col);
                    if (ingre.OnPlate)
                    {
                        //�տ� ����� ���ø� 
                        if (counter.PutOnOb.CompareTag("Plate"))
                        {

                            //���� ��� �� 
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
                            else // ���� ��� X 
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
                        else // ������
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
                                Debug.Log("������");
                            }
                        }
                    }
                }


            }
        }
        else // ��ó�� ī���� ������ ���� �����ٴ� 
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

            // ���۸��ϰ� �������� ó���� ��ᰡ?
            // ������ �ִ���, ���� �ڽĿ� �±װ� ����� ������Ʈ�� �ִ��� + ��� �� �� ���ִ� boolean���� 
            if (counter.ChoppingBoard != null)
            {
                if (counter.ChoppingBoard.transform.childCount.Equals(2))
                {
                    if (counter.ChoppingBoard.transform.GetChild(1).gameObject.CompareTag("Ingredients") /* ��ᰡ ����ִ���  */)
                    {
                        counter.ChoppingBoard.transform.GetChild(1).gameObject.transform.TryGetComponent(out Ingredient ingre);

                        if (ingre != null && ingre.OnChopping)
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

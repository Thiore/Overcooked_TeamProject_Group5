using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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

    public GameObject HandsOnObject {  get => HandsOnOb; }

    //�̰� �ν����Ϳ��� ���� �ؿ� ���̷��� Attach �־� ����ϱ�
    [SerializeField] private Transform Attachtransform;

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


            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                //�丮���� ��ȣ�ۿ�
                // ���� ��� 
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

        // ��ó ī���Ͱ� �ְ� ���� ���� ���°� �ƴ϶�� 
        if(isHolding)
        {
            DropObject(nearCount, nearob);
            yield return new WaitForSeconds(0.3f);
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
                            Debug.Log("����");
                            // ������ ��� ������Ʈ �ٷ� ���� �޼ҵ� �߰� �ʿ� 
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
                    //ī���� ���� �� �ִ� ������Ʈ�� �ְ�, ������ ������ 
                    if (counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Cooker") /* ��ȭ�� �±� �߰� �ʿ� */)
                    {
                        TakeHandObject(counter.PutOnOb);
                    }
                    //���� �� �ִ� ������Ʈ�� ���� ���� ������ 
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
            // ��ó ī���Ͱ� ���ٸ�(���ٴ��̰���)
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
        // ��ó�� ī���Ͱ� �ִٸ� 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // ī���Ϳ� �ö󰣰� ���ٸ�  
            {
                if (counter.ChoppingBoard == null) // ī���Ϳ� ������ ���� ���̶�� 
                {
                    if (counter.gameObject.CompareTag("Crate")) // ��� �ڽ� ���� �ƴ϶� ����� �� 
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
                else // ������ �ִٸ� 
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
                Debug.Log("�������");
            }
            else // ī���Ϳ� �ö� �ִµ� ����Ϸ��� �ϸ� 
            {
                Debug.Log("�̹̿ö�����");
            }
        }
        else // ��ó�� ī���� ������ ���� �����ٴ� 
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
                        }
                        // ��� eCooked enum���� �ް� �븻�϶��� }
                    }
                }
            }
        }

        yield return null;
    }


    private void DropObject()
    {
        // ��ó�� ī���Ͱ� �ִٸ� 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // ī���Ϳ� �ö����� �ʴٸ�  
            {
                if (counter.ChoppingBoard == null) // ī���Ϳ� ������ ���� ���̶�� 
                {
                    HandsOnOb.transform.SetParent(nearcounter.transform);
                    HandsOnOb.transform.position = nearcounter.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChangePuton();
                    counter.PutOnOb = HandsOnOb;
                }
                else // ������ �ִٸ� 
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                    //������ ���� 
                    counter.ChangePuton();
                }

                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
            else // ī���Ϳ� �ö� �ִµ� ����Ϸ��� �ϸ� 
            {
                Debug.Log("�̹̿ö�����");
            }
        }
        else // ��ó�� ī���� ������ ���� �����ٴ� 
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
